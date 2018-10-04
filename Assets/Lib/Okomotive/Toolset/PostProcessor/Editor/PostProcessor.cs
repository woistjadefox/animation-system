using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Okomotive.Toolset.PostProcessor {

    public class SceneReferecesPostProcessor {

        public static PostProcessBehaviour[] _postProcessBehaviours;

        [PostProcessScene(1)]
        public static void OnPostprocessScene() {

            _postProcessBehaviours = Resources.FindObjectsOfTypeAll<PostProcessBehaviour>();

            for (int i = 0; i < _postProcessBehaviours.Length; i++) {

                if (_postProcessBehaviours[i].hideFlags == HideFlags.NotEditable || _postProcessBehaviours[i].hideFlags == HideFlags.HideAndDontSave) {
                    continue;
                }
                        
                if (EditorUtility.IsPersistent(_postProcessBehaviours[i].transform.root.gameObject)) {
                    continue;
                }

                _postProcessBehaviours[i].StartPostProcess();
            }
            
        }
    }
}
