using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Quaternion t_Rotation = Quaternion.LookRotation(target.position);
            Vector3 t_Euler = new Vector3(0, t_Rotation.eulerAngles.y, 0); // Keep only the yaw
            transform.eulerAngles = t_Euler;
        }
    }
}
