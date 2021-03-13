using UnityEngine;
public class MovementController : MonoBehaviour
{
    Camera MainCamera;

    float Throttle;
    public float GasAmount;
    private Rigidbody mRigidBody;

    public Transform LeftBottom;
    public Transform RightUp;
    void Start()
    {
        MainCamera = Camera.main;
        mRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(this.transform.position, (-this.transform.up * Throttle) + this.transform.position, Color.green);
        mRigidBody.AddForce(this.transform.up * Throttle, ForceMode.Acceleration);
        mRigidBody.velocity = Vector3.ClampMagnitude(mRigidBody.velocity, 2);
    }


    void Update()
    {
        //Debug.DrawRay(this.transform.position, this.transform.up, Color.green);
        //Debug.DrawRay(this.transform.position, this.transform.right, Color.red);
        //Debug.DrawRay(this.transform.position, this.transform.forward, Color.blue);

        if (Input.GetKey("w") || Input.GetMouseButton(2))
        {
            Throttle += GasAmount;
            Mathf.Clamp(Throttle, 0, 10);
        }
        if (Input.GetKeyUp("w") || Input.GetMouseButtonUp(2))
        {
            Throttle = 0;
        }

        Vector3 mouseDir = MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        Vector3 pos = transform.position;
        Vector3 dirMove = mouseDir - pos;
        dirMove.Normalize();

        float rot = Mathf.Atan2(dirMove.y, dirMove.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rot - 90);

        //if (MainCamera.WorldToScreenPoint(transform.position).x >= MainCamera.pixelWidth)
        //    transform.position = new Vector3(MainCamera.ScreenToWorldPoint(new Vector3(0, 0, 10)).x, transform.position.y, 0);
        //if (MainCamera.WorldToScreenPoint(transform.position).x <= 0)
        //    transform.position = new Vector3(MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth, 0, 10)).x, transform.position.y, 0);
        //if (MainCamera.WorldToScreenPoint(transform.position).y >= MainCamera.pixelHeight)
        //    transform.position = new Vector3(transform.position.x, MainCamera.ScreenToWorldPoint(new Vector3(0, 0, 10)).y, 0);
        //if (MainCamera.WorldToScreenPoint(transform.position).y <= 0)
        //    transform.position = new Vector3(transform.position.x, MainCamera.ScreenToWorldPoint(new Vector3(0, MainCamera.pixelHeight, 10)).y, 0);

        transform.position = Warp(transform.position, LeftBottom.position, RightUp.position);
    }

    private Vector3 Warp(Vector3 position, Vector3 leftBottom, Vector3 rightUp)
    {
        if (position.x >= rightUp.x)
            position = new Vector3(leftBottom.x, position.y, position.z);
        else if (position.x <= leftBottom.x)
            position = new Vector3(rightUp.x, position.y, position.z);
        else if (position.y >= rightUp.y)
            position = new Vector3(position.x, leftBottom.y, position.z);
        else if (position.y <= leftBottom.y)
            position = new Vector3(position.x, rightUp.y, position.z);

        return position;
    }
}
