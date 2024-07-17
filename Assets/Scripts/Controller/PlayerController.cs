using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform tf_Crosshair;

    [SerializeField] Transform tf_CameraView;

    [SerializeField] float sightMoveSpeed;
    [SerializeField] float sightSensitive;
    [SerializeField] float lookLimitX;
    [SerializeField] float lookLimitY;
    float currentAngleX;
    float currentAngleY;

    void Update()
    {
        CrosshairMoving();
        CameraViewMoving();
        CameraViewMovingByKey();
    }

    void CameraViewMovingByKey()
    {
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            currentAngleY += sightSensitive * Input.GetAxisRaw("Horizontal");
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            currentAngleX += -sightSensitive * Input.GetAxisRaw("Vertical");
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
        }

        tf_CameraView.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_CameraView.localEulerAngles.z);
    }

    void CameraViewMoving()
    {
        if (tf_Crosshair.localPosition.x > (Screen.width / 2 - 100) || tf_Crosshair.localPosition.x < (-Screen.width / 2 + 100))
        {
            currentAngleY += tf_Crosshair.localPosition.x > 0 ? sightSensitive : -sightSensitive;
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);

            float t_applySpeed = (tf_Crosshair.localPosition.x > 0 ? sightMoveSpeed : -sightMoveSpeed);
            tf_Crosshair.localPosition = new Vector2(tf_Crosshair.localPosition.x + t_applySpeed, tf_Crosshair.localPosition.y);
        }

        if (tf_Crosshair.localPosition.y > (Screen.height / 2 - 100) || tf_Crosshair.localPosition.y < (-Screen.height/ 2 + 100))
        {
            currentAngleX += tf_Crosshair.localPosition.y > 0 ? -sightSensitive : sightSensitive;
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
        }

        tf_CameraView.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_CameraView.localEulerAngles.z);
    }
    void CrosshairMoving()
    {
        // 마우스 위치를 화면 중앙을 기준으로 한 상대적 위치로 변환
        tf_Crosshair.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);

        float mouseX = tf_Crosshair.localPosition.x;
        float mouseY = tf_Crosshair.localPosition.y;

        // 십자선의 위치를 화면의 경계 내로 제한하고 조금씩 이격
        float clampedX = Mathf.Clamp(mouseX, -Screen.width / 2 + 50, Screen.width / 2 - 50);
        float clampedY = Mathf.Clamp(mouseY, -Screen.height / 2 + 50, Screen.height / 2 - 50);

        // 제한된 위치를 십자선의 위치로 설정
        tf_Crosshair.localPosition = new Vector2(clampedX, clampedY);
    }
}
