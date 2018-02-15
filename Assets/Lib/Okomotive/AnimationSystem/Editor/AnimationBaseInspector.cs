
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Okomotive.AnimationSystem {

    [CustomEditor(typeof(AnimationBase), true, isFallback = true)]
    [CanEditMultipleObjects]
    public class AnimationBaseInspector : Editor {

#pragma warning disable 414
        private AnimationBase animation;
#pragma warning restore 414

        private bool onPlay = false;

        private ReorderableList states;
        private bool[] showState;
        private bool showPlayer;
        private int playState;

        public void OnEnable() {
        }

        public override void OnInspectorGUI() {

            animation = (AnimationBase)target;

            // show default inspector property editor withouth script referenz
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, new string[] { "m_Script" });

            showPlayer = EditorGUILayout.Foldout(showPlayer, new GUIContent("Player"));

            if (showPlayer) {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("State");
                playState = EditorGUILayout.IntField(playState, GUILayout.Width(20));

                if (onPlay == false && GUILayout.Button("Play", GUILayout.Width(100))) {

                    if (animation.StateExists(playState)) {
                        animation.EditorPlayState(playState);
                        onPlay = true;
                    } else {
                        EditorUtility.DisplayDialog("State does not exists", "State " + playState + " is not defined!", "ok");
                    }
                }

                if (onPlay) {
                    if (GUILayout.Button("Reset", GUILayout.Width(100))) {

                        if (animation.StateExists(playState)) {
                            animation.EditorPlayReset();
                            onPlay = false;
                        } else {
                            EditorUtility.DisplayDialog("State does not exists", "State " + playState + " is not defined!", "ok");
                        }

                    }
                }

                GUILayout.EndHorizontal();
            }
                   
            serializedObject.ApplyModifiedProperties();
        }
    }
}
