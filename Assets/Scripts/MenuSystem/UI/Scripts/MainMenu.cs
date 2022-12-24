using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MenuSystem
{
    public class MainMenu : MonoBehaviour
    {
        public Button play, playAi, exit;
        
        void Start()
        {
            play.onClick.AddListener(OnPlay);
            playAi.onClick.AddListener(OnPlayAi);
            exit.onClick.AddListener(OnExit);
        }

        private void OnPlayAi()
        {
            BoardManager.Instance.EnableAI();
            BoardManager.Instance.isAiPlayer = true;
            GameEvents.Instance.StartGame();
            //OnBuyMeTea();
        }

        private void OnExit()
        {
            Application.Quit();
        }

        private void OnPlay()
        {
            GameEvents.Instance.StartGame();
        }
       

    }
}
