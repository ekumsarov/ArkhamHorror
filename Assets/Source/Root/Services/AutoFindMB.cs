using System.Reflection;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;



#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AutoFindMB : MonoBehaviour
{

#if UNITY_EDITOR
    private void OnValidate()
    {
        InitializeComponents();
    }
#endif

    private void InitializeComponents()
    {
        var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<AutoFindComponentAttribute>();
            if (attribute != null && field.GetValue(this) == null)
            {
                Type fieldType = field.FieldType;

                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // Работа с полем типа List<T>
                    Type itemType = fieldType.GetGenericArguments()[0];
                    var components = attribute.SearchInChildren ? GetComponentsInChildren(itemType) : GetComponents(itemType);

                    IList list = (IList)Activator.CreateInstance(fieldType);

                    foreach (var component in components)
                    {
                        list.Add(component);
                    }

                    field.SetValue(this, list);
                }
                else
                {
                    // Работа с одиночным компонентом
                    var componentType = field.FieldType;
                    object component = attribute.SearchInChildren ? GetComponentInChildren(componentType) : GetComponent(componentType);

                    if (component != null)
                    {
                        field.SetValue(this, component);
                    }
                }
            }
        }
    }
}
