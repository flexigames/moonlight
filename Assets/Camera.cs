using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target == null) return;

        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z - 15);        
    }
}
