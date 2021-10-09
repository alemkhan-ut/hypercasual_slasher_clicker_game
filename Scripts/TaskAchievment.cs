using System;
using UnityEngine;
using UnityEngine.UI;

public class TaskAchievment : MonoBehaviour
{
    GameData game_Data;
    TaskAchievmentManager achievmentManager_;

    public string taskName;
    public string taskDescription;
    public GameObject taskIsDoneObject;
    public int taskRewardValue;

    public bool isTask;
    public TaskNumber taskNumber;
    public bool isAchievments;
    public TaskNumber achievmentNumber;
    public ActionType actionType;

    public bool IsActived; // 0 - true / 1 - false
    public bool IsReady;
    public bool IsRewarded;
    public Color buttonReadyColor_;
    public Color buttonLockColor_;
    public Color buttonActivatedColor_;

    public int progressValueNow; // Текущий прогресс
    public int progressValueDesired; // Нужный прогресс

    public Text taskDescriptionText; // текст задачи

    public Image progressBar; // прогрессБар задачи
    public Text progressText; // текст прогрессБара

    public Text taskRewardValueText; // текст с вознаграждением

    public Button rewardButton;

    private GameControl gameControl_;
    private int hitsCounter_;

    private DateTime lastTimeToTaskReward_;

    public enum TaskNumber
    {
        number01,
        number02,
        number03,
        number04,
        number05,
        number06,
        number07,
        number08,
        number09,
        number10
    }

    public enum ActionType
    {
        HitCounter,
        StarCounter,
        EnemyDestroyCounter,
        BossDestroyCounter,
        RecordTracker,

    }

    private void Awake()
    {
        game_Data = Resources.Load<GameData>("Game Data");

        gameControl_ = FindObjectOfType<GameControl>();
        achievmentManager_ = FindObjectOfType<TaskAchievmentManager>();
    }

    private void Start()
    {
        //taskDescriptionText.text = taskDescription;

        taskRewardValueText.text = taskRewardValue.ToString();

        ProgressCheck();
    }

    public void IsActivedCheck()
    {
        if (isTask)
        {
            if (PlayerPrefs.GetInt(game_Data.TaskStatus_ + taskNumber) >= 100)
            {
                IsActived = false;
            }
        }
        if (isAchievments)
        {
            if (PlayerPrefs.GetInt(game_Data.AchievmentValue + achievmentNumber) >= 100)
            {
                IsActived = false;
            }
        }
    }

    public void ProgressCheck()
    {
        IsActivedCheck();

        if (IsActived)
        {
            switch (actionType)
            {
                case ActionType.HitCounter:
                    progressValueNow = (PlayerPrefs.GetInt(game_Data.HitsCounter_));
                    break;
                case ActionType.StarCounter:
                    progressValueNow = PlayerPrefs.GetInt(game_Data.StarCollectInSession);
                    break;
                case ActionType.EnemyDestroyCounter:
                    progressValueNow = PlayerPrefs.GetInt("TotalEnemyDestroyed");
                    break;
                case ActionType.BossDestroyCounter:
                    progressValueNow = PlayerPrefs.GetInt("TotalBossDestroyed");
                    break;
                case ActionType.RecordTracker:
                    break;
                default:
                    break;
            }
        }
        else
        {
            taskIsDoneObject.SetActive(true);
        }

        progressText.text = progressValueNow + " / " + progressValueDesired;

        progressBar.fillAmount = progressValueNow / (float)progressValueDesired;

        if (progressBar.fillAmount >= 1)
        {
            IsReady = true;
            rewardButton.interactable = IsReady;
            rewardButton.gameObject.GetComponent<Image>().color = buttonReadyColor_;
        }
        else
        {
            IsReady = false;
            rewardButton.interactable = IsReady;
            rewardButton.gameObject.GetComponent<Image>().color = buttonLockColor_;
        }
    }

    public void GetReward()
    {
        IsActived = false;
        IsRewarded = true;

        achievmentManager_.ProgressCheck();

        if (isTask)
        {
            PlayerPrefs.SetInt(game_Data.TaskStatus_ + taskNumber, 100);
            Debug.Log("Сохранение значения задачи в ключ: " + game_Data.TaskStatus_ + taskNumber);
        }

        if (isAchievments)
        {
            PlayerPrefs.SetInt(game_Data.AchievmentValue + achievmentNumber, 100);
            Debug.Log("Сохранение значения достижения в ключ: " + game_Data.AchievmentValue + achievmentNumber);
        }

        progressValueNow = 0;

        taskIsDoneObject.SetActive(true);

        PlayerPrefs.SetInt(game_Data.GetTotalStars(), PlayerPrefs.GetInt(game_Data.GetTotalStars()) + taskRewardValue);

        gameControl_.UpdateUI();
        lastTimeToTaskReward_ = DateTime.Now;

        ProgressCheck();
    }
}
