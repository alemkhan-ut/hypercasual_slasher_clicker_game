using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameData game_Data;
    public SoundManager soundManager;

    [Header("Game Objects")]
    public GameObject settingsPanel;
    public GameObject shopPanel;
    public GameObject musicBlockImage;
    public GameObject soundBlockImage;

    TimeSpan nextTime; // DEBUG

    public DateTime TodayDay;
    public DateTime NextDay;
    [Range(0, 5)] public double dayCount;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    public void MusicSwitch() // Переключение статуса музыки
    {
        soundManager.PlayButtonSound(); // Запуск звука

        game_Data.IsMusicOn = !game_Data.IsMusicOn; // Инверсируем текущее значение

        if (game_Data.IsMusicOn) // Проверяем статус
        {
            musicBlockImage.SetActive(false);
            PlayerPrefs.SetString(game_Data.MusicState, "ON"); // меняем данные на устройстве

            soundManager.musicSound.mute = false;
        }
        else
        {
            musicBlockImage.SetActive(true);
            PlayerPrefs.SetString(game_Data.MusicState, "OFF"); // меняем данные на устройстве

            soundManager.musicSound.mute = true;
        }
    }
    public void SoundSwitch() // Переключение статуса звуков
    {
        soundManager.PlayButtonSound(); // Запуск звука

        game_Data.IsSoundOn = !game_Data.IsSoundOn;// Инверсируем текущее значение


        if (game_Data.IsSoundOn)// Проверяем статус
        {
            soundBlockImage.SetActive(false); // меняем спрайт
            PlayerPrefs.SetString(game_Data.SoundState, "ON"); // меняем данные на устройстве

            soundManager.gameSound.GetComponent<AudioSource>().mute = false; // меняем звук
            soundManager.enemySound.GetComponent<AudioSource>().mute = false; // меняем звук
            soundManager.weaponSound.GetComponent<AudioSource>().mute = false; // меняем звук
        }
        else
        {
            soundBlockImage.SetActive(true); // меняем спрайт
            PlayerPrefs.SetString(game_Data.SoundState, "OFF"); // меняем данные на устройстве

            soundManager.gameSound.GetComponent<AudioSource>().mute = true; // меняем звук
            soundManager.enemySound.GetComponent<AudioSource>().mute = true; // меняем звук
            soundManager.weaponSound.GetComponent<AudioSource>().mute = true; // меняем звук
        }
    }
    public void GetVK()
    {
        Application.OpenURL(game_Data.OurVK);
        //sound_Manager.PlayButtonSound(); // Запуск звука
    }

    public void GetFB()
    {
        Application.OpenURL(game_Data.OurFB);
        //sound_Manager.PlayButtonSound(); // Запуск звука
    }

    public void GetInstagram()
    {
        Application.OpenURL(game_Data.OurInstagram);
        //sound_Manager.PlayButtonSound(); // Запуск звука
    }

    public void GetYouTube()
    {
        Application.OpenURL(game_Data.OurYouTube);
        //sound_Manager.PlayButtonSound(); // Запуск звука
    }

    public void SelectWeapon(int id)
    {
        PlayerPrefs.SetString(game_Data.SelectedWeaponID, id.ToString());
        //sound_Manager.PlayButtonSound();
    }

    public void DailyBonus()
    {
    }

    public void OpenPanel(GameObject panelObject)
    {
        panelObject.SetActive(true);
        //sound_Manager.PlayButtonSound();
    }
    public void ClosePanel(GameObject panelObject)
    {
        panelObject.SetActive(false);
        //sound_Manager.PlayButtonSound();
    }

    public void PlayButtonSound()
    {
        //sound_Manager.PlayButtonSound();
    }

    public void GetDate(Text text)
    {
    }

    private void Update()
    {
        //TodayDay = DateTime.Now;
        //nextTime = DateTime.Now.AddDays(dayCount)- TodayDay;

        //if (nextTime.Hours == 0 && nextTime.Minutes == 0 && nextTime.Seconds == 0)
        //{
        //    _text.text = "ТЫ ЧОРТ";
        //}
        //else
        //_text.text = nextTime.Hours.ToString() + ":" + nextTime.Minutes.ToString() + ":" + nextTime.Seconds.ToString();
    }
}
