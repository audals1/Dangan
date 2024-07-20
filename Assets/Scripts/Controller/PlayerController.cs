using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform tf_Crosshair;

    [SerializeField] Transform tf_CameraView;
    [SerializeField] Vector2 camBoundary;

    [SerializeField] float sightMoveSpeed;
    [SerializeField] float sightSensitive;
    [SerializeField] float lookLimitX;
    [SerializeField] float lookLimitY;
    float currentAngleX;
    float currentAngleY;

    float originPosY;

    [SerializeField] GameObject go_NotcamDown;
    [SerializeField] GameObject go_NotcamUp;
    [SerializeField] GameObject go_NotcamRight;
    [SerializeField] GameObject go_NotcamLeft;

    void Start()
    {
        originPosY = tf_CameraView.localPosition.y;    
    }

    void Update()
    {
        CrosshairMoving();
        CameraViewMoving();
        CameraViewMovingByKey();
        CameraLimit();
        NotCamUI();
    }

    void NotCamUI()
    {
        go_NotcamDown.SetActive(false);
        go_NotcamUp.SetActive(false);
        go_NotcamRight.SetActive(false);
        go_NotcamLeft.SetActive(false);

        if (currentAngleY >= lookLimitX)
            go_NotcamRight.SetActive(true);
        else if (currentAngleY <= -lookLimitX)
            go_NotcamLeft.SetActive(true);
        if (currentAngleX <= -lookLimitY)
            go_NotcamUp.SetActive(true);
        else if (currentAngleX >= lookLimitY)
            go_NotcamDown.SetActive(true);
    }

    void CameraLimit()
    {
        if(tf_CameraView.localPosition.x >= camBoundary.x)
        {
            tf_CameraView.localPosition = new Vector3(camBoundary.x, tf_CameraView.localPosition.y, tf_CameraView.localPosition.z);
        }
        else if(tf_CameraView.localPosition.x <= -camBoundary.x)
        {
            tf_CameraView.localPosition = new Vector3(-camBoundary.x, tf_CameraView.localPosition.y, tf_CameraView.localPosition.z);
        }
        if(tf_CameraView.localPosition.y >= originPosY + camBoundary.y)
        {
            tf_CameraView.localPosition = new Vector3(tf_CameraView.localPosition.x, originPosY + camBoundary.y, tf_CameraView.localPosition.z);
        }
        else if(tf_CameraView.localPosition.y <= originPosY -camBoundary.y)
        {
            tf_CameraView.localPosition = new Vector3(tf_CameraView.localPosition.x, originPosY - camBoundary.y, tf_CameraView.localPosition.z);
        }
    }

    void CameraViewMovingByKey()
    {
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            currentAngleY += sightSensitive * Input.GetAxisRaw("Horizontal");
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);
            tf_CameraView.localPosition = new Vector3(tf_CameraView.localPosition.x + sightMoveSpeed * Input.GetAxisRaw("Horizontal"), tf_CameraView.localPosition.y, tf_CameraView.localPosition.z);
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            currentAngleX += sightSensitive * -Input.GetAxisRaw("Vertical");
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
            tf_CameraView.localPosition = new Vector3(tf_CameraView.localPosition.x, tf_CameraView.localPosition.y + sightMoveSpeed * Input.GetAxisRaw("Vertical"), tf_CameraView.localPosition.z);
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
            tf_CameraView.localPosition = new Vector3(tf_CameraView.localPosition.x + t_applySpeed, tf_CameraView.localPosition.y,tf_CameraView.localPosition.z);
        }

        if (tf_Crosshair.localPosition.y > (Screen.height / 2 - 100) || tf_Crosshair.localPosition.y < (-Screen.height/ 2 + 100))
        {
            currentAngleX += tf_Crosshair.localPosition.y > 0 ? -sightSensitive : sightSensitive;
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);

            float t_applySpeed = (tf_Crosshair.localPosition.y > 0 ? sightMoveSpeed : -sightMoveSpeed);
            tf_CameraView.localPosition = new Vector3(tf_CameraView.localPosition.x, tf_CameraView.localPosition.y + t_applySpeed, tf_CameraView.localPosition.z);
        }

        tf_CameraView.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_CameraView.localEulerAngles.z);
    }
    void CrosshairMoving()
    {
        // ���콺 ��ġ�� ȭ�� �߾��� �������� �� ����� ��ġ�� ��ȯ
        tf_Crosshair.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);

        float mouseX = tf_Crosshair.localPosition.x;
        float mouseY = tf_Crosshair.localPosition.y;

        // ���ڼ��� ��ġ�� ȭ���� ��� ���� �����ϰ� ���ݾ� �̰�
        float clampedX = Mathf.Clamp(mouseX, -Screen.width / 2 + 50, Screen.width / 2 - 50);
        float clampedY = Mathf.Clamp(mouseY, -Screen.height / 2 + 50, Screen.height / 2 - 50);

        // ���ѵ� ��ġ�� ���ڼ��� ��ġ�� ����
        tf_Crosshair.localPosition = new Vector2(clampedX, clampedY);
    }
}
