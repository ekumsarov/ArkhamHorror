using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class TypeArrayComparer : IEqualityComparer<Type[]>
{
    public bool Equals(Type[] x, Type[] y)
    {
        if (x.Length != y.Length)
            return false;

        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
                return false;
        }

        return true;
    }

    public static bool ContainsParameters(Type[] first, Type[] from)
    {
        if (first.Length != from.Length)
            return false;

        for (int i = 0; i < from.Length; i++)
        {
            if (first.Any(check => check == from[i] || check == from[i].BaseType) == false)
                return false;
        }

        return true;
    }

    public static bool ContainsParametersWithConversetion(Type[] first, in Type[] from)
    {
        if (first.Length != from.Length)
            return false;

        for (int i = 0; i < from.Length; i++)
        {
            foreach (var check in first)
            {
                if (check == from[i].BaseType)
                    from[i] = from[i].BaseType;

                if (check != from[i])
                    return false;
            }
        }

        return true;
    }

    public static Type[] CreateFrom(params object[] args)
    {
        Type[] argTypes = new Type[args.Length];
        for (int i = 0; i < args.Length; i++)
        {
            argTypes[i] = args[i].GetType();
        }

        return argTypes;
    }

    public int GetHashCode(Type[] obj)
    {
        int hash = 17;
        foreach (var type in obj)
        {
            hash = hash * 23 + type.GetHashCode();
        }
        return hash;
    }
}
