using System.Collections;
using UnityEngine;

public class Disappear : MonoBehaviour
{

    [SerializeField] float disappearTime;

    void OnEnable()
    {
        StartCoroutine(DisappearCoroutine());
    }

    IEnumerator DisappearCoroutine()
    {
        yield return new WaitForSeconds(disappearTime);
        gameObject.SetActive(false);
    }
}
