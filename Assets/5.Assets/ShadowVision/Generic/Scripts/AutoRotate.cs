using UnityEngine;
using System.Collections;

namespace ShadowVision {
	public class AutoRotate : MonoBehaviour {
		public float speedMod = 1;
		public Vector3 vel;
		public Vector3 randMax;
		public Vector3 randMin;

		public Space space = Space.Self;

		// Use this for initialization
		void Start() {
			vel += new Vector3(Random.Range(randMin.x, randMax.x), Random.Range(randMin.y, randMax.y),
				Random.Range(randMin.z, randMax.z));
		}

		// Update is called once per frame
		void LateUpdate() {
			transform.Rotate(vel * speedMod * Time.deltaTime * 10, space);
		}
	}
}
