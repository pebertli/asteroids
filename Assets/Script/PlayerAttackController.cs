using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform AttackPivot;
    public float BulletSpeed;
    public GameVariables Manager;
    private const float BulletCooldownMax = 0.3f;
    private float BulletCooldown = BulletCooldownMax;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        EventManager.StartListening("AsteroidDestroyed", AddScore);
    }

    private void OnDisable()
    {
        EventManager.StopListening("AsteroidDestroyed", AddScore);
    }

    public void AddScore(GameObject s)
    {
        Manager.Score += GameVariables.ScoreIncrement;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (BulletCooldown <= 0)
            {
                IntantiateBullet();
                BulletCooldown = BulletCooldownMax;
            }
        }

        BulletCooldown -= Time.deltaTime;
    }

    private void IntantiateBullet()
    {
        GameObject bullet = GameObject.Instantiate(BulletPrefab, AttackPivot.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(this.transform.up * BulletSpeed, ForceMode.Impulse);
        //bullet.transform.localScale = Vector3.one;
    }

    public void DestroyAllBullets()
    {
        foreach (Transform c in AttackPivot)
        {
            Destroy(c.gameObject);
        }
    }
}
