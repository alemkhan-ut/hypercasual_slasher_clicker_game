using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameNotificationManager : MonoBehaviour
{
    [SerializeField] private float startXPosition_;
    [SerializeField] private float targetXPosition_;
    [SerializeField] private float moveDuration_;
    [SerializeField] private float messageWaitDuration_;

    [SerializeField] private Transform transform_;
    [SerializeField] private Text titleText_;
    [SerializeField] private Text messageText_;
    [SerializeField] private AudioSource sound_;

    [SerializeField] private GameObject newRewardMessage_;
    [SerializeField] private GameObject newWeaponMessage_;
    public enum NotificationType
    {
        NewReward,
        NewWeapon
    }

    private void Start()
    {
        transform_ = GetComponent<Transform>();
    }

    public void SetupNotification(NotificationType type)
    {
        switch (type)
        {
            case NotificationType.NewReward:
                newRewardMessage_.SetActive(true);
                newWeaponMessage_.SetActive(false);
                break;
            case NotificationType.NewWeapon:
                newRewardMessage_.SetActive(false);
                newWeaponMessage_.SetActive(true);
                break;
            default:
                break;
        }

        StartCoroutine(ShowNotification());
    }

    private IEnumerator ShowNotification()
    {
        transform_.DOLocalMoveY(targetXPosition_, moveDuration_).SetEase(Ease.InOutBack);
        sound_.Play();
        yield return new WaitForSeconds(messageWaitDuration_);
        CloseNotification();
    }

    private void CloseNotification()
    {
        transform_.DOLocalMoveY(startXPosition_, moveDuration_).SetEase(Ease.InOutBack);
    }

}
