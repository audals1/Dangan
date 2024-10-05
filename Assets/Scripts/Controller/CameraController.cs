using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool onlyView = true; //이동가능할 때는 rot이 이전 rot으로, 이동 불가할 때는 zero로 설정
    Vector3 originPos;
    Quaternion originRot;

    InteractionController interactionController;
    PlayerController playerController;

    Coroutine cameraCoroutine;

    private void Start()
    {
        interactionController = FindObjectOfType<InteractionController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public void CamOriginSetting()
    {
        originPos = transform.position;
        if (onlyView)
            originRot = Quaternion.Euler(0, 0, 0);
        else
            originRot = transform.rotation;
    }

    public void CameraTargetting(Transform target, float camSpeed = 0.05f, bool isReset = false, bool isFinish = false)
    {
        if (!isReset)
        {
            if (target == null) return;
            StopAllCoroutines();
            cameraCoroutine = StartCoroutine(CameraTargettingCoroutine(target, camSpeed));
        }
        else
        {
            if (cameraCoroutine != null)
            {
                StopCoroutine(cameraCoroutine);
            }
            StartCoroutine(CameraResetCoroutine(camSpeed, isFinish));
        }
    }

    IEnumerator CameraTargettingCoroutine(Transform target, float camSpeed = 0.05f)
    {
        Vector3 targetPosition = target.position;
        Vector3 targetFrontPos = targetPosition + target.forward;
        Vector3 dir = (targetPosition - targetFrontPos).normalized;

        while (transform.position != targetFrontPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetFrontPos, camSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), camSpeed);
            yield return null;
        }
    }

    IEnumerator CameraResetCoroutine(float camSpeed = 0.1f, bool isFinish = false)
    {
        yield return new WaitForSeconds(0.5f);

        while (transform.position != originPos || Quaternion.Angle(transform.rotation, originRot) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, camSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originRot, camSpeed);
            yield return null;
        }

        transform.position = originPos;

        if (isFinish)
        {
            InteractionController.isInteract = false;
            playerController.ResetAngle();
        }
    }
}
