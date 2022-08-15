using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : Manager<GameEvents>
{
    public event Action OnStartGame;

    public event Action OnLoadMainMenu;
    public event Action<string> OnUpdateGameText , OnEndGame;

    public event Action<GameController.GameState, GameController.GameState> GameState;

    internal void LoadMainMenu()
    {
        if(OnLoadMainMenu != null)
        {
            OnLoadMainMenu();
        }
    } 

    internal void StartGame()
    {
        if(OnStartGame != null)
        {
            OnStartGame();
        }
    }
    internal void OnGameStateUpdate(GameController.GameState curent, GameController.GameState priv)
    {
        if(GameState != null)
        {
            GameState(curent, priv);
        }
    }

    internal void GameOver(string stats)
    {
        if(OnEndGame != null)
        {
            OnEndGame(stats);
        }
    }
    internal void UpdateGameText(string stats)
    {
        if(OnUpdateGameText != null)
        {
            OnUpdateGameText(stats);
        }
    }
}
