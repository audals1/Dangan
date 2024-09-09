using UnityEngine;

public class CrosshairSpin : MonoBehaviour
{

    [SerializeField] float spinSpeed;
    [SerializeField] Vector3 spinDirection;


    void Update()
    {
        transform.Rotate(spinDirection * spinSpeed * Time.deltaTime);
    }
}
