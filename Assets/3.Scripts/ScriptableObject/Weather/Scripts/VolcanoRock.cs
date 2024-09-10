using UnityEngine;

public class VolcanoRock : MonoBehaviour
{
    const float min = 2f;
    const float max = 3f;

    float rotateSpeedX;
    float rotateSpeedY;
    float rotateSpeedZ;


    private void Start()
    {
        rotateSpeedX = Random.Range(min, max);
        rotateSpeedY = Random.Range(min, max);
        rotateSpeedZ = Random.Range(min, max);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(rotateSpeedX, rotateSpeedY, rotateSpeedZ));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("destory");
            Destroy(gameObject, 15f);
        }
    }
}