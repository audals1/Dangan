using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{

    [SerializeField] Image Image;

    [SerializeField] Color colorWhite;
    [SerializeField] Color colorBlack;

    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeSlowSpeed;

    public bool isFinished = false;

    public IEnumerator FadeOut(bool isWhite, bool isSlow)
    {
        Color t_color = isWhite ? colorWhite : colorBlack;
        t_color.a = 0;

        Image.color = t_color;

        while (t_color.a < 1)
        {
            t_color.a += isSlow ? fadeSlowSpeed : fadeSpeed;
            Image.color = t_color;
            yield return null;
        }

        isFinished = true;
    }

    public IEnumerator FadeIn(bool isWhite, bool isSlow)
    {
        Color t_color = isWhite ? colorWhite : colorBlack;
        t_color.a = 1;

        Image.color = t_color;

        while (t_color.a > 0)
        {
            t_color.a -= isSlow ? fadeSlowSpeed : fadeSpeed;
            Image.color = t_color;
            yield return null;
        }

        isFinished = true;
    }
    
}
