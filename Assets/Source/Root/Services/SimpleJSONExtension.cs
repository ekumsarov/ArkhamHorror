using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public static class SimpleJSONExtensions
{
    public static JSONNode ObjectToJson(this object obj)
    {
        if (obj == null)
            return JSONNull.CreateOrGet();

        var jsonObject = new JSONObject
        {
            ["_type"] = obj.GetType().AssemblyQualifiedName // Сохранение типа объекта в JSON
        };

        switch (obj)
        {
            case GameObject go:
                jsonObject["PrefabName"] = go.name;
                jsonObject["PrefabType"] = go.GetType().FullName;
                break;

            case MonoBehaviour mb:
                jsonObject["ComponentType"] = mb.GetType().AssemblyQualifiedName;
                jsonObject["GameObjectName"] = mb.gameObject.name;
                break;

            case TrailRenderer trail:
                jsonObject["ComponentType"] = trail.GetType().AssemblyQualifiedName;
                jsonObject["GameObjectName"] = trail.gameObject.name;
                break;

            case Sprite sprite:
                return SpriteToJson(sprite);

            case string str:
                return new JSONString(str);

            case char chr:
                return new JSONString(chr.ToString());

            case bool boolean:
                return new JSONBool(boolean);

            case int _:
            case long _:
            case short _:
            case sbyte _:
            case byte _:
            case uint _:
            case ulong _:
            case ushort _:
                return new JSONNumber(Convert.ToInt64(obj));

            case float _:
            case double _:
            case decimal _:
                return new JSONNumber(Convert.ToDouble(obj));

            case IList list:
                var jsonArray = new JSONArray();
                foreach (var item in list)
                    jsonArray.Add(ObjectToJson(item));
                return jsonArray;

            case LocalizedString localizedString:
                return LocalizedStringToJson(localizedString);

            case IDictionary dictionary:
                foreach (var key in dictionary.Keys)
                    jsonObject[key.ToString()] = ObjectToJson(dictionary[key]);
                break;

            default:
                if (obj.GetType().GetCustomAttribute<JSONSerializableAttribute>() != null)
                {
                    var members = GetFieldsAndPropertiesWithAttribute<JSONConvertAttribute>(obj.GetType());
                    foreach (var kvp in members)
                    {
                        jsonObject[kvp.Key] = ObjectToJson(GetValue(kvp.Value, obj));
                    }
                }
                else
                {
                    return obj.ToString();
                }
                break;
        }

        return jsonObject;
    }

    public static object JsonToObject(this JSONNode node, Type type)
    {
        if (type == null)
        {
            Debug.LogError("Тип данных для десериализации не определен.");
            return null;
        }

        if (typeof(IList).IsAssignableFrom(type))
        {
            var listType = type.GetGenericArguments()[0];
            var list = (IList)CreateInstance(type);
            foreach (var item in node.AsArray)
            {
                list.Add(JsonToObject(item, listType));
            }
            return list;
        }

        if (typeof(IDictionary).IsAssignableFrom(type))
        {
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];
            var dict = (IDictionary)CreateInstance(type);
            foreach (var key in node.Keys)
            {
                dict.Add(Convert.ChangeType(key, keyType), JsonToObject(node[key], valueType));
            }
            return dict;
        }

        if (type == typeof(Sprite))
            return JsonToSprite(node);

        if (type == typeof(string))
            return node.Value;

        if (type == typeof(bool))
            return node.AsBool;

        if (type == typeof(LocalizedString))
            return JsonToLocalizedString(node);

        if (type.IsPrimitive)
        {
            return type switch
            {
                _ when type == typeof(int) => node.AsInt,
                _ when type == typeof(long) => node.AsLong,
                _ when type == typeof(float) => node.AsFloat,
                _ when type == typeof(double) => node.AsDouble,
                _ when type == typeof(byte) => (byte)node.AsInt,
                _ when type == typeof(short) => (short)node.AsInt,
                _ when type == typeof(uint) => (uint)node.AsInt,
                _ when type == typeof(ulong) => (ulong)node.AsLong,
                _ when type == typeof(ushort) => (ushort)node.AsInt,
                _ => null
            };
        }

        if (type.IsEnum)
            return Enum.Parse(type, node.Value);

        if (type == typeof(GameObject))
        {
            var prefabName = node["PrefabName"];
            var prefabTypeName = node["_type"];
            var prefabType = Type.GetType(prefabTypeName);

            if (prefabType != null)
            {
                var prefab = LoadPrefabByName(prefabName, prefabType);
                if (prefab != null)
                {
                    return prefab;
                }
            }

            Debug.LogError($"Failed to load GameObject prefab with name {prefabName} and type {prefabTypeName}");
            return null;
        }

        var typeName = node["_type"]?.Value;
        if (typeName != null)
        {
            type = Type.GetType(typeName);
        }

        if (type == null)
        {
            Debug.LogError("Не удалось определить тип для десериализации.");
            return null;
        }

        if (type.IsSubclassOf(typeof(MonoBehaviour)) || type == typeof(MonoBehaviour))
        {
            var gameObjectName = node["GameObjectName"];
            var componentTypeName = node["ComponentType"];
            var componentType = Type.GetType(componentTypeName);

            var gameObject = LoadPrefabByName(gameObjectName, typeof(GameObject));
            if (gameObject != null && componentType != null)
            {
                var component = gameObject.GetComponent(componentType) ?? gameObject.AddComponent(componentType);
                return component;
            }

            Debug.LogError($"Failed to deserialize {componentTypeName} to {componentType}");
            return null;
        }

        if (type.GetCustomAttribute<JSONSerializableAttribute>() != null)
        {
            var obj = CreateInstance(type);
            var members = GetFieldsAndPropertiesWithAttribute<JSONConvertAttribute>(type);
            foreach (var kvp in members)
            {
                var name = kvp.Key;
                var member = kvp.Value;
                if (node[name] != null)
                {
                    var deserializedValue = JsonToObject(node[name], GetMemberType(member));
                    if (deserializedValue != null && GetMemberType(member).IsAssignableFrom(deserializedValue.GetType()))
                    {
                        SetValue(member, obj, deserializedValue);
                    }
                    else
                    {
                        Debug.LogError($"Failed to deserialize {name} to {GetMemberType(member)}");
                    }
                }
            }
            return obj;
        }

        return node.ToString();
    }

    private static Dictionary<string, MemberInfo> GetFieldsAndPropertiesWithAttribute<TAttribute>(Type type) where TAttribute : Attribute
    {
        var result = new Dictionary<string, MemberInfo>();

        while (type != null && type != typeof(object))
        {
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                               .Where(field => field.GetCustomAttributes<TAttribute>().Any());

            var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                                 .Where(prop => prop.GetCustomAttributes<TAttribute>().Any());

            foreach (var field in fields)
            {
                result[field.Name] = field;
            }

            foreach (var prop in properties)
            {
                result[prop.Name] = prop;
            }

            type = type.BaseType;
        }

        return result;
    }

    private static object GetValue(MemberInfo member, object obj)
    {
        return member switch
        {
            FieldInfo field => field.GetValue(obj),
            PropertyInfo prop => prop.GetValue(obj),
            _ => null
        };
    }

    private static void SetValue(MemberInfo member, object obj, object value)
    {
        switch (member)
        {
            case FieldInfo field:
                field.SetValue(obj, value);
                break;
            case PropertyInfo prop:
                prop.SetValue(obj, value);
                break;
        }
    }

    private static Type GetMemberType(MemberInfo member)
    {
        return member switch
        {
            FieldInfo field => field.FieldType,
            PropertyInfo prop => prop.PropertyType,
            _ => null
        };
    }

    private static JSONNode LocalizedStringToJson(LocalizedString localizedString)
    {
        return new JSONObject
        {
            ["TableReference"] = localizedString.TableReference.TableCollectionNameGuid,
            ["TableEntryReference"] = localizedString.TableEntryReference.Key.ToString(),
            ["ReferenceType"] = localizedString.TableEntryReference.ReferenceType.ToString()
        };
    }

    private static LocalizedString JsonToLocalizedString(JSONNode node)
    {
        var tableReference = Guid.Parse(node["TableReference"].Value);
        var referenceType = Enum.Parse<TableEntryReference.Type>(node["ReferenceType"]);

        return new LocalizedString
        {
            TableReference = tableReference,
            TableEntryReference = node["TableEntryReference"].Value
        };
    }

    private static GameObject LoadPrefabByName(string name, Type type)
    {
        var prefabs = Resources.LoadAll<GameObject>("");
        foreach (var loadedPrefab in prefabs)
        {
            if (loadedPrefab.name == name && type.IsAssignableFrom(loadedPrefab.GetType()))
            {
                return loadedPrefab;
            }
        }

        Debug.LogError($"Префаб с именем {name} и типом {type} не найден.");
        return null;
    }

    private static JSONNode SpriteToJson(Sprite sprite)
    {
        var jsonObject = new JSONObject();

        if (sprite == null)
        {
            jsonObject.Add("ResourcePath", null);
            return jsonObject;
        }


        jsonObject["SpriteName"] = sprite.name;
        jsonObject["ResourcePath"] = sprite.name; // Assuming sprite is loaded from Resources with name as path
        return jsonObject;
    }

    private static Sprite JsonToSprite(JSONNode node)
    {
        var resourcePath = node["ResourcePath"];

        if (resourcePath == null)
            return null;

        return Resources.Load<Sprite>(resourcePath);
    }

    private static object CreateInstance(Type type)
    {
        if (type == null)
        {
            Debug.LogError("Тип данных для создания не определен.");
            return null;
        }

        if (typeof(ScriptableObject).IsAssignableFrom(type))
        {
            return ScriptableObject.CreateInstance(type);
        }

        return Activator.CreateInstance(type, true);
    }
}
