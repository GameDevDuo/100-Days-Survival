using System.Collections;
using System.Collections.Generic;
using ShadowVision;
using UnityEngine;

namespace ShadowVision {
    public abstract class SinAnimation : MonoBehaviour {
        public float speed = 1;
        public float wavelength = 1;
        public float phase = 0;

        // Update is called once per frame
        void Update() {
            OnValueChange(WaveUtils.SinWave01(Time.time * speed, 1, wavelength, phase));
        }

        /// <summary>
        /// Zero to one sin animation
        /// </summary>
        /// <param name="delta">0-1 value</param>
        protected abstract void OnValueChange(float delta);
    }
}