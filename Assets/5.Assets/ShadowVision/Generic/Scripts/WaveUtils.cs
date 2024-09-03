using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowVision {
    public static class WaveUtils {
        public const float WaveCycle2Pi = 6.28318530718f;
        
        [System.Serializable]
        public struct GerstnerWaveData {
            public Vector2 dir;
            public float steepness;
            public float wavelength;
            public float speed;
        }
        
        [System.Serializable]
        public class WaveData {
            public Vector2 direction;
            public float amplitude = 1;
            public float frequency = 1;
            public float phase = 0;
        }
        
        public static float SinWave(float delta, float amplitude, float wavelength, float phase) {
            return Mathf.Sin(delta * WaveCycle2Pi * wavelength + phase) * amplitude;
        }

        public static float SinWave(float delta, WaveData waveData) {
            return ((Mathf.Sin(delta * 6.28318530718f * waveData.frequency + waveData.phase) + 1) / 2f) * waveData.amplitude;
        }

        public static float SinWave01(float delta, float amplitude, float wavelength, float phase) {
            return ((Mathf.Sin(delta * WaveCycle2Pi * wavelength + phase) + 1) / 2f) * amplitude;
        }

        /// <summary>
        /// Takes a grid position and applies a gerstner wave to it.
        /// </summary>
        /// <param name="position">The original position of the point</param>
        /// <param name="direction">The direction of the wave</param>
        /// <param name="steepness">How steep the wave is</param>
        /// <param name="wavelength">How long the wave is</param>
        /// <param name="speed">How fast is the wave moving</param>
        /// <returns>The new position of the point</returns>
        public static Vector3 GerstnerWave(Vector3 position, Vector2 direction, float steepness, float wavelength, float speed) {
                float k = 2 * Mathf.PI / wavelength;
                Vector2 d = direction.normalized;
                Vector3 p = new Vector2(position.x, position.z);
                float f = k * (Vector2.Dot(d, p) - speed * Time.time);
                float a = steepness / k;
                
                p.x += d.x * (a * Mathf.Cos(f));
                p.y = a * Mathf.Sin(f);
                p.z += d.y * (a * Mathf.Cos(f));

                return p;
        }

        public static Vector3 GerstnerWave(Vector3 position, GerstnerWaveData waveData) {
            return GerstnerWave(position, waveData.dir, waveData.steepness, waveData.wavelength, waveData.speed);
        }
    }
}
