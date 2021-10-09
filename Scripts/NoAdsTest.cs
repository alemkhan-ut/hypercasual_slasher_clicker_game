using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoAdsTest : MonoBehaviour
{
    private GameControl gameControl_;

    // Start is called before the first frame update
    void Start()
    {
        gameControl_ = FindObjectOfType<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameControl_.adsManager_.NoAds_)
        {
            GetComponent<Text>().text = "-";
        }
        else
        {
            GetComponent<Text>().text = "+";
        }
    }
}
