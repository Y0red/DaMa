using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuSystem
{
    public class GamePlayMenu : MonoBehaviour
    {
        [SerializeField] Button pauseButton, mainMenu;
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text winnerText;
        [SerializeField] Transform endGameHolder;

        // Start is called before the first frame update
        void Start()
        {
            pauseButton.onClick.AddListener(OnPause);
            GameEvents.Instance.OnUpdateGameText += UpdateGameText;
            mainMenu.onClick.AddListener(OnMainMenu);
            GameEvents.Instance.OnEndGame += OnEndGame;
        }

        private void OnEndGame(string obj)
        {
            endGameHolder.gameObject.SetActive(true);
            winnerText.text = $"{obj} is the Winner";
        }

        private void OnMainMenu()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private void UpdateGameText(string obj)
        {
            scoreText.text = $"{obj} turn";
        }

        private void OnPause()
        {
            throw new NotImplementedException();
        }
    }
}
