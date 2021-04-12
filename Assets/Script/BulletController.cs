using UnityEngine;

public class BulletController : MonoBehaviour
{
    public ParticleSystem HitParticle;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
        {
            Instantiate(HitParticle, transform.position, Quaternion.Euler(collision.GetContact(0).normal));
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
