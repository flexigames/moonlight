using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static bool isDark = false;

    public Light gameLight;

    private static Game instance;

    public static Game Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<Game>();
            }
            return instance;
        }
    }

    public static void ToggleDarkness() {
        isDark = !isDark;
        if (isDark) {
            Instance.gameLight.intensity = 0.0f;
        } else {
            Instance.gameLight.intensity = 0.4f;
        }
    }
}
