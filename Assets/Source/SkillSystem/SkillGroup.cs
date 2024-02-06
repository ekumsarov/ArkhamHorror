using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    public class SkillGroup : Bindable
    {
        [SerializeField, OnInspectorInit("InitializeSkills")]
        private Dictionary<SkillType, int> _skills;

        protected override void SerializeJson(JSONNode node)
        {
            InitializeSkills();
            JSONArray temp = node.AsArray;
            if (temp != null)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    SkillType checkSkill = Enum.Parse<SkillType>(temp["Skill"].Value);
                    if (_skills.ContainsKey(checkSkill))
                        _skills[checkSkill] = temp["Amount"].AsInt;
                }
            }
        }

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

