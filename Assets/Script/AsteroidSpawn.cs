using UnityEngine;

public class AsteroidSpawn : MonoBehaviour
{

    public GameObject AsteroidPrefab;
    public Transform LeftUp;
    public Transform RightBottom;
    public float CooldownSpawnMax;
    public const int SpawnMax = 10;

    private static int CurrentSpawnedAmmount;

    float CooldownSpawn;

    // Start is called before the first frame update
    void Start()
    {
        CooldownSpawn = CooldownSpawnMax;
    }

    private void Spawn(float scale)
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
        GameObject asteroid = Instantiate(AsteroidPrefab, SpawnPos, Random.rotation);
        asteroid.GetComponent<Rigidbody>().AddForce(Direction * Random.Range(70, 200), ForceMode.Acceleration);
        asteroid.GetComponent<Rigidbody>().AddTorque(Random.insideUnitCircle * Random.Range(70, 200), ForceMode.Acceleration);

        if (scale > 0.75f)
            asteroid.GetComponent<AsteroidController>().AsteroidSize = 1;
        else
            asteroid.GetComponent<AsteroidController>().AsteroidSize = 0;
        asteroid.transform.localScale = new Vector3(scale, scale, scale);

        CurrentSpawnedAmmount++;
    }

    private void Spawn(Vector3 position, float scale)
    {
        GameObject asteroid = Instantiate(AsteroidPrefab, position, Random.rotation);
        asteroid.GetComponent<Rigidbody>().AddForce(Random.insideUnitCircle * Random.Range(70, 200), ForceMode.Acceleration);
        asteroid.GetComponent<Rigidbody>().AddTorque(Random.insideUnitCircle * Random.Range(70, 200), ForceMode.Acceleration);

        asteroid.GetComponent<AsteroidController>().AsteroidSize = 0;
        asteroid.transform.localScale = new Vector3(scale, scale, scale);

        CurrentSpawnedAmmount++;
    }

    public void AsteroidDestroyed(Vector3 position, int asteroidSize)
    {
        if (asteroidSize >= 1)
        {
            Spawn(position, Random.Range(0.1f, 0.3f));
            Spawn(position, Random.Range(0.1f, 0.3f));
            Spawn(position, Random.Range(0.1f, 0.3f));
        }
        else
        {
            Spawn(position, Random.Range(0.1f, 0.3f));
            Spawn(position, Random.Range(0.1f, 0.3f));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (CooldownSpawn <= 0f)
        {
            float scale = Random.Range(0.5f, 1f);
            Spawn(scale);
            CooldownSpawn = CooldownSpawnMax;
        }
        else
        {
            CooldownSpawn -= Time.deltaTime;
        }
    }
}
