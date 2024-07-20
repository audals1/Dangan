using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    [SerializeField] float spinSpeed;
    [SerializeField] Vector3 spinDirection;

    
    void Update()
    {
        transform.Rotate(spinDirection * spinSpeed * Time.deltaTime);
    }
}
