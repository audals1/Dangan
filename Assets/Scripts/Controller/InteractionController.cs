using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera Cam;

    RaycastHit hitInfo;

    [SerializeField] GameObject go_NormalCrosshair;
    [SerializeField] GameObject go_InteractionCrosshair;
    [SerializeField] GameObject go_Crosshair;
    [SerializeField] GameObject go_Cursor;
    [SerializeField] GameObject go_TargetNameBar;
    [SerializeField] Text txt_TargetName;

    bool isContact = false;
    public static bool isInteract = false;

    [SerializeField] ParticleSystem ps_QuestionEffect;

    [SerializeField] Image img_Interaction;
    [SerializeField] Image img_InteractionEffect;

    DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteract) return;
        CheckObject();
        ClickLeftBtn();
    }

    public void HideUI()
    {
        go_Crosshair.SetActive(false);
        go_Cursor.SetActive(false);
        go_TargetNameBar.SetActive(false);
    }

    void CheckObject()
    {
        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        if (Physics.Raycast(Cam.ScreenPointToRay(t_MousePos), out hitInfo, 100f))
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
            go_TargetNameBar.SetActive(true);
            txt_TargetName.text = hitInfo.transform.GetComponent<InteractionType>().GetName();
            if (isContact) return;
            isContact = true;
            go_InteractionCrosshair.SetActive(true);
            go_NormalCrosshair.SetActive(false);
            StopCoroutine("Interaction");
            StopCoroutine("InteractionEffect");
            StartCoroutine(Interaction(true));
            StartCoroutine(InteractionEffect());
        }
        else NotContact();
    }

    void NotContact()
    {
        go_TargetNameBar.SetActive(false);
        if (!isContact) return;
        isContact = false;
        go_InteractionCrosshair.SetActive(false);
        go_NormalCrosshair.SetActive(true);
        StopCoroutine("Interaction");
        StopCoroutine("InteractionEffect");
        StartCoroutine(Interaction(false));
    }

    void ClickLeftBtn()
    {
        if (isInteract) return;
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

        StopCoroutine("Interaction");

        Color color = img_Interaction.color;
        color.a = 0;
        img_Interaction.color = color;

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

        dialogueManager.ShowDialogue(hitInfo.transform.GetComponent<InteractionEvent>().GetDialogue());
    }

    IEnumerator Interaction(bool p_Appear)
    {
        Color t_Color = img_Interaction.color;
        if (p_Appear)
        {
            t_Color.a = 0;
            while (t_Color.a < 1)
            {
                t_Color.a += 0.1f;
                img_Interaction.color = t_Color;
                yield return null;
            }
        }
        else
        {
            while (t_Color.a > 0)
            {
                t_Color.a -= 0.1f;
                img_Interaction.color = t_Color;
                yield return null;
            }
        }
    }

    IEnumerator InteractionEffect()
    {
        while (isContact && !isInteract)
        {
            Color color = img_InteractionEffect.color;
            color.a = 0.5f;
            img_InteractionEffect.transform.localScale = Vector3.one;
            Vector3 t_Scale = Vector3.one;

            while (color.a > 0)
            {
                color.a -= Time.deltaTime;
                img_InteractionEffect.color = color;

                t_Scale.Set(t_Scale.x + Time.deltaTime, t_Scale.y + Time.deltaTime, t_Scale.z + Time.deltaTime);
                img_InteractionEffect.transform.localScale = t_Scale;
                yield return null;
            }
            yield return null;
        }
    }
}
