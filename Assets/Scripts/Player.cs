using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    private CharacterController controller;

    public ParticleSystem runningCloud;

    private Vector3 lastPosition = Vector3.zero;

    private float interactDistance = 3.0f;

    private Grave currentGrave = null;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleRunning();
        HandleRunningCloud();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Game.ToggleDarkness();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (currentGrave != null)
            {
                currentGrave.StopInteract();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryInteractWithGrave(true);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            TryInteractWithGrave(false);
        }
    }

    void TryInteractWithGrave(bool isFirst)
    {
        var grave = FindOneOfComponentType<Grave>();
        if (grave != null)
        {
            if (isFirst)
            {
                currentGrave = grave;
                grave.OnFirstInteract();
            }
            else
            {
                grave.Interact();
            }
        }
    }

    void HandleRunningCloud()
    {
        if (runningCloud == null) return;

        if (lastPosition != transform.position)
        {
            if (!runningCloud.isEmitting)
            {
                runningCloud.Play();
            }
            lastPosition = transform.position;
        }
        else
        {
            if (runningCloud.isEmitting)
            {
                runningCloud.Stop();
            }
        }
    }

    void HandleRunning()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (direction.magnitude < 0.01f) return;

        direction.Normalize();

        controller.SimpleMove(direction * speed);

        transform.rotation = Quaternion.LookRotation(direction);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            Game.PlayCoinSound();
            Game.AddCoinToCollected();
        }
    }

    List<T> FindOfComponentType<T>()
    {
        Collider[] colldiers = Physics.OverlapSphere(transform.position, interactDistance);
        var listOfComponentTypes = new List<T>();
        foreach (Collider collider in colldiers)
        {
            T componentType = collider.GetComponent<T>();
            if (componentType != null) listOfComponentTypes.Add(componentType);
        }
        return listOfComponentTypes;
    }

    T FindOneOfComponentType<T>()
    {
        List<T> componentTypes = FindOfComponentType<T>();

        if (componentTypes.Count > 0) return componentTypes[0];

        return default(T);
    }
}
