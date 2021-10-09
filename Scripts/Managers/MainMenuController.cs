using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameData game_Data;

    [SerializeField] private Text _gameVersionText;
    [SerializeField] private Text _bestLevelText;
    [SerializeField] private Text _collectCoinsText;

    private void Start()
    {
    }

    public void RefreshMainMenu()
    {
        _gameVersionText.text = "Version: " + game_Data.GameVersion;

        if (PlayerPrefs.GetInt(game_Data.GetBestLevel()) <= 0 || !PlayerPrefs.HasKey(game_Data.GetBestLevel())) // Если значение отсутствует или оно равняется или меньше 0
        {
            _bestLevelText.text = game_Data.Zero;
        }
        else
        {
            _bestLevelText.text = PlayerPrefs.GetInt(game_Data.GetBestLevel()).ToString(); // Иначе дает значение из данных
        }

        if (PlayerPrefs.GetInt(game_Data.GetTotalStars()) <= 0 || !PlayerPrefs.HasKey(game_Data.GetTotalStars()))
        {
            _collectCoinsText.text = game_Data.Zero;
        }
        else
        {
            _collectCoinsText.text = PlayerPrefs.GetInt(game_Data.GetTotalStars()).ToString();
        }
    }
}
