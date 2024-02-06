using EVI.Game;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    [Serializable]
    public class CardCell 
    {
        [SerializeField]
        private CardType _cellType;

        [SerializeField]
        public GameCard Card;
    }
}
