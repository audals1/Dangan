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
    AppearSlide,
    DisappearSlide,
    ChangeSlide,

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
public class EventTiming
{
    public int eventNum; //캐릭터 등장 조건 이벤트 번호
    public int[] enventConditions; // 특정 이벤트를 봤다?를 체크 ex)봉란 등장하려면 이 배열에 1,3번 이벤트 번호가 들어가 있어야 등장한다
    public bool conditionFlag; // 해당 이벤트에 대한 캐릭터 등장 조건 ex)3번이벤트를 봤어야 봉란 등장? 그럼 3번 이벤트의 conditionFlag는 true여야 한다
    public int eventEndNum; //이 이벤트를 봤으면 캐릭터 퇴장
}

public enum AppearType
{
    None,
    Appear,
    Disappear,
}


[System.Serializable]
public class DialogueEvent
{
    public string name;
    public EventTiming eventTiming;

    public Dialogue[] dialogues;
    public Vector2 line;

    [Space]
    public Dialogue[] dialoguesAfter;
    public Vector2 lineAfter;

    [Space][Space][Space]
    public AppearType appearType;
    public GameObject[] appearObjects;
    [Space]
    public GameObject go_NextEvent;

    public bool isSame;
}
