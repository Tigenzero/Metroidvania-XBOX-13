using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string newGameScene;

    public GameObject continueButton;

    public PlayerAbilityTracker player;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("ContinueLevel")) {
            continueButton.SetActive(true);

        }
        AudioManager.instance.PlayMainMenuMusic();
    }

    public void NewGame(){
        // Deletes Progress
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(newGameScene);
    }

    public void Continue() {

        player.gameObject.SetActive(true);
        LoadPlayerPrefs();
        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }

    public void QuitGame(){
        Application.Quit();

        Debug.Log("Game Quit");
    }

    private void LoadPlayerPrefs() {
        // reposition player
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));
        player.canDoubleJump = IsAbilityUnlocked("DoubleJumpUnlocked");
        player.canDash = IsAbilityUnlocked("DashUnlocked");
    }

    private bool IsAbilityUnlocked(string abilityKey) {
        if (PlayerPrefs.HasKey(abilityKey)) {
            return Convert.ToBoolean(PlayerPrefs.GetInt(abilityKey));
        }
        return false;
    }

    
}
