using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[] { '\n' }); // �������� ����

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' }); // ,�� ����

            Dialogue dialogue = new Dialogue(); // ��ȭ ��ü ����

            dialogue.name = row[1]; // �̸�
            List<string> contextList = new List<string>(); // ��ȭ ����
            do
            {
                contextList.Add(row[2]); //���� �������� ��ȭ�κ��� ����Ʈ�� �߰�
                if (++i < data.Length)
                    row = data[i].Split(new char[] { ',' }); // ���� ��ȭ �������� �̵�
                else break;
            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray(); // ��ȭ ���� �迭�� ��ȯ

            dialogueList.Add(dialogue); // ��ȭ ��ü ����Ʈ�� �߰�
        }

        return dialogueList.ToArray();
    }

    private void Start()
    {
        Parse("prologue");
    }

}
