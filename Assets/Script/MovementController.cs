using UnityEngine;

public class MovementController : MonoBehaviour
{
    Camera MainCamera;

    void Start()
    {
        MainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 mouseDir = MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        Vector3 pos = transform.position;
        Vector3 dirMove = mouseDir - pos;
        dirMove.Normalize();

        float rot = Mathf.Atan2(dirMove.y, dirMove.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rot - 90);
    }
}
