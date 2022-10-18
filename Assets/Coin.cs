using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, transform.rotation.y + 100 * Time.deltaTime, 0);
    }
}
