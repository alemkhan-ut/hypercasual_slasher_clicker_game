using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public GameObject buttonObjectForOff; // ссылка на кнопку для отключения ее во время загрузки
    public SoundManager soundManager;
    public bool IsSceneLoading;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            IsSceneLoading = true;
            buttonObjectForOff.SetActive(false);
            soundManager = FindObjectOfType<SoundManager>(); // Находим объект со Звуками
        }
    }

    public void SceneLoaded() // Завершение загрузки сцены
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            IsSceneLoading = false;
            buttonObjectForOff.SetActive(true);
        }

    }

    public void Slide()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (soundManager != null)
            {
                soundManager.PlaySlide(); // Запуск звука слайдера
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
