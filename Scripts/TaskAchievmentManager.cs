using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TaskAchievmentManager : MonoBehaviour
{
    [SerializeField] private List<TaskAchievment> rewardsElements_;
    [SerializeField] private InGameNotificationManager inGameNotificationManager_;
    [SerializeField] private GameObject notificationIcon_;

    private void Start()
    {
        ProgressCheck();

        inGameNotificationManager_ = FindObjectOfType<InGameNotificationManager>();

        //foreach (TaskAchievment taskAchievment in Resources.FindObjectsOfTypeAll(typeof(TaskAchievment)) as TaskAchievment[])
        //{
        //    TaskAchievment taskAchievment1 = taskAchievment as TaskAchievment;
        //    if (taskAchievment1 != null && !EditorUtility.IsPersistent(taskAchievment1.transform.root.gameObject) && !(taskAchievment.hideFlags == HideFlags.NotEditable || taskAchievment.hideFlags == HideFlags.HideAndDontSave))
        //        rewardsElements_.Add(taskAchievment);
        //}
    }

    public void ProgressCheck()
    {
        foreach (TaskAchievment taskAchievment in rewardsElements_)
        {
            if (!taskAchievment.IsReady)
            {
                notificationIcon_.SetActive(false);
            }
        }

        foreach (TaskAchievment taskAchievment in rewardsElements_)
        {
            if (taskAchievment.IsReady)
            {
                notificationIcon_.SetActive(true);
                return;
            }
        }

    }
}
