using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public GameVariables Manager;
    private TextMeshProUGUI mScoreText;

    private void OnEnable()
    {
        Manager.ScoreChanged += ScoreUpdate;
    }

    private void OnDisable()
    {
        Manager.ScoreChanged -= ScoreUpdate;
    }

    //private void AddScore(Object obj)
    //{
    //    Manager.Score += GameManagerVariables.ScoreIncrement;
    //}

    // Start is called before the first frame update
    void Start()
    {
        mScoreText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ScoreUpdate()
    {
        mScoreText.text = "Score: " + Manager.Score;
    }
}
