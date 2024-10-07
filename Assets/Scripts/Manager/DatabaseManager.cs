using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [SerializeField] string csvFileName;

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public bool[] eventflags = new bool[100];

    public static bool isFinished = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DialogueParser parser = GetComponent<DialogueParser>();
            Dialogue[] dialogues = parser.Parse(csvFileName);
            for (int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(i + 1, dialogues[i]);
            }
            isFinished = true;
        }
    }

    public Dialogue[] GetDialogue(int _startNum, int _endNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        for (int i = 0; i <= _endNum - _startNum; i++)
        {
            dialogueList.Add(dialogueDic[_startNum + i]);
        }

        return dialogueList.ToArray();
    }
}
