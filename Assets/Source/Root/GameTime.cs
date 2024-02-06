using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    [Serializable]
    public class GameTime
    {
        [SerializeField]
        private int _maxDay;
        public int MaxDay => _maxDay;

        [SerializeField]
        private int _currentDay = 0;
        public int CurrentDay => _currentDay;

        
    }
}