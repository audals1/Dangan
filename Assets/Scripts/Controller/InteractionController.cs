using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera Cam;

    RaycastHit hitInfo;

    [SerializeField] GameObject go_NormalCrosshair;
    [SerializeField] GameObject go_InteractionCrosshair;
    [SerializeField] GameObject go_Crosshair;
    [SerializeField] GameObject go_Cursor;

    bool isContact = false;
    bool isInteract = false;

    [SerializeField] ParticleSystem ps_QuestionEffect;
    [SerializeField] ParticleSystem ps_ColliderEffect;

    DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckObject();
        ClickLeftBtn();
    }

    public void HideUI()
    {
        go_Crosshair.SetActive(false);
        go_Cursor.SetActive(false);
    }

    void CheckObject()
    {
        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        if(Physics.Raycast(Cam.ScreenPointToRay(t_MousePos), out hitInfo, 100f))
        {
            Contact();
        }
        else
        {
            NotContact();
        }
    }

    void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            if (isContact) return;
            isContact = true;
            go_InteractionCrosshair.SetActive(true);
            go_NormalCrosshair.SetActive(false);
        }
        else NotContact();
    }

    void NotContact()
    {
       if (!isContact) return;
       isContact = false;
       go_InteractionCrosshair.SetActive(false);
       go_NormalCrosshair.SetActive(true);
    }

    void ClickLeftBtn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isContact)
            {
                Interact();
            }
        }
    }

    void Interact()
    {
        isInteract = true;
        ps_QuestionEffect.gameObject.SetActive(true);
        Vector3 t_TargetPos = hitInfo.transform.position;
        ps_QuestionEffect.GetComponent<QuestionEffect>().SetTarget(t_TargetPos);
        ps_QuestionEffect.transform.position = Cam.transform.position;

        StartCoroutine(WaitCollision());
    }

    IEnumerator WaitCollision()
    {
        yield return new WaitUntil(() => QuestionEffect.isCollide);
        QuestionEffect.isCollide = false;

        dialogueManager.ShowDialogue();
    }
}
