using System.Collections;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;


    private bool CheckSameSprite(SpriteRenderer spriteRenderer, Sprite sprite)
    {
        if (spriteRenderer.sprite == sprite)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator SpriteChangeCoroutine(Transform target, string spriteName)
    {
        SpriteRenderer[] t_spriteRenderer = target.GetComponentsInChildren<SpriteRenderer>();
        Sprite t_Sprite = Resources.Load("Characters/" + spriteName, typeof(Sprite)) as Sprite;

        if (!CheckSameSprite(t_spriteRenderer[0], t_Sprite))
        {
            Color t_color = t_spriteRenderer[0].color;
            Color t_shadowColor = t_spriteRenderer[1].color;
            t_color.a = 0;
            t_shadowColor.a = 0;
            t_spriteRenderer[0].color = t_color;
            t_spriteRenderer[1].color = t_shadowColor;

            t_spriteRenderer[0].sprite = t_Sprite;
            t_spriteRenderer[1].sprite = t_Sprite;

            while (t_color.a < 1)
            {
                t_color.a += fadeSpeed;
                t_shadowColor.a += fadeSpeed;
                t_spriteRenderer[0].color = t_color;
                t_spriteRenderer[1].color = t_shadowColor;
                yield return null;
            }
        }


    }
}
