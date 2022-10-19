using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static bool isDark = true;

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

    void Start() {
        SetDarkness(true);
    }

    public static void ToggleDarkness() {
        SetDarkness(!isDark);
    }

    public static void SetDarkness(bool isDark) {
        Game.isDark = isDark;
        if (isDark) {
            Instance.gameLight.intensity = 0.0f;
        } else {
            Instance.gameLight.intensity = 0.4f;
        }
    }

    public static void GameOver() {
        SetDarkness(true);
        
        SceneManager.LoadScene("GameOver");
    }
}
