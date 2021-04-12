using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public GameObject Player;
    public float ParallaxFactor;

    Rigidbody mPlayerRB;
    MeshRenderer mMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        mPlayerRB = Player.GetComponent<Rigidbody>();
        mMeshRenderer = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mPlayerRB)
        {
            Vector2 offset = mMeshRenderer.material.GetVector("_Parallax");

            offset.x += mPlayerRB.velocity.x / ParallaxFactor;
            offset.y += mPlayerRB.velocity.y / ParallaxFactor;

            mMeshRenderer.material.SetVector("_Parallax", offset);
        }


    }
}
