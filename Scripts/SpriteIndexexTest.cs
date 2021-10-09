using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteIndexexTest : MonoBehaviour
{
    [SerializeField] private Sprite sprite_;
    [SerializeField] private Image image_;
    [SerializeField] private index index_;

    enum index
    {
        first,
        second,
        third,
        fourth
    }

    private void Update()
    {
        switch (index_)
        {
        }
    }
}
