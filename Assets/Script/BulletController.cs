using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
        {
            collision.collider.GetComponent<AsteroidController>().Damage();
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
