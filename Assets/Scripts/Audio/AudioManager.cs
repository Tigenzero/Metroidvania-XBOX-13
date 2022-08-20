using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    // TODO: Create Key/Value pair list of SFX. We want to reference the sound effect name, not its number, in other scripts.

    public static AudioManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        
    }

    public AudioSource mainMenuMusic, levelMusic, bossMusic;

    public AudioSource[] sfx;

    public void PlayMainMenuMusic(){
        PlayMusic(mainMenuMusic);
    }

    public void PlayLevelMusic(){
        PlayMusic(levelMusic);
    }

    public void PlayBossMusic(){
        PlayMusic(bossMusic);
    }

    private void PlayMusic(AudioSource music) {
        if (!music.isPlaying){
            StopMusic();
            music.Play();
        }
    }

    private void StopMusic() {
        levelMusic.Stop();
        mainMenuMusic.Stop();
        bossMusic.Stop();
    }

    public void PlaySFX(int sfxToPlay){
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();

    }

    // TODO replace int with string
    public void PlaySFXAdjusted(int sfxToAdjust){
        sfx[sfxToAdjust].pitch = Random.Range(.8f, 1.2f);
        PlaySFX(sfxToAdjust);
    }
}
