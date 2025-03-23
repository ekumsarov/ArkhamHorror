using EVI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class LogicNodeReflection
{
    /// <summary>
    /// Возвращает список ValueDropdownItem для всех конкретных типов, наследующихся от LogicNode.
    /// </summary>
    public static IEnumerable<ValueDropdownItem<Type>> GetAllLogicNodeTypes()
    {
        var baseType = typeof(LogicNode);

        // Сканируем ВСЕ сборки текущего AppDomain
        // Если хотите отфильтровать, например, только свою Assembly,
        // можно взять конкретную сборку: typeof(LogicNode).Assembly
        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Ищем типы, которые унаследованы от LogicNode, не абстрактные и не интерфейсы
        var derivedTypes = allAssemblies
            .SelectMany(asm => SafeGetTypes(asm))  // Чтобы не падать на ReflectionTypeLoadException
            .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t))
            .ToList();

        // Формируем ValueDropdownItem, где текст = t.Name, а значение = сам Type
        foreach (var type in derivedTypes)
        {
            yield return new ValueDropdownItem<Type>(type.Name, type);
        }
    }

    /// <summary>
    /// Безопасная версия GetTypes(), чтобы не вызывать исключения, если сборка не может отразить некоторые типы.
    /// </summary>
    private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch
        {
            return Array.Empty<Type>();
        }
    }
}
