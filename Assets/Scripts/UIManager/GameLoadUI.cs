using System;
using Platformer.Mechanics;
using Platformer.Observer;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameLoadUI : MonoBehaviour
{
    private float loading = 0.7f;

    private void Awake()
    {
        YandexGame.SwitchLangEvent += s => MultiTextUI.lang = s;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            GameManager.Instance.StartGame();
            this.PostEvent(EventID.Home);
            gameObject.SetActive(false);
        }
    }
}
