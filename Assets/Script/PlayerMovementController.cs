using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float GasAmount;
    public GameVariables Variables;
    public GameManager Manager;
    public MeshRenderer[] MaterialRocket;

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
            //Destroy(this.gameObject);
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

    void Update()
    {
        Debug.DrawRay(transform.position, transform.up, Color.green);
        Debug.DrawRay(transform.position, transform.right, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

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