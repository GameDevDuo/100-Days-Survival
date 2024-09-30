using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TornadoCenter : MonoBehaviour, IRigidbodyFreezeHandler
{
    [SerializeField] private GameObject pullingObject;

    private Collider[] colliders;
    private LayerMask layer;

    private float radius = 250f;
    private float speed = 4f;

    private float minV = 1f;
    private float maxV = 5f;

    private void Start()
    {
        layer = (1 << 7) | (1 << 9);
    }

    private void Update()
    {
        if (GameManager.Instance.CheckForObject(out colliders, transform.position, radius, layer))
        {
            speed += 0.1f * Time.deltaTime;
            Debug.Log("Check!");
            foreach (Collider col in colliders)
            {
                Debug.Log("Pulling!");
                
                col.transform.position = Vector3.MoveTowards(
                    col.transform.position,
                    pullingObject.transform.position,
                    speed * Time.deltaTime
                    );

                if (col.gameObject.layer == 9)
                {
                    GameObject gameObject = col.gameObject;
                    gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    GameManager.Instance.RotateObject(ref gameObject, minV, maxV, maxV, maxV);
                }
            }
        }
        else
        {
            foreach (Collider col in colliders)
            {
                col.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == (1 << 9))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.layer == (1 << 7))
        {
            Debug.Log("Player Die!");
        }
    }

    public void RigidFreezeHandler(ref Rigidbody rb, RigidbodyConstraints constraints)
    {
        rb.constraints = constraints;
    }
}
