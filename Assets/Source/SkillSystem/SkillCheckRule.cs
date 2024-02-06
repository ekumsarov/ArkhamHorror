using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace EVI
{
	[Serializable]
	public class SkillCheckRule
	{
		[SerializeField]
		private SkillcheckQuantityRule _quantityRule;

		[SerializeField]
		private SkillcheckAmountRule _amountRule;

        [SerializeField]
		private List<SkillType> _skills;

		[SerializeField, ShowIf("@_amountRule == SkillcheckAmountRule.Amount")]
		private int _rollAmount;
	}
}
