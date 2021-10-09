using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public GameData game_Data;

    public AudioSource musicSound;
    public AudioSource weaponSound;
    public AudioSource gameSound;
    public AudioSource enemySound;

    public AudioClip mainMenuMusicClip;
    public AudioClip gamePlayMusicClip;
    public AudioClip bossMusicClip;
    public AudioClip simpleButtonClip;
    public AudioClip startGameButtonClip;
    public AudioClip loseSound;
    public AudioClip pickupClip;
    public AudioClip gameLoseClip;
    public AudioClip bossPresentationClip;
    public AudioClip slideSound;
    public AudioClip purchaseAccept;
    public AudioClip purchaseDenied;
    public AudioClip[] hitClips;
    public AudioClip[] misstakeHitClips;
    public AudioClip[] throwWeaponClips;
    public AudioClip[] ouchVoiceClips;
    public AudioClip[] fruitSliceClips;
    public AudioClip[] youLostClips_;
    public AudioClip[] gameOverClips_;
    public AudioClip evilLaught_;


    private void Start()
    {
        //UpdateAudio();

        PlayerPrefs.SetString(game_Data.MusicState, "ON");
        PlayerPrefs.SetString(game_Data.SoundState, "ON");

        gameSound.GetComponent<AudioSource>().mute = false;
        enemySound.GetComponent<AudioSource>().mute = false;
        weaponSound.GetComponent<AudioSource>().mute = false;
        musicSound.GetComponent<AudioSource>().mute = false;
    }

    public void UpdateAudio()
    {
        if (PlayerPrefs.GetString(game_Data.MusicState) == "ON")
        {
            gameSound.GetComponent<AudioSource>().mute = false;
            enemySound.GetComponent<AudioSource>().mute = false;
            weaponSound.GetComponent<AudioSource>().mute = false;
            musicSound.GetComponent<AudioSource>().mute = false;
        }
        else
        {
            gameSound.GetComponent<AudioSource>().mute = true;
            enemySound.GetComponent<AudioSource>().mute = true;
            weaponSound.GetComponent<AudioSource>().mute = true;
            musicSound.GetComponent<AudioSource>().mute = true;
        }

        if (PlayerPrefs.GetString(game_Data.SoundState) == "ON")
        {
            gameSound.GetComponent<AudioSource>().mute = false;
            enemySound.GetComponent<AudioSource>().mute = false;
            weaponSound.GetComponent<AudioSource>().mute = false;
        }
        else
        {
            gameSound.GetComponent<AudioSource>().mute = true;
            enemySound.GetComponent<AudioSource>().mute = true;
            weaponSound.GetComponent<AudioSource>().mute = true;

            SoundRefresh();
        }
    }

    public void MusicPause()
    {
        musicSound.Pause();
    }
    public void PlayHitSound()
    {
        gameSound.clip = fruitSliceClips[Random.Range(0, fruitSliceClips.Length)];
        gameSound.Play();
    }
    public void PlayBossPresentationSound()
    {
        enemySound.clip = bossPresentationClip;
        musicSound.clip = bossMusicClip;
        enemySound.Play();
        musicSound.Play();
    }
    public void PlayLoseSound()
    {
        gameSound.clip = youLostClips_[Random.Range(0, youLostClips_.Length)]; ;
        gameSound.Play();
    }
    public void PlayPurchaseDeniedSound()
    {
        gameSound.clip = purchaseDenied;
        gameSound.Play();
    }

    public void PlayMainMenuMusic()
    {
        musicSound.clip = mainMenuMusicClip;
        musicSound.Play();
    }

    public void PlayGamePlayMusic()
    {
        musicSound.clip = gamePlayMusicClip;
        musicSound.Play();
    }

    public void PlayDemoEvilLaught()
    {
        musicSound.clip = evilLaught_;
        musicSound.Play();
    }

    public void PlayMisstakeHitSound()
    {
        weaponSound.clip = gameOverClips_[Random.Range(0, gameOverClips_.Length)];
        weaponSound.Play();
    }
    public void PlayThrowWeaponSound()
    {
        weaponSound.clip = throwWeaponClips[Random.Range(0, throwWeaponClips.Length)];
        weaponSound.Play();
    }

    public void SoundRefresh()
    {
        weaponSound.clip = throwWeaponClips[Random.Range(0, throwWeaponClips.Length)];
        gameSound.clip = simpleButtonClip;
        enemySound.clip = hitClips[Random.Range(0, hitClips.Length)];
    }

    public void PlaySlide()
    {
        gameSound.clip = slideSound;
        gameSound.Play();
    }

    public void PurchaseAccept()
    {
        gameSound.clip = purchaseAccept;
        gameSound.Play();
    }
    public void PurchaseDenied()
    {
        gameSound.clip = purchaseDenied;
        gameSound.Play();
    }

    public void PlayButtonSound()
    {
        gameSound.clip = simpleButtonClip;
        gameSound.Play(); // Запуск звука
    }
}
