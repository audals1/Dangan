using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType
{
    Targetting,
    Reset,
    FadeOut,
    FadeIn,
    FlashOut,
    FlashIn,
    ShowCutScene,
    HideCutScene,
}

[System.Serializable]
public class Dialogue
{
    [Header("카메라 타겟 대상")]
    public CameraType cameraType;
    public Transform target;

    [HideInInspector]
    public string name;

    [HideInInspector]
    public string[] contexts;

    [HideInInspector]
    public string[] spriteName;

    [HideInInspector]
    public string[] voiceName;
}

[System.Serializable]
public class DialogueEvent
{
    public string name;

    public Vector2 line;
    public Dialogue[] dialogues;
}
