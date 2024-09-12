using System.Collections;
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
    TransferManager transferManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        transferManager = FindObjectOfType<TransferManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteract) return;
        CheckObject();
        ClickLeftBtn();
    }

    public void SettingMouseUI(bool p_flag)
    {
        go_Crosshair.SetActive(p_flag);
        go_Cursor.SetActive(p_flag);

        if (!p_flag)
        {
            StopCoroutine("Interaction");
            Color color = img_Interaction.color;
            color.a = 0;
            img_Interaction.color = color;
            go_TargetNameBar.SetActive(p_flag);
        }

        else
        {
            go_NormalCrosshair.SetActive(true);
            go_InteractionCrosshair.SetActive(false);
        }

        isInteract = !p_flag;
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

    void DialogueCall(InteractionEvent p_event)
    {
        dialogueManager.SetNextEvent(p_event.GetNextEvent());
        if (p_event.GetAppearType() == AppearType.Appear) dialogueManager.SetAppearObjects(p_event.GetTargets());
        else if (p_event.GetAppearType() == AppearType.Disappear) dialogueManager.SetDisappearObjects(p_event.GetTargets());
        dialogueManager.ShowDialogue(p_event.GetDialogue());
    }

    void TrancferCall()
    {
        string t_SceneName = hitInfo.transform.GetComponent<InteractionDoor>().GetSceneName();
        string t_LocationName = hitInfo.transform.GetComponent<InteractionDoor>().GetLocationName();
        var coroutine = transferManager.Transfer(t_SceneName, t_LocationName);
        StartCoroutine(coroutine);
    }

    IEnumerator WaitCollision()
    {
        yield return new WaitUntil(() => QuestionEffect.isCollide);
        QuestionEffect.isCollide = false;

        yield return new WaitForSeconds(0.5f);

        InteractionEvent t_InteractionEvent = hitInfo.transform.GetComponent<InteractionEvent>();

        if (hitInfo.transform.GetComponent<InteractionType>().isObject)
        {
            DialogueCall(t_InteractionEvent);
        }

        else
        {
            TrancferCall();
        }
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
