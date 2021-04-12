using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameVariables Variables;
    public PlayerController Player;
    public MainMenuController MainMenu;
    public HealthController HealthHud;
    public AsteroidSpawnController AsteroidSpawnController;
    public GameObject BackgroundAudio;
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
                BackgroundAudio.GetComponent<AudioSource>().Play();
                SetGameState(GameVariables.GameState.Playing);
                break;
            case GameVariables.GameState.Playing:
                Time.timeScale = 1;
                MainMenu.Show(false);
                BackgroundAudio.GetComponent<AudioLowPassFilter>().enabled = false;
                break;
            case GameVariables.GameState.Pause:
                Time.timeScale = 0;
                MainMenu.Show(true);
                BackgroundAudio.GetComponent<AudioLowPassFilter>().enabled = true;
                BackgroundAudio.GetComponent<AudioLowPassFilter>().cutoffFrequency = 200;
                //MainMenuController.Show();
                break;
            case GameVariables.GameState.GameOver:
                BackgroundAudio.GetComponent<AudioSource>().Stop();
                //gameObject.SetActive(false);
                Time.timeScale = 0;
                MainMenu.Show(true);
                //MainMenuController.Show();
                break;
            default:
                break;
        }

        //Variables.CurrentGameState = nextGameState;
    }

    public void Warpsound()
    {
        StartCoroutine("easingCos");
    }

    IEnumerator easingCos()
    {
        BackgroundAudio.GetComponent<AudioSource>().pitch = 1.0f;
        for (float i = 0; i < 1; i += 0.01f)
        {
            float res = (2 * i * Mathf.PI);
            BackgroundAudio.GetComponent<AudioSource>().pitch = (Mathf.Cos(res) + 1f) / 2f;
            Debug.Log(Mathf.Cos(res));
            yield return new WaitForSeconds(0.01f);
        }
        BackgroundAudio.GetComponent<AudioSource>().pitch = 1.0f;
        yield return null;
    }
}
