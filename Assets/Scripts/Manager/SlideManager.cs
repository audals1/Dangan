using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlideManager : MonoBehaviour
{
    [SerializeField] Image img_SlideCG;
    [SerializeField] Animation anim;

    public static bool isFinished = false;
    public static bool isChanged = false;

    public IEnumerator AppearSlide(string slideName)
    {
        Sprite t_Sprite = Resources.Load<Sprite>("Slide_Image/" + slideName);
        if (t_Sprite != null)
        {
            img_SlideCG.gameObject.SetActive(true);
            img_SlideCG.sprite = t_Sprite;
            anim.Play("Appear");
        }
        else
        {
            Debug.LogError(slideName + "이미지가 없습니다.");
        }

        yield return new WaitForSeconds(0.5f);

        isFinished = true;
    }

    public IEnumerator DisappearSlide()
    {
        anim.Play("Disappear");

        yield return new WaitForSeconds(0.5f);

        img_SlideCG.gameObject.SetActive(false);

        isFinished = true;
    }

    public IEnumerator ChangeSlide(string slideName)
    {
        isFinished = false;
        StartCoroutine(DisappearSlide());
        yield return new WaitUntil(() => isFinished);

        isFinished = false;
        StartCoroutine(AppearSlide(slideName));
        yield return new WaitUntil(() => isFinished);

        isChanged = true;
    }
}
