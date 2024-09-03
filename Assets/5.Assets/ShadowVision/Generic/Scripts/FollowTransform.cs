using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowVision {
    public class FollowTransform : MonoBehaviour {
        public Transform followTarget;
        public bool followPosition = true;
        public bool followRotation = true;
        public bool lockXAxis = false;
        public bool lockYAxis = false;
        public bool lockZAxis = false;

        // Update is called once per frame
        void Update() {
            if (followPosition) {
                transform.position = followTarget.position;
            }

            if (followRotation) {
                if (lockXAxis || lockYAxis || lockZAxis) {
                    Vector3 target = followTarget.eulerAngles;
                    target.x = lockXAxis ? 0 : target.x;
                    target.y = lockYAxis ? 0 : target.y;
                    target.z = lockZAxis ? 0 : target.z;
                    transform.eulerAngles = target;
                } else {
                    transform.rotation = followTarget.rotation;
                }
            }
        }
    }
}