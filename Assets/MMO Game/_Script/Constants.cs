using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public static int SelectedCharacterModel;
    public static string MaleSetting;
    public static string FemaleSetting;
}

public enum CameraPerspective
{
    ThirdPerson,
    FirstPerson
}

public enum PlayerStates
{
    Idle,
    Walking,
    Running,
    Jumping
}