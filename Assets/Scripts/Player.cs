using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    private CharacterController controller;

    public ParticleSystem runningCloud;

    public Light light;

    private Vector3 lastPosition = Vector3.zero;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleRunning();
        HandleRunningCloud();

        if (Input.GetKey(KeyCode.E))
        {
            light.intensity += 0.01f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            light.intensity -= 0.01f;
        }
    }

    void HandleRunningCloud() {
        if (runningCloud == null) return;

        if (lastPosition != transform.position) {
            if (!runningCloud.isEmitting) {
                runningCloud.Play();
            }
            lastPosition = transform.position;
        } else {
            if (runningCloud.isEmitting) {
                runningCloud.Stop();
            }
        }
    }

    void HandleRunning() {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));   

        if (direction.magnitude < 0.01f) return;

        direction.Normalize();

        controller.SimpleMove(direction * speed);

        transform.rotation = Quaternion.LookRotation(direction);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin") {
            Destroy(other.gameObject);
        }
    }
}
