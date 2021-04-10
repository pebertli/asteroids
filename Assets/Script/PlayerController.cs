using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float GasAmount;
    public GameVariables Variables;
    public MeshRenderer[] MaterialRocket;
    public GameObject BulletPrefab;
    public Transform AttackPivot;
    public float BulletSpeed;

    const float BulletCooldownMax = 0.3f;
    float BulletCooldown = BulletCooldownMax;
    Camera mMainCamera;
    Rigidbody mRigidbody;
    float mThrottle;
    float mRotVelocity;

    float InvencibleCooldown;
    const float InvencibleTime = 3.0f;

    void Start()
    {
        mMainCamera = Camera.main;
        mRigidbody = GetComponent<Rigidbody>();
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
        Variables.Score += GameVariables.ScoreIncrement;
    }

    private void FixedUpdate()
    {
        mRigidbody.AddForce(transform.up * mThrottle, ForceMode.Acceleration);
        mRigidbody.velocity = Vector3.ClampMagnitude(mRigidbody.velocity, 2);
    }

    public void Respawn(bool invencible)
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, -180, 0);
        mRigidbody.velocity = Vector3.zero;
        mRigidbody.angularVelocity = Vector3.zero;
        gameObject.SetActive(true);

        SetInvencible(invencible);
    }

    void SetInvencible(bool enable)
    {
        if (enable)
        {
            StartCoroutine("Blink");
            InvencibleCooldown = InvencibleTime;
            gameObject.GetComponent<Collider>().enabled = false;
        }
        else
        {
            InvencibleCooldown = 0f;
            gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    IEnumerator Blink()
    {
        for (int i = 0; i < 24; ++i)
        {
            foreach (var m in MaterialRocket)
                m.enabled = !m.enabled;
            for (float active = 0.0f; active < 0.125f; active += Time.deltaTime)
            {
                yield return null;
            }
        }

        yield break;
    }

    private void IntantiateBullet()
    {
        GameObject bullet = GameObject.Instantiate(BulletPrefab, AttackPivot.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(this.transform.up * BulletSpeed, ForceMode.Impulse);
    }

    private void InstantiateExplosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 5);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(1000, explosionPos, 5);
        }
    }

    public void DestroyAllBullets()
    {
        foreach (Transform c in AttackPivot)
        {
            Destroy(c.gameObject);
        }
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.up, Color.green);
        Debug.DrawRay(transform.position, transform.right, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (BulletCooldown <= 0)
            {
                IntantiateBullet();
                //InstantiateExplosion();
                BulletCooldown = BulletCooldownMax;
            }
        }

        BulletCooldown -= Time.deltaTime;

        if (InvencibleCooldown > 0)
            InvencibleCooldown -= Time.deltaTime;
        else
        {
            SetInvencible(false);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w"))
        {
            mThrottle += GasAmount;
            mThrottle = Mathf.Clamp(mThrottle, 0, 10);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp("w"))
        {
            mThrottle = 0;
        }

        Vector3 mouseDir = mMainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        Vector3 pos = transform.position;
        Vector3 dirMove = mouseDir - pos;
        dirMove.Normalize();

        float rot = Mathf.Atan2(dirMove.y, dirMove.x) * Mathf.Rad2Deg - 90;

        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, rot, ref mRotVelocity, 0.1f);

        transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);

        if (mMainCamera.WorldToScreenPoint(transform.position).x >= mMainCamera.pixelWidth)
        {
            transform.position = new Vector3(mMainCamera.ScreenToWorldPoint(new Vector3(0, 0, 10)).x, transform.position.y, transform.position.z);
        }
        if (mMainCamera.WorldToScreenPoint(transform.position).x <= 0)
        {
            transform.position = new Vector3(mMainCamera.ScreenToWorldPoint(new Vector3(mMainCamera.pixelWidth, 0, 10)).x, transform.position.y, transform.position.z);
        }
        if (mMainCamera.WorldToScreenPoint(transform.position).y >= mMainCamera.pixelHeight)
        {
            transform.position = new Vector3(transform.position.x, mMainCamera.ScreenToWorldPoint(new Vector3(0, 0, 10)).y, transform.position.z);
        }
        if (mMainCamera.WorldToScreenPoint(transform.position).y <= 0)
        {
            transform.position = new Vector3(transform.position.x, mMainCamera.ScreenToWorldPoint(new Vector3(0, mMainCamera.pixelHeight, 10)).y, transform.position.z);
        }

    }
}
