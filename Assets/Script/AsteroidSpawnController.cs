using UnityEngine;

public class AsteroidSpawnController : MonoBehaviour
{

    public GameObject AsteroidPrefab;
    public Transform LeftUp;
    public Transform RightBottom;
    public float CooldownSpawnMax;
    public Transform SpawnParent;
    public GameVariables Variables;

    private float mCooldownSpawn;

    // Start is called before the first frame update
    void Start()
    {
        mCooldownSpawn = CooldownSpawnMax;
    }

    private void OnEnable()
    {
        EventManager.StartListening("AsteroidDestroyed", AsteroidDestroyed);
    }

    private void OnDisable()
    {
        EventManager.StopListening("AsteroidDestroyed", AsteroidDestroyed);
    }

    private void RandomOnScreenBoundary()
    {
        int normalizedPos = Random.Range(0, 4);
        Vector3 SpawnPos = Vector3.zero;
        Vector3 Direction = Vector3.up;
        switch (normalizedPos)
        {
            case 0:
                SpawnPos = new Vector3(LeftUp.position.x, Random.Range(LeftUp.position.y, RightBottom.position.y), transform.position.z);
                Direction = Vector3.right;
                break;
            case 1:
                SpawnPos = new Vector3(Random.Range(LeftUp.position.x, RightBottom.position.x), LeftUp.position.y, transform.position.z);
                Direction = -Vector3.up;
                break;
            case 2:
                SpawnPos = new Vector3(RightBottom.position.x, Random.Range(LeftUp.position.y, RightBottom.position.y), transform.position.z);
                Direction = -Vector3.right;
                break;
            case 3:
                SpawnPos = new Vector3(Random.Range(LeftUp.position.x, RightBottom.position.x), RightBottom.position.y, transform.position.z);
                Direction = Vector3.up;
                break;
        }

        Spawn(AsteroidPrefab, SpawnPos, Direction * Random.Range(70, 200), Random.Range(0.3f, 1.0f));
    }

    private void Spawn(GameObject prefab, Vector3 position, Vector3 direction, float scale)
    {
        GameObject asteroid = Instantiate(prefab, position, Random.rotation, SpawnParent);
        asteroid.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
        asteroid.GetComponent<Rigidbody>().AddTorque(Random.insideUnitCircle * Random.Range(70, 200), ForceMode.Acceleration);
        asteroid.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void AsteroidDestroyed(GameObject asteroidDestroid)
    {
        if (asteroidDestroid.transform.localScale.x >= 0.3f)
        {
            Spawn(AsteroidPrefab, asteroidDestroid.transform.position, Random.insideUnitCircle * Random.Range(70, 200), Random.Range(0.1f, 0.3f));
            Spawn(AsteroidPrefab, asteroidDestroid.transform.position, Random.insideUnitCircle * Random.Range(70, 200), Random.Range(0.1f, 0.3f));
            Spawn(AsteroidPrefab, asteroidDestroid.transform.position, Random.insideUnitCircle * Random.Range(70, 200), Random.Range(0.1f, 0.3f));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (mCooldownSpawn <= 0f && Variables.AsteroidAmount < GameVariables.MaxAsteroidAmount)
        {
            RandomOnScreenBoundary();
            mCooldownSpawn = CooldownSpawnMax;
            Variables.AsteroidAmount++;
        }
        else
        {
            mCooldownSpawn -= Time.deltaTime;
        }
    }
}
