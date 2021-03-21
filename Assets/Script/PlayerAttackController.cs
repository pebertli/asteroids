using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform AttackPivot;
    public float BulletSpeed;
    private const float BulletCooldownMax = 0.3f;
    private float BulletCooldown = BulletCooldownMax;

    // Start is called before the first frame update
    void Start()
    {

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
    }
}
