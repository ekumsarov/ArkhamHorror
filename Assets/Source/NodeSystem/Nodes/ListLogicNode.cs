using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace EVI
{
    [Serializable]
    public partial class ListLogicNode : LogicNode
    {
        // Предположим, у нас есть список саб-нод:
        [SerializeField]
        private List<LogicNode> _nodes = new List<LogicNode>();
    }
}
