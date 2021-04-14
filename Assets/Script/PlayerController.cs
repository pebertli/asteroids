using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float GasAmount;
    public GameVariables Variables;
    public GameManager Manager;
    public MeshRenderer[] MaterialRocket;
    public GameObject BulletPrefab;
    public Transform AttackPivot;
    public float BulletSpeed;
    public ParallaxController Background1;
    public ParallaxController Background2;
    public AudioSource ThrotleAudio;
    public AudioSource LaserAudio;
    public AudioClip[] LaserAudioClip;

    const float BulletCooldownMax = 0.3f;
    float BulletCooldown = BulletCooldownMax;
    Camera mMainCamera;
    Rigidbody mRigidbody;
    ParticleSystem mBurstParticle;
    float mThrottle;
    float mRotVelocity;

    float InvencibleCooldown;
    const float InvencibleTime = 3.0f;

    void Start()
    {
        mMainCamera = Camera.main;
        mRigidbody = GetComponent<Rigidbody>();
        mBurstParticle = GetComponentInChildren<ParticleSystem>();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
        {
            if (InvencibleCooldown <= 0)
                Damage();
        }
    }

    void Damage()
    {

        EventManager.TriggerEvent("PlayerDestroyed", null);
        if (Variables.Health <= 0)
        {
            Manager.SetGameState(GameVariables.GameState.GameOver);

        }
        else
        {
            Respawn(true);
        }
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
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 20);
        foreach (Collider hit in colliders)
        {
            //Rigidbody rb = hit.GetComponent<AsteroidController>.GetComponent<Rigidbody>();

            //if (rb != null)
            //    rb.AddExplosionForce(1000, explosionPos, 10);
            if (hit.CompareTag("Asteroid"))
            {
                float delay = Vector3.Distance(hit.transform.position, transform.position) / 15;
                hit.GetComponent<AsteroidController>().ExplosionEffect(transform.position, delay);
            }
        }


        RaycastHit hitRay;
        //LayerMask mask = LayerMask.GetMask("Background");
        // Bit shift the index of the layer (8) to get a bit mask
        int mask = LayerMask.NameToLayer("Background");

        //// This would cast rays only against colliders in layer 8.
        //// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.forward, out hitRay, Mathf.Infinity, ~mask))
        {
            Vector3 offset1 = new Vector2(0.5f, 0.5f);
            Vector3 v1 = Camera.main.WorldToViewportPoint(hitRay.point);
            Vector3 v2 = Camera.main.WorldToViewportPoint(transform.position);
            //Debug.Log(v2 - v1);
            Vector3 pointVector = Background1.transform.InverseTransformPoint(hitRay.point) + offset1 + (v2 - v1);
            Background1.AddEffect(pointVector, 1f);
            Background2.AddEffect(pointVector, 1f);
            Manager.Warpsound();
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitRay.distance, Color.yellow);
            //Debug.Log("Did Hit");
        }
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.forward * 1000, Color.white);
        //    Debug.Log("Did not Hit");
        //}

        //Background1.AddEffect(new Vector2(0.5f, 0.5f), 2f);
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
                AudioClip laser = LaserAudioClip[Random.Range(0, LaserAudioClip.Length)];
                LaserAudio.PlayOneShot(laser);
                //InstantiateExplosion();
                BulletCooldown = BulletCooldownMax;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
            //{
            //    if (BulletCooldown <= 0)
            //    {
            //IntantiateBullet();
            InstantiateExplosion();
        //    BulletCooldown = BulletCooldownMax;
        //}    

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

        var emission = mBurstParticle.emission;
        emission.rateOverDistance = mThrottle * 3f;
        ThrotleAudio.volume = mThrottle / 10f;
        Debug.Log(mThrottle);
        if (mThrottle > 0)
        {
            ThrotleAudio.mute = false;
        }
        else
        {
            ThrotleAudio.mute = true;
        }


        //mBurstParticle.mo;

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
