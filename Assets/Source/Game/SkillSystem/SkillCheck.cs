using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    [Serializable]
    public class SkillCheck 
    {
        [SerializeField]
        private SkillType _skill;
        public SkillType Skill => _skill;

        [SerializeField]
        private int _amountToCheck = 1;

        public int AmountToCheck => _amountToCheck;
    }
}

