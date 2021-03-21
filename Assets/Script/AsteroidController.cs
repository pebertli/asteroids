using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [HideInInspector]
    public int AsteroidSize;

    public AsteroidSpawn AsteroidSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 5f);
    }

    public void Damage()
    {
        Destroy();
    }

    private void Destroy()
    {
        AsteroidSpawn.AsteroidDestroyed(transform.position, AsteroidSize);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
