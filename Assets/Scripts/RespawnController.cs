using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;

    private void Awake() {
        instance = this;
    }


    private Vector3 respawnPoint;
    public float waitToRespawn;

    private GameObject thePlayer;

    public GameObject deathEffect;

    public float deathTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = PlayerHealthController.instance.gameObject;

        respawnPoint = thePlayer.transform.position;
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpawn(Vector3 newPosition) {
        respawnPoint = newPosition;
    }

    public void Respawn() {
        StartCoroutine(RespawnCo());
    }

    IEnumerator RespawnCo() {
        Time.timeScale = deathTimeScale;
        yield return new WaitForSeconds(waitToRespawn);
        Time.timeScale = 1f;
        thePlayer.SetActive(false);
        // if (deathEffect != null) {
        //     Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation);
        // }

        // yield return new WaitForSeconds(waitToRespawn/2);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        thePlayer.transform.position = respawnPoint;
        thePlayer.SetActive(true);
        thePlayer.GetComponent<HeroinePlayerController>().revivePlayer();
        PlayerHealthController.instance.FillHealth();
        thePlayer.GetComponent<HeroinePlayerController>().anim.SetTrigger("isUp");
        
    }
}
