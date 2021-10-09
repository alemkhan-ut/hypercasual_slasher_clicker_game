using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Image BG_1;
    [SerializeField] private Image BG_2;

    [SerializeField] private Sprite[] simpleBGs_;
    [SerializeField] private Sprite[] bossBGs_;
    [SerializeField] private Color transparentColor_;

    [SerializeField] private float backgroundSwitchTime_;
    [SerializeField] private GameControl gameControl_;

    private void Awake()
    {
        gameControl_ = FindObjectOfType<GameControl>();
    }

    private void Start()
    {
        if (!gameControl_.IsDemo_)
        {
            StartCoroutine(BackgroundSwitch());
        }
    }

    public IEnumerator BackgroundSwitch(bool isBossBG = false)
    {
        if (isBossBG)
        {
            BG_2.DOFade(0, backgroundSwitchTime_);
            BG_2.sprite = GetRandomSprite(true);
            BG_2.DOFade(1, backgroundSwitchTime_);
        }
        else
        {
            BG_2.sprite = BG_1.sprite;
            yield return BG_2.DOFade(0, backgroundSwitchTime_).WaitForCompletion();
            BG_1.sprite = GetRandomSprite();
        }
    }

    private Sprite GetRandomSprite(bool isBossBG = false)
    {
        if (isBossBG)
        {
            return bossBGs_[Random.Range(0, bossBGs_.Length)];
        }
        else
        {
            return simpleBGs_[Random.Range(0, simpleBGs_.Length)];
        }
    }
}
