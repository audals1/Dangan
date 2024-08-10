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

    bool isDialogue = false; // ��ȭ ���� ��� true
    bool isNext = false; // Ű �Է� ���

    int lineCount = 0; // ��ȭ ī��Ʈ
    int contextCount = 0; // ��� ī��Ʈ

    [Header("�ؽ�Ʈ ��� ������")]
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

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount]; //'�� ,�� �ٲٱ�
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
