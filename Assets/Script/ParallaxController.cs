using System.Collections;
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

    public void AddEffect(Vector2 center, float duration)
    {
        mMeshRenderer.material.SetVector("_Center", center);
        mMeshRenderer.material.SetInt("_Shockwave", 1);
        StartCoroutine("CountEffect", duration);
    }

    IEnumerator CountEffect(float duration)
    {
        float currentDuration = 0f;

        while (currentDuration < 1.0f)
        {
            currentDuration += Time.deltaTime * (1f / duration);
            mMeshRenderer.material.SetFloat("_Duration", currentDuration);
            yield return null;
        }
        mMeshRenderer.material.SetInt("_Shockwave", 0);
        yield break;
    }
}
