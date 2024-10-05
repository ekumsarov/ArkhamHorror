using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    [JSONSerializable]
    public class SkillGroup : SOBindable
    {
        [SerializeField, OnInspectorInit("InitializeSkills"), JSONConvert]
        private Dictionary<SkillType, int> _skills;

        private void InitializeSkills()
        {
            if (_skills != null)
                return;

            _skills = new Dictionary<SkillType, int>();
            foreach (var skill in Enum.GetNames(typeof(SkillType)))
            {
                _skills.Add(Enum.Parse<SkillType>(skill), 50);
            }
        }

        public bool CheckSkill(SkillType skill)
        {
            if (_skills.ContainsKey(skill))
            {
                return UnityEngine.Random.Range(0, 101) <= _skills[skill];
            }

            Debug.LogError("Has no skill: " + skill.ToString());
            return false;
        }
    }
}

