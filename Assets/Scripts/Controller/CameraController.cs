using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public void CameraTargetting(Transform target, float camSpeed = 0.05f)
    {
        if (target == null) return;
        StopAllCoroutines();
        StartCoroutine(CameraTargettingCoroutine(target, camSpeed));
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
}
