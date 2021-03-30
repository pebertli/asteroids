using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameVariables Variables;
    public PlayerMovementController Player;
    public MainMenuController MainMenu;
    public HealthController HealthHud;
    public AsteroidSpawnController AsteroidSpawnController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            if (Variables.CurrentGameState == GameVariables.GameState.Playing)
            {
                SetGameState(GameVariables.GameState.Pause);
            }
            else if (Variables.CurrentGameState == GameVariables.GameState.Pause)
            {
                SetGameState(GameVariables.GameState.Playing);
            }
        }
    }

    public void SetGameState(GameVariables.GameState nextGameState)
    {
        Variables.CurrentGameState = nextGameState;
        switch (Variables.CurrentGameState)
        {
            case GameVariables.GameState.MainMenu:
                MainMenu.Show(true);
                break;
            case GameVariables.GameState.Start:
                Player.Respawn(false);
                AsteroidSpawnController.DestroyAllAsteroids();
                Variables.Score = GameVariables.InitialScore;
                Variables.Health = GameVariables.InitialHealth;
                HealthHud.UpdateHealthHud();
                //Player.DestroyAll();
                SetGameState(GameVariables.GameState.Playing);
                break;
            case GameVariables.GameState.Playing:
                Time.timeScale = 1;
                MainMenu.Show(false);
                break;
            case GameVariables.GameState.Pause:
                Time.timeScale = 0;
                MainMenu.Show(true);
                //MainMenuController.Show();
                break;
            case GameVariables.GameState.GameOver:
                gameObject.SetActive(false);
                Time.timeScale = 0;
                MainMenu.Show(true);
                //MainMenuController.Show();
                break;
            //case GameManagerVariables.GameState.Dead:
            //    //if (ManagerVariables.Health <= 0)
            //    //MainMenuController.Show();
            //    break;
            default:
                break;
        }

        //Variables.CurrentGameState = nextGameState;
    }
}
