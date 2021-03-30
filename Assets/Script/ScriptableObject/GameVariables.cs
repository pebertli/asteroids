using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerVariables", menuName = "ScrpitableObjects/GameManagerVariables", order = 1)]
public class GameVariables : ScriptableObject
{
    private int mScore;
    public int Health;
    public int AsteroidAmount;

    public const int MaxAsteroidAmount = 5;
    public const int InitialScore = 0;
    public const int ScoreIncrement = 100;
    public const int InitialHealth = 3;

    public Action ScoreChanged = delegate { };

    public int Score { get => mScore; set { mScore = value; ScoreChanged(); } }

    public enum GameState
    {
        Start = 0,
        Playing,
        Pause,
        MainMenu,
        GameOver
    };

    public GameState CurrentGameState;


    // Start is called before the first frame update
    void OnEnable()
    {
        CurrentGameState = GameState.MainMenu;
        Score = InitialScore;
        Health = 3;
        AsteroidAmount = 0;
    }
}
