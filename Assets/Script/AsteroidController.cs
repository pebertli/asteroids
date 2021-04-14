using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameVariables Variables;
    public AudioSource AudioDestruction;

    private Vector3 ExplosionPosition;
    private Rigidbody mRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Damage();
        }
    }

    public void Damage()
    {
        AudioDestruction.gameObject.transform.parent = null;
        GameObject.Destroy(AudioDestruction.gameObject, 2f);
        AudioDestruction.Play();
        EventManager.TriggerEvent("AsteroidDestroyed", this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (gameObject.transform.localScale.x >= 0.3f)
            Variables.AsteroidAmount--;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExplosionEffect(Vector3 pos, float delay = 0)
    {
        ExplosionPosition = pos;
        Invoke("ExplosionEffect", delay);
    }

    private void ExplosionEffect()
    {
        //Rigidbody rb = hit.GetComponent<AsteroidController>.GetComponent<Rigidbody>();

        if (mRigidbody != null)
            mRigidbody.AddExplosionForce(1500, ExplosionPosition, 20);
    }
}
