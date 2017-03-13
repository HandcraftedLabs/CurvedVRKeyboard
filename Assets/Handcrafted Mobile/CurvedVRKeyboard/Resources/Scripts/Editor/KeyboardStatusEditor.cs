//using CurvedVRKeyboard;
//using UnityEditor;
//using UnityEngine;

//namespace CurvedVRKeyboard {


//    [CustomEditor(typeof(KeyboardStatus))]
//    [CanEditMultipleObjects]
//    public class KeyboardStatusEditor: Editor {

//        private KeyboardStatus keybaordStatus;

//        private static GUIContent OUTPUT = new GUIContent("Output", "field receiving input from the keyboard");
//        private static GUIContent OUTPUT_LENGTH = new GUIContent("Output Length", "Maximum output text length");




//        public void OnGUI () {
//            keybaordStatus = target as KeyboardStatus;

//            //keybaordStatus.RaycastingSource = EditorGUILayout.ObjectField(RAYCASTING_SOURCE_CONTENT, keyboardCreator.RaycastingSource, typeof(Transform), true) as Transform;

//        }
//    }

//}