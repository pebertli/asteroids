using UnityEngine;

public class HealthController : MonoBehaviour
{
    public GameVariables Manager;
    private void OnEnable()
    {
        EventManager.StartListening("PlayerDestroyed", PlayerDestroyed);
    }

    private void OnDisable()
    {
        EventManager.StopListening("PlayerDestroyed", PlayerDestroyed);
    }

    public void UpdateHealthHud()
    {
        switch (Manager.Health)
        {
            case (3):
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                break;
            case (2):
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(false);
                break;
            case (1):
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                break;
            default:
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }

    void PlayerDestroyed(Object obj)
    {
        Manager.Health--;
        UpdateHealthHud();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
