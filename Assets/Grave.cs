using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    public GameObject coin;

    public float secondsNeeded = 2;
    public float secondsSpent = 0;

    public AudioClip diggingSound;

    public void OnFirstInteract()
    {
        GetComponent<AudioSource>().PlayOneShot(diggingSound);
    }

    public void Interact()
    {
        secondsSpent += Time.deltaTime;

        // progressBar.SetProgress(secondsSpent / secondsNeeded);

        if (secondsSpent > secondsNeeded)
        {
            OnDone();
        }
    }

    public void StopInteract()
    {
        secondsSpent = 0;
        GetComponent<AudioSource>().Stop();
    }

    public void OnDone()
    {
        if (coin == null) return;
        coin.SetActive(true);
    }
}
