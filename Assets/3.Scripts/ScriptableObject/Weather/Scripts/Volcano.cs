using UnityEngine;

public class Volcano : RandomPosBase
{
    [SerializeField] private Transform target;
    private Rigidbody rb;
    private float initialAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private Vector3 GetVelocity(Vector3 start, Vector3 target, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0f, target.z);
        Vector3 planarPos = new Vector3(start.x, 0f, start.z);

        float distance = Vector3.Distance(planarTarget, planarPos);
        float yOffset = start.y - target.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * 
            Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / 
            (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0f,
            initialVelocity * Mathf.Sin(angle),
            initialVelocity * Mathf.Cos(angle));

        float angleBetweenObject = Vector3.Angle(Vector3.forward,
            planarTarget - planarPos) * (target.x > start.x ? 1 : -1);

        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObject,
            Vector3.up) * velocity;

        return finalVelocity;
    }
}
