using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string p_CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(p_CSVFileName);

        string[] data = csvData.text.Split(new char[] { '\n' }); // 개행으로 구분

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' }); // ,로 구분

            Dialogue dialogue = new Dialogue(); // 대화 객체 생성

            dialogue.name = row[1];

            List<string> contextList = new List<string>();
            List<string> spriteList = new List<string>();
            List<string> voiceList = new List<string>();

            do
            {
                contextList.Add(row[2]);
                spriteList.Add(row[3]);
                voiceList.Add(row[4]);
                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }
            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray();
            dialogue.spriteName = spriteList.ToArray();
            dialogue.voiceName = voiceList.ToArray();
            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }

    private void Start()
    {
        Parse("prologue");
    }
}
