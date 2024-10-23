using System.Collections.Generic;
using EVI.Game;
using UnityEngine;
using System.Linq;
using System;

namespace EVI
{
    public static class TypeSelector
    {
        public static IEnumerable<string> GetAllTypeIDs()
        {
            return new List<Type>() { typeof(GameCard), typeof(Location) }.Select(t => t.FullName).ToList();
        }

        public static Type GetTypeByName(string typeName)
        {
            return Type.GetType(typeName);
        }
    }
}