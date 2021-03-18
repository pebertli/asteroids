using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float GasAmount;

    Camera mMainCamera;
    Rigidbody mRigidbody;
    float mThrottle;
    float mRotVelocity;

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

    void Update()
    {
        Debug.DrawRay(transform.position, transform.up, Color.green);
        Debug.DrawRay(transform.position, transform.right, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

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
