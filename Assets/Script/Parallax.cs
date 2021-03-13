using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject Player;
    public float ParallaxEffect;

    MeshRenderer mMesh;
    Rigidbody mPlayerRB;

    // Start is called before the first frame update
    void Start()
    {
        mMesh = GetComponent<MeshRenderer>();
        mPlayerRB = Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 offset = mMesh.material.mainTextureOffset;
        //we need to offset in normalized unit
        //offset.x = Player.transform.position.x / transform.localScale.x * ParallaxEffect;
        //offset.y = Player.transform.position.y / transform.localScale.y * ParallaxEffect;
        offset.x += mPlayerRB.velocity.x / ParallaxEffect;
        offset.y += mPlayerRB.velocity.y / ParallaxEffect;

        mMesh.material.mainTextureOffset = offset;
    }
}
