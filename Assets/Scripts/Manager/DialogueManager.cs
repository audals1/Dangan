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

    bool isDialogue = false;

    InteractionController interactionController;

    private void Start()
    {
        interactionController = FindObjectOfType<InteractionController>();
    }

    public void ShowDialogue()
    {
        txt_Dialogue.text = "";
        txt_Name.text = "";

        interactionController.HideUI();

        SetUI(true);
    }

    private void SetUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
    }
}
