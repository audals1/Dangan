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

    InteractionController interactionController;

    private void Start()
    {
        interactionController = FindObjectOfType<InteractionController>();
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
                            StartCoroutine(TypeWriter());
                        }
                        else
                        {
                            EndDialogue();
                        }
                    }
                }
            }
        }
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDialogue = true;
        txt_Dialogue.text = "";
        txt_Name.text = "";

        interactionController.SettingMouseUI(false);

        dialogues = p_dialogues;

        StartCoroutine(TypeWriter());
    }

    void EndDialogue()
    {
        isDialogue = false;
        lineCount = 0;
        contextCount = 0;
        dialogues = null;
        isNext = false;
        interactionController.SettingMouseUI(true);
        SettingDialogueUI(false);
    }

    IEnumerator TypeWriter()
    {
        SettingDialogueUI(true);

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount]; //'를 ,로 바꾸기
        t_ReplaceText = t_ReplaceText.Replace("'", ",");
        txt_Name.text = dialogues[lineCount].name;

        for (int i = 0; i < t_ReplaceText.Length; i++)
        {
            txt_Dialogue.text += t_ReplaceText[i];
            yield return new WaitForSeconds(typeDelay);
        }

        txt_Dialogue.text = t_ReplaceText;

        isNext = true;
    }

    private void SettingDialogueUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
    }
}
