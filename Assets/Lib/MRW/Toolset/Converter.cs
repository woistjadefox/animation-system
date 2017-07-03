using UnityEngine;

namespace MRW.Toolset {

    public static class Converter {

        public static Vector3 ScreenToWorld(Vector2 screenPos, float ClipPlane = 0) {

            if (ClipPlane == 0) {
                ClipPlane = UnityEngine.Camera.main.nearClipPlane;
            }

            return UnityEngine.Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, ClipPlane));

        }

        public static Vector2 WorldToScreen(Vector3 worldPos) {

            return UnityEngine.Camera.main.WorldToScreenPoint(worldPos);
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2, bool clamp=false) {

            if (clamp) {

                if(to2 > from2){
                    return Mathf.Clamp((value - from1) / (to1 - from1) * (to2 - from2) + from2, from2, to2);
                } else {
                    return Mathf.Clamp((value - from1) / (to1 - from1) * (to2 - from2) + from2, to2, from2);
                }
                
            }

            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float Remap(this float value, Vector4 map, bool clamp = false) {
            return Remap(value, map.x, map.y, map.z, map.w, clamp);
        }

    }
}
