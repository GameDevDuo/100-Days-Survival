using System.Collections.Generic;
using UnityEngine;

public class TornadoCenter : MonoBehaviour, IRigidbodyFreezeHandler
{
    [SerializeField] private GameObject pullingObject;

    private Collider[] colliders;
    private LayerMask layer;

    private float radius = 350f;
    private float speed = 4f;

    private void Start()
    {
        layer = (1 << 7) | (1 << 9);
    }

    private void Update()
    {
        if (GameManager.Instance.CheckForObject(out colliders, transform.position, radius, layer))
        {
            Debug.Log("Check!");
            foreach (Collider col in colliders)
            {
                Debug.Log("Pulling!");
                Rigidbody rb = col.GetComponent<Rigidbody>();
                rb.useGravity = false;
                col.transform.position = Vector3.MoveTowards(
                    col.transform.position,
                    pullingObject.transform.position,
                    speed * Time.deltaTime
                    );
            }
        }
        else
        {
            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                rb.useGravity = true;
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
