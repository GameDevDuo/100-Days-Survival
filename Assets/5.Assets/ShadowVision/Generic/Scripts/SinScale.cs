using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowVision {
    public class SinScale : SinAnimation {
        public bool scaleX = true;
        public bool scaleY = true;
        public bool scaleZ = true;
        public float minScale = 1;
        public float maxScale = 2;

        protected override void OnValueChange(float delta) {
            var newScale = transform.localScale;
            float scale = Mathf.Lerp(minScale, maxScale, delta);
            newScale.x = scaleX ? scale : newScale.x;
            newScale.y = scaleY ? scale : newScale.y;
            newScale.z = scaleZ ? scale : newScale.z;
            transform.localScale = newScale;
        }
    }
}
