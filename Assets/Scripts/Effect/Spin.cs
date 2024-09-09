using System.Collections;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] Transform target;

    bool isSpin = false;
    public static bool isFinished = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (!isSpin)
            {
                /*Quaternion t_Rotation = Quaternion.LookRotation(target.position);
                Vector3 t_Euler = new Vector3(0, t_Rotation.eulerAngles.y, 0);
                transform.eulerAngles = t_Euler;*/
                // 플레이어를 향한 방향 벡터 계산
                Vector3 direction = target.position - transform.position;
                direction.y = 0; // Y축 회전만 고려

                // 방향 벡터를 기준으로 Y축 회전 각도 계산
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                // 부모 오브젝트의 회전 값 설정
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }

            else
            {
                transform.Rotate(0, 90 * Time.deltaTime * 8, 0);
            }

        }
    }

    public IEnumerator SetAppearOrDisappear(bool p_flag)
    {
        isSpin = true;

        SpriteRenderer[] t_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        Color t_FrontColor = t_SpriteRenderers[0].color;
        Color t_RearColor = t_SpriteRenderers[1].color;

        if (p_flag)
        {
            t_FrontColor.a = 0; t_RearColor.a = 0;
            t_SpriteRenderers[0].color = t_FrontColor; t_SpriteRenderers[1].color = t_RearColor;
        }

        float t_FadeSpeed = (p_flag == true) ? 0.01f : -0.01f;

        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            if (p_flag && t_FrontColor.a >= 1)
            {
                break;
            }
            else if (!p_flag && t_FrontColor.a <= 0)
            {
                break;
            }
            t_FrontColor.a += t_FadeSpeed;
            t_RearColor.a += t_FadeSpeed;

            t_SpriteRenderers[0].color = t_FrontColor;
            t_SpriteRenderers[1].color = t_RearColor;

            yield return null;
        }

        isSpin = false;
        isFinished = true;
        gameObject.SetActive(p_flag);
    }
}
