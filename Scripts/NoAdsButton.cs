using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAdsButton : MonoBehaviour
{
    private GameControl gameControl_;
    void Awake()
    {
        gameControl_ = FindObjectOfType<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameControl_.adsManager_.NoAds_)
        {
            gameObject.SetActive(false);
        }
    }
}
