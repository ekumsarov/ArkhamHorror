using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    [Serializable, JSONSerializable]
    public class GameTime
    {
        [SerializeField, JSONConvert]
        private int _maxDay;
        public int MaxDay => _maxDay;

        [SerializeField, JSONConvert]
        private int _currentDay = 0;
        public int CurrentDay => _currentDay;

        [SerializeField, JSONConvert]
        private GamePhase _currentPhase = GamePhase.Morning;
        public GamePhase CurrentPhase => _currentPhase;
        
    }
}