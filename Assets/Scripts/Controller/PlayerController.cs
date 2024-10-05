using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    float applySpeed;

    [SerializeField] float fieldSensitivity;
    [SerializeField] float fieldLookLimitX;

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

    public void ResetAngle()
    {
        tf_Crosshair.localPosition = Vector3.zero;
        currentAngleX = 0;
        currentAngleY = 0;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }   

    void Start()
    {
        originPosY = tf_CameraView.localPosition.y;
    }

    void Update()
    {
        if (InteractionController.isInteract) return;
        if (CameraController.onlyView)
        {
            CrosshairMoving();
            CameraViewMoving();
            CameraViewMovingByKey();
            CameraLimit();
            NotCamUI();
        }

        else
        {
            FieldMoving();
            FieldLooking();
        }
    }

    void FieldMoving()
    {
        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            Vector3 t_MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                applySpeed = runSpeed;
            }
            else
            {
                applySpeed = walkSpeed;
            }

            transform.Translate(t_MoveDir * applySpeed * Time.deltaTime, Space.Self);
        }
    }

    void FieldLooking() 
    {
        if (Input.GetAxisRaw("Mouse X") != 0)
        {
            float t_AngleY = Input.GetAxisRaw("Mouse X") * fieldSensitivity;
            Vector3 t_Rot = new Vector3(0, t_AngleY * fieldSensitivity, 0);
            transform.rotation = Quaternion.Euler(transform.eulerAngles + t_Rot);
        }

        if (Input.GetAxisRaw("Mouse Y") != 0)
        {
            float t_AngleX = Input.GetAxisRaw("Mouse Y") * fieldSensitivity;
            currentAngleX -= t_AngleX;
            currentAngleX = Mathf.Clamp(currentAngleX, -fieldLookLimitX, fieldLookLimitX);
            tf_CameraView.localEulerAngles = new Vector3(currentAngleX, 0, 0);
        }
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
        if (tf_CameraView.localPosition.x >= camBoundary.x)
        {
            tf_CameraView.localPosition = new Vector3(camBoundary.x, tf_CameraView.localPosition.y, tf_CameraView.localPosition.z);
        }
        else if (tf_CameraView.localPosition.x <= -camBoundary.x)
        {
            tf_CameraView.localPosition = new Vector3(-camBoundary.x, tf_CameraView.localPosition.y, tf_CameraView.localPosition.z);
        }
        if (tf_CameraView.localPosition.y >= originPosY + camBoundary.y)
        {
            tf_CameraView.localPosition = new Vector3(tf_CameraView.localPosition.x, originPosY + camBoundary.y, tf_CameraView.localPosition.z);
        }
        else if (tf_CameraView.localPosition.y <= originPosY - camBoundary.y)
        {
            tf_CameraView.localPosition = new Vector3(tf_CameraView.localPosition.x, originPosY - camBoundary.y, tf_CameraView.localPosition.z);
        }
    }

    void CameraViewMovingByKey()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
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
            tf_CameraView.localPosition = new Vector3(tf_CameraView.localPosition.x + t_applySpeed, tf_CameraView.localPosition.y, tf_CameraView.localPosition.z);
        }

        if (tf_Crosshair.localPosition.y > (Screen.height / 2 - 100) || tf_Crosshair.localPosition.y < (-Screen.height / 2 + 100))
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
