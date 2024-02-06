using EVI.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
	public interface ISkillChecking 
	{
		public bool Check(List<GameCard> cards);

		public bool Check(GameCard card);
	}
}
