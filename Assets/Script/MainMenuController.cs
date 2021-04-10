using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameVariables Variables;
    public GameManager Manager;
    public GameObject MainPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayClicked()
    {
        if (Variables.CurrentGameState == GameVariables.GameState.MainMenu)
        {
            Manager.SetGameState(GameVariables.GameState.Start);
        }
        else if (Variables.CurrentGameState == GameVariables.GameState.Pause)
        {
            Manager.SetGameState(GameVariables.GameState.Playing);
        }
        else if (Variables.CurrentGameState == GameVariables.GameState.GameOver)
        {
            Manager.SetGameState(GameVariables.GameState.Start);
        }
    }

    public void RestartClicked()
    {
        if (Variables.CurrentGameState == GameVariables.GameState.Pause)
        {
            Manager.SetGameState(GameVariables.GameState.Start);
        }
        else if (Variables.CurrentGameState == GameVariables.GameState.GameOver)
        {
            Manager.SetGameState(GameVariables.GameState.Start);
        }
    }

    public void ExitClicked()
    {
        Application.Quit();
    }

    public void Show(bool show)
    {
        MainPanel.SetActive(show);
    }
}
