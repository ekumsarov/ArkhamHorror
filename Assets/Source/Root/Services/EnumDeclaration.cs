using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum NodeType
{
    Empty,
    EncounterWork
}

public enum LocationType
{

}

public enum GameState
{
    ManualControl
}

public enum GamePhase
{
    Morning,
    Day,
    Night
}

[Flags]
public enum CardType
{
    Player = 1,
    Enemy = 2,
    Item = 4
}

public enum EncounterActionType
{
    Dialogue,
    Resource,
    Event
}

public enum SkillType
{
    Strenght,
    Dexterity,
    Determination,
    Attentiveness,
    Knowledge
}

public enum SkillcheckQuantityRule
{
    Single,
    Group,
    Biggest,
    Lowest
}

public enum SkillcheckAmountRule
{
    Success,
    Amount
}