using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    public GameObject coin;

    public GameObject dirt;

    public float secondsNeeded = 2;
    public float secondsSpent = 0;

    public AudioClip diggingSound;

    public bool isDone = false;

    public void OnFirstInteract()
    {
        if (isDone) return;

        Game.SetDiggingProgress(secondsSpent / secondsNeeded);
        Game.ShowDiggingIndicator();
        GetComponent<AudioSource>().PlayOneShot(diggingSound);
    }

    public void Interact()
    {
        if (isDone) return;

        secondsSpent += Time.deltaTime;

        Game.SetDiggingProgress(secondsSpent / secondsNeeded);

        if (secondsSpent > secondsNeeded)
        {
            OnDone();
            StopInteract();
        }
    }

    public void StopInteract()
    {
        Game.HideDiggingIndicator();
        GetComponent<AudioSource>().Stop();
    }

    public void OnDone()
    {
        if (coin == null) return;
        isDone = true;
        coin.SetActive(true);
        dirt.SetActive(false);
    }
}
