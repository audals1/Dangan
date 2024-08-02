using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[] { '\n' }); // 개행으로 구분

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' }); // ,로 구분

            Dialogue dialogue = new Dialogue(); // 대화 객체 생성

            dialogue.name = row[1]; // 이름
            List<string> contextList = new List<string>(); // 대화 내용
            do
            {
                contextList.Add(row[2]); //엑셀 데이터중 대화부분을 리스트에 추가
                if (++i < data.Length)
                    row = data[i].Split(new char[] { ',' }); // 다음 대화 내용으로 이동
                else break;
            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray(); // 대화 내용 배열로 변환

            dialogueList.Add(dialogue); // 대화 객체 리스트에 추가
        }

        return dialogueList.ToArray();
    }

    private void Start()
    {
        Parse("prologue");
    }

}
