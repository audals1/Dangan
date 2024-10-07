using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] bool isAutoEvent = false;
    [SerializeField] DialogueEvent[] dialogueEvent;
    int currentEventNum = 0;

    void Start()
    {
        bool t_Flag = CheckEvent();
        gameObject.SetActive(t_Flag);
    }

    bool CheckEvent()
    {
        bool t_Flag = true;

        for(int x = 0; x < dialogueEvent.Length; x++)
        {

            t_Flag = true;

            // 캐릭터 등장조건과 일치하지 않을 경우 등장시키지 않음
            for (int i = 0; i < dialogueEvent[x].eventTiming.enventConditions.Length; i++)
            {
                if (DatabaseManager.instance.eventflags[dialogueEvent[x].eventTiming.enventConditions[i]] != dialogueEvent[x].eventTiming.conditionFlag)
                {
                    t_Flag = false;
                    break;
                }
            }

            // 등장 조건과 관계 없이 퇴장 조건과 일치할 경우 등장시키지 않음
            if (DatabaseManager.instance.eventflags[dialogueEvent[x].eventTiming.eventEndNum])
            {
                t_Flag = false;
            }

            if (t_Flag)
            {
                currentEventNum = x;
                break;
            }
        }

        return t_Flag;
    }

    public Dialogue[] GetDialogue()
    {
        if (DatabaseManager.instance.eventflags[dialogueEvent[currentEventNum].eventTiming.eventEndNum]) return null;
        // 상호작용 전 대화
        if (!DatabaseManager.instance.eventflags[dialogueEvent[currentEventNum].eventTiming.eventNum] || dialogueEvent[currentEventNum].isSame)
        {
            DatabaseManager.instance.eventflags[dialogueEvent[currentEventNum].eventTiming.eventNum] = true;
            dialogueEvent[currentEventNum].dialogues = SettingDialogue(dialogueEvent[currentEventNum].dialogues, (int)dialogueEvent[currentEventNum].line.x, (int)dialogueEvent[currentEventNum].line.y);
            return dialogueEvent[currentEventNum].dialogues;
        }

        // 상호작용 후 대화
        else 
        {
            dialogueEvent[currentEventNum].dialoguesAfter = SettingDialogue(dialogueEvent[currentEventNum].dialoguesAfter, (int)dialogueEvent[currentEventNum].lineAfter.x, (int)dialogueEvent[currentEventNum].lineAfter.y);
            return dialogueEvent[currentEventNum].dialoguesAfter;
        }
    }

    Dialogue[] SettingDialogue(Dialogue[] p_Dialogue, int p_LineX, int p_LineY)
    {
        Dialogue[] t_Dialogues = DatabaseManager.instance.GetDialogue(p_LineX, p_LineY);

        for (int i = 0; i < t_Dialogues.Length; i++)
        {
            t_Dialogues[i].target = p_Dialogue[i].target;
            t_Dialogues[i].cameraType = p_Dialogue[i].cameraType;
        }

        return t_Dialogues;
    } 

    public AppearType GetAppearType()
    {
        return dialogueEvent[currentEventNum].appearType;
    }

    public GameObject[] GetTargets()
    {
        return dialogueEvent[currentEventNum].appearObjects;
    }

    public GameObject GetNextEvent()
    {
        return dialogueEvent[currentEventNum].go_NextEvent;
    }

    public int GetEventNum()
    {
        CheckEvent();
        return dialogueEvent[currentEventNum].eventTiming.eventNum;
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
