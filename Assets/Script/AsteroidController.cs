using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameVariables Variables;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Damage();
        }
    }

    public void Damage()
    {
        EventManager.TriggerEvent("AsteroidDestroyed", this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (gameObject.transform.localScale.x >= 0.3f)
            Variables.AsteroidAmount--;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
