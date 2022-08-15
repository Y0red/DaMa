using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using MenuSystem;

public class GameController : MonoBehaviour
{
    public enum GameState {PLAYING, PAUSE, MENU};

    public GameState currentGameState = GameState.MENU;

    void Start()
    {
        GameEvents.Instance.GameState += UpdateGameState;
    }

    private void UpdateGameState(GameState arg1, GameState arg2)
    {
        throw new NotImplementedException();
    }
    public void UpdateState(GameState state)
    {
        GameState priviousGameState = currentGameState;
        currentGameState = state;

        switch (currentGameState)
        {
            case GameState.PLAYING:
                Time.timeScale = 1f;
                break;

            case GameState.PAUSE:
                Time.timeScale = 0f;
                break;

            case GameState.MENU:
                Time.timeScale = 1f;
                break;

            default:
                break;
        }

        
    }
}