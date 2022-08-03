using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Canvas canvas;

    private void Awake() {
        instance = this;
    }

    public Slider healthSlider;

    public Image fadeScreen;

    public float fadeSpeed = 2f;
    private bool fadingToBlack, fadingFromBlack;

    public string mainMenuScene;

    public GameObject pauseScreen;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = true;
        // BUG: PlayerHealthController isn't setup at this point. If enabled, player starts with an empty health bar. If disabled, player starts with full health bar.
        // TODO: Store current/max health in playerprefs so we can properly update this and keep track between sessions.
    //    UpdateHealth(PlayerHealthController.instance.currentHealth, PlayerHealthController.instance.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingToBlack) {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed*Time.deltaTime));
            if (fadeScreen.color.a == 1f) {
                fadingToBlack = false;
            }
        } else if (fadingFromBlack) {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed*Time.deltaTime));
            if (fadeScreen.color.a == 0f) {
                fadingFromBlack = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            PauseUnpause();
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth) {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void StartFadeToBlack() {
        fadingToBlack = true;
        fadingFromBlack = false;
    }

    public void StartFadeFromBlack() {
        fadingFromBlack = true;
        fadingToBlack = false;

    }

    public void PauseUnpause() {
        if(!pauseScreen.activeSelf){
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        } else {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }
    }

    public void GoToMainMenu(){
        Destroy(PlayerHealthController.instance.gameObject);
        PlayerHealthController.instance = null;

        Destroy(RespawnController.instance.gameObject);
        RespawnController.instance = null;
        
        instance = null;
        Destroy(gameObject);

        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);
    }
}
