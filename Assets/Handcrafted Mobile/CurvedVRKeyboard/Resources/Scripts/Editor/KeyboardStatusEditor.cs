using CurvedVRKeyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CurvedVRKeyboard {


    [CustomEditor(typeof(KeyboardStatus))]
    [CanEditMultipleObjects]
    public class KeyboardStatusEditor: Editor {


        private KeyboardStatus keybaordStatus;
        private GameObject current;
        private Component[] components;
        private Component[] componentsWithText;
        private Component[] filtred;
        private string[] scriptsNames;

        private int currentSelected;
        private int previousSelected = -1;
        private bool changedTargetGameobject = false;

        private static GUIContent OUTPUT = new GUIContent("Gameobject Output", "field receiving input from the keyboard (Text,InputField,TextMeshPro)");
        private static GUIContent OUTPUT_LENGTH = new GUIContent("Output Length", "Maximum output text length");
        private static string OUTPUT_TYPE = "Choose Output Type";

        private void Awake () {
            keybaordStatus = target as KeyboardStatus;
            GetComponentsName();
        }

        public override void OnInspectorGUI () {
            keybaordStatus = target as KeyboardStatus;
            CheckOutputGameboject();
            DrawPopupList();
            EditorUtility.SetDirty(keybaordStatus);
        }

        private void DrawPopupList () {
            if(filtred.Length > 0) {
                currentSelected = EditorGUILayout.Popup( previousSelected, scriptsNames);
                if(( changedTargetGameobject || currentSelected != previousSelected ) && currentSelected != -1) {//if any selected 
                    previousSelected = currentSelected;
                    changedTargetGameobject = false;
                    keybaordStatus.type = filtred[previousSelected];
                    keybaordStatus.targetGameobject = filtred[previousSelected].gameObject;
                    keybaordStatus.output = (string)filtred[previousSelected].GetType().GetProperty("text").GetValue(filtred[previousSelected], null);
                }
            }
        }

        private void CheckOutputGameboject () {
            EditorGUI.BeginChangeCheck();
            keybaordStatus.targetGameobject = EditorGUILayout.ObjectField(OUTPUT, keybaordStatus.targetGameobject, typeof(GameObject), true) as GameObject;
            if(keybaordStatus.targetGameobject != null && EditorGUI.EndChangeCheck()) { //if not null and changed
                GetComponentsName();
            }
            if(keybaordStatus.targetGameobject == null && EditorGUI.EndChangeCheck()) {// if set to null and changed
                filtred = new Component[0];
            }
        }

        private void GetComponentsName () {
            components = keybaordStatus.targetGameobject.GetComponents<Component>();
            filtred = components.Where(x => x.GetType().GetProperty("text") != null).ToArray();
            scriptsNames = filtred.Select(x => x.GetType().ToString()).ToArray<String>();
            changedTargetGameobject = true;
            previousSelected = -1;
        }
    }

}