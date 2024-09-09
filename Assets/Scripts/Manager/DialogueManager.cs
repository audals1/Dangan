using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;

    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;

    Dialogue[] dialogues;

    bool isDialogue = false; // 대화 중일 경우 true
    bool isNext = false; // 키 입력 대기

    int lineCount = 0; // 대화 카운트
    int contextCount = 0; // 대사 카운트

    [Header("텍스트 출력 딜레이")]
    [SerializeField] float typeDelay = 0.1f;


    // 이벤트 끝나면 등장/퇴장시킬 오브젝트들
    GameObject[] go_AppearObjects;
    byte appearTypeNumber = 0;
    const byte None = 0, APPEAR = 1, DISAPPEAR = 2;

    public void SetAppearObjects(GameObject[] p_Targets)
    {
        go_AppearObjects = p_Targets;
        appearTypeNumber = APPEAR;
    }

    public void SetDisappearObjects(GameObject[] p_Targets)
    {
        go_AppearObjects = p_Targets;
        appearTypeNumber = DISAPPEAR;
    }

    InteractionController interactionController;
    CameraController cameraController;
    SpriteManager spriteManager;
    SplashManager splashManager;
    CutSceneManager cutSceneManager;
    SlideManager slideManager;

    private void Start()
    {
        interactionController = FindObjectOfType<InteractionController>();
        cameraController = FindObjectOfType<CameraController>();
        spriteManager = FindObjectOfType<SpriteManager>();
        splashManager = FindObjectOfType<SplashManager>();
        cutSceneManager = FindObjectOfType<CutSceneManager>();
        slideManager = FindObjectOfType<SlideManager>();
    }

    private void Update()
    {
        if (isDialogue)
        {
            if(isNext)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isNext = false;
                    txt_Dialogue.text = "";
                    if(++contextCount < dialogues[lineCount].contexts.Length)
                        StartCoroutine(TypeWriter());
                    else
                    {
                        contextCount = 0;
                        if(++lineCount < dialogues.Length)
                        {
                            StartCoroutine(CameraTargetSettingType());
                        }
                        else
                        {
                            StartCoroutine(EndDialogue());
                        }
                    }
                }
            }
        }
    }

    public void ShowDialogue(Dialogue[] p_Dialogues)
    {
        isDialogue = true;
        txt_Dialogue.text = "";
        txt_Name.text = "";

        interactionController.SettingMouseUI(false);

        dialogues = p_Dialogues;
        cameraController.CamOriginSetting();
        StartCoroutine(CameraTargetSettingType());
    }

    IEnumerator CameraTargetSettingType()
    {
        switch (dialogues[lineCount].cameraType)
        {
            case CameraType.FadeIn: SettingDialogueUI(false); splashManager.isFinished = false; StartCoroutine(splashManager.FadeIn(false, true)); yield return new WaitUntil(() => splashManager.isFinished); break;
            case CameraType.FadeOut: SettingDialogueUI(false); splashManager.isFinished = false; StartCoroutine(splashManager.FadeOut(false, true)); yield return new WaitUntil(() => splashManager.isFinished); break;
            case CameraType.FlashIn: SettingDialogueUI(false); splashManager.isFinished = false; StartCoroutine(splashManager.FadeIn(true, true)); yield return new WaitUntil(() => splashManager.isFinished); break;
            case CameraType.FlashOut: SettingDialogueUI(false); splashManager.isFinished = false; StartCoroutine(splashManager.FadeOut(true, true)); yield return new WaitUntil(() => splashManager.isFinished); break;
            case CameraType.Targetting: cameraController.CameraTargetting(dialogues[lineCount].target); break;
            case CameraType.Reset: cameraController.CameraTargetting(null, 0.01f, true, false); break;
            case CameraType.ShowCutScene: SettingDialogueUI(false); CutSceneManager.isFinished = false; StartCoroutine(cutSceneManager.CutSceneCoroutine(dialogues[lineCount].spriteName[contextCount], true)); yield return new WaitUntil(() => CutSceneManager.isFinished); break;
            case CameraType.HideCutScene: SettingDialogueUI(false); CutSceneManager.isFinished = false; StartCoroutine(cutSceneManager.CutSceneCoroutine(null, false)); yield return new WaitUntil(() => CutSceneManager.isFinished); cameraController.CameraTargetting(dialogues[lineCount].target); break;
            case CameraType.AppearSlide: SlideManager.isFinished = false; StartCoroutine(slideManager.AppearSlide(SplitSlideCGName())); yield return new WaitUntil(() => SlideManager.isFinished); cameraController.CameraTargetting(dialogues[lineCount].target); break;
            case CameraType.DisappearSlide: SlideManager.isFinished = false; StartCoroutine(slideManager.DisappearSlide()); yield return new WaitUntil(() => SlideManager.isFinished); cameraController.CameraTargetting(dialogues[lineCount].target); break;
            case CameraType.ChangeSlide: SlideManager.isChanged = false; StartCoroutine(slideManager.ChangeSlide(SplitSlideCGName())); yield return new WaitUntil(() => SlideManager.isChanged); cameraController.CameraTargetting(dialogues[lineCount].target); break;  
            default:
                break;
        }

        StartCoroutine(TypeWriter());
    }

    string SplitSlideCGName()
    {
        string t_Text = dialogues[lineCount].spriteName[contextCount];
        string[] t_SplitText = t_Text.Split(new char[] { '/' });
        if(t_SplitText.Length <= 1)
            return t_SplitText[0];
        else
            return t_SplitText[1];
    }

    IEnumerator EndDialogue()
    {
        SettingDialogueUI(false);
        if(cutSceneManager.CheckCutScene())
        {
            CutSceneManager.isFinished = false;
            StartCoroutine(cutSceneManager.CutSceneCoroutine(null, false));
            yield return new WaitUntil(() => CutSceneManager.isFinished);
        }

        AppearOrDisappearObjects();

        yield return new WaitUntil(() => Spin.isFinished);

        isDialogue = false;
        lineCount = 0;
        contextCount = 0;
        dialogues = null;
        isNext = false;
        cameraController.CameraTargetting(null, 0.01f, true, true);
        SettingDialogueUI(false);
    }

    void AppearOrDisappearObjects()
    {
        if (go_AppearObjects != null)
        {
            Spin.isFinished = false;
            for (int i = 0; i < go_AppearObjects.Length; i++)
            {
                if (appearTypeNumber == APPEAR)
                {
                    go_AppearObjects[i].SetActive(true);
                    StartCoroutine(go_AppearObjects[i].GetComponent<Spin>().SetAppearOrDisappear(true));
                }
                else if (appearTypeNumber == DISAPPEAR)
                {
                    StartCoroutine(go_AppearObjects[i].GetComponent<Spin>().SetAppearOrDisappear(false));
                }
            }
        }

        go_AppearObjects = null;
        appearTypeNumber = None;
    }

    void ChangeSprite()
    {
        if (dialogues[lineCount].target != null)
        {
            if (dialogues[lineCount].spriteName[contextCount] == "")
                return;

            StartCoroutine(spriteManager.SpriteChangeCoroutine(dialogues[lineCount].target, dialogues[lineCount].spriteName[contextCount].Split(new char[] { '/' })[0]));
        }
    }

    void PlaySound()
    {
        if (dialogues[lineCount].voiceName[contextCount] == "")
            return;

        SoundManager.instance.PlaySound(dialogues[lineCount].voiceName[contextCount], 2);
    }

    IEnumerator TypeWriter()
    {
        SettingDialogueUI(true);
        ChangeSprite();
        PlaySound();

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount]; //'를 ,로 바꾸기
        t_ReplaceText = t_ReplaceText.Replace("'", ",");
        t_ReplaceText = t_ReplaceText.Replace("\\n", "\n");

        bool t_white = false, t_yellow = false, t_cyan = false;
        bool t_ignore = false;

        for (int i = 0; i < t_ReplaceText.Length; i++)
        {
            switch (t_ReplaceText[i])
            {
                case 'ⓦ': t_white = true; t_yellow = false; t_cyan = false; t_ignore = true; break;
                case 'ⓨ': t_white = false; t_yellow = true; t_cyan = false; t_ignore = true; break;
                case 'ⓒ': t_white = false; t_yellow = false; t_cyan = true; t_ignore = true; break;
                case '①': StartCoroutine(splashManager.Splash()); SoundManager.instance.PlaySound("Emotion1", 1); t_ignore = true; break;
                case '②': StartCoroutine(splashManager.Splash()); SoundManager.instance.PlaySound("Emotion2", 1); t_ignore = true; break;
            }

            string t_letter = t_ReplaceText[i].ToString();

            if (!t_ignore)
            {
                if (t_white)
                {
                    t_letter = "<color=#ffffff>" + t_letter + "</color>";
                }

                else if (t_yellow)
                {
                    t_letter = "<color=#ffff00>" + t_letter + "</color>";
                }

                else if (t_cyan)
                {
                    t_letter = "<color=#00ffff>" + t_letter + "</color>";
                }

                txt_Dialogue.text += t_letter;
            }

            t_ignore = false;

            yield return new WaitForSeconds(typeDelay);
        }

        isNext = true;
    }

    private void SettingDialogueUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);

        if (p_flag)
        {
            if (dialogues[lineCount].name == "")
            {
                go_DialogueNameBar.SetActive(false);
            }
            else
            {
                go_DialogueNameBar.SetActive(true);
                txt_Name.text = dialogues[lineCount].name;
            }
        }

        else
        {
            go_DialogueNameBar.SetActive(false);
        }
    }
}
