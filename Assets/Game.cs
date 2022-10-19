using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static bool isDark;

    public Light gameLight;

    private static Game instance;

    public GameObject lightBar;

    private float lightCooldown = 1.0f;

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

    void Update() {
        if (lightCooldown < 1) {
            lightCooldown += Time.deltaTime / 10;
        } else {
            lightCooldown = 1;
        }
        lightBar.transform.localScale = new Vector3(lightCooldown, 1, 1);
    }

    public static void ToggleDarkness() {
        if (Instance.lightCooldown < 1) return;

        SetDarkness(false);

        Instance.StartCoroutine(Instance.WaitAndResetDarkness());
    }

    public IEnumerator WaitAndResetDarkness() {
        yield return new WaitForSeconds(2);
        SetDarkness(true);
        Instance.lightCooldown = 0;
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
