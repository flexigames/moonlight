using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
    public bool isDark;

    public bool playerCanDie = true;

    public float gameLightIntensity = 0.4f;

    public Light gameLight;

    private static Game instance;

    public GameObject lightBar;

    private float lightCooldown = 1.0f;

    public int totalCoins = 0;

    public int collectedCoins = 0;

    public TextMeshProUGUI coinText;

    public AudioClip howlSound;

    public AudioClip coinSound;

    public static bool PlayerCanDie
    {
        get
        {
            return instance.playerCanDie;
        }
    }

    public static bool IsDark
    {
        get
        {
            return Instance.isDark;
        }

        set
        {
            Instance.isDark = value;
        }
    }

    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Game>();
            }
            return instance;
        }
    }

    void Start()
    {
        SetDarkness(true);
    }

    void Update()
    {
        UpdateLightCooldown();
        UpdateCoinText();
        UpdateDarkness();
        if (collectedCoins == totalCoins)
        {
            SceneManager.LoadScene("Win");
        }
    }

    void UpdateLightCooldown()
    {
        if (lightCooldown < 1)
        {
            lightCooldown += Time.deltaTime / 10;
        }
        else
        {
            lightCooldown = 1;
        }
        lightBar.transform.localScale = new Vector3(lightCooldown, 1, 1);
    }

    void UpdateCoinText()
    {
        coinText.text = collectedCoins + "/" + totalCoins;
    }

    public static void ToggleDarkness()
    {
        if (Instance.lightCooldown < 1) return;

        SetDarkness(false);

        Instance.StartCoroutine(Instance.WaitAndResetDarkness());
    }

    public IEnumerator WaitAndResetDarkness()
    {
        yield return new WaitForSeconds(2);
        SetDarkness(true);
        Instance.lightCooldown = 0;
    }

    public static void SetDarkness(bool isDark)
    {
        Game.Instance.isDark = isDark;
    }

    public void UpdateDarkness()
    {
        if (isDark)
        {
            gameLight.intensity = 0.0f;
        }
        else
        {
            gameLight.intensity = gameLightIntensity;
        }
    }

    public static void GameOver()
    {
        if (!Instance.playerCanDie) return;

        SetDarkness(true);

        SceneManager.LoadScene("GameOver");
    }

    public static void Win()
    {
        SceneManager.LoadScene("Win");
    }

    public static void SetTotalCoins(int totalCoins)
    {
        Instance.totalCoins = totalCoins;
    }

    public static void AddCoinToTotal()
    {
        Instance.totalCoins += 1;
    }

    public static void AddCoinToCollected()
    {
        Instance.collectedCoins += 1;
    }

    public static void OnMazeDone()
    {

    }

    public static void PlayHowl()
    {
        var audioSource = Instance.GetComponent<AudioSource>();
        audioSource.PlayOneShot(Instance.howlSound);
    }

    public static void PlayCoinSound()
    {
        var audioSource = Instance.GetComponent<AudioSource>();
        audioSource.PlayOneShot(Instance.coinSound);
    }
}
