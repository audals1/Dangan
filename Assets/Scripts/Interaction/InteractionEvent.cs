using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] bool isAutoEvent = false;
    [SerializeField] DialogueEvent dialogueEvent;

    public Dialogue[] GetDialogue()
    {
        DialogueEvent t_DialogueEvent = new DialogueEvent();

        t_DialogueEvent.dialogues = DatabaseManager.instance.GetDialogue((int)dialogueEvent.line.x, (int)dialogueEvent.line.y);

        for (int i = 0; i < t_DialogueEvent.dialogues.Length; i++)
        {
            t_DialogueEvent.dialogues[i].target = dialogueEvent.dialogues[i].target;
            t_DialogueEvent.dialogues[i].cameraType = dialogueEvent.dialogues[i].cameraType;
        }

        dialogueEvent.dialogues = t_DialogueEvent.dialogues;

        return dialogueEvent.dialogues;
    }

    public AppearType GetAppearType()
    {
        return dialogueEvent.appearType;
    }

    public GameObject[] GetTargets()
    {
        return dialogueEvent.appearObjects;
    }

    public GameObject GetNextEvent()
    {
        return dialogueEvent.go_NextEvent;
    }

     void Update()
    {
        if (isAutoEvent && DatabaseManager.isFinished && TransferManager.isFinished)
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            DialogueManager.isWaiting = true;

            if (GetAppearType() == AppearType.Appear) dialogueManager.SetAppearObjects(GetTargets());
            else if (GetAppearType() == AppearType.Disappear) dialogueManager.SetDisappearObjects(GetTargets());
            dialogueManager.SetNextEvent(GetNextEvent());
            dialogueManager.ShowDialogue(GetDialogue());

            gameObject.SetActive(false);
        }            
    }
}
