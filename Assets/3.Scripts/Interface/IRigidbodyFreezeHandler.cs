using UnityEngine;

public interface IRigidbodyFreezeHandler
{
    public void RigidFreezeHandler(ref Rigidbody rb, RigidbodyConstraints constraints);
}
