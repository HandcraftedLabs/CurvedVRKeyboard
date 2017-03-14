using CurvedVRKeyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CurvedVRKeyboard {


    [CustomEditor(typeof(KeyboardStatus))]
    [CanEditMultipleObjects]
    public class KeyboardStatusEditor: Editor {


        private KeyboardStatus keybaordStatus;
        private Component[] filtred;
        private string[] scriptsNames;

        private bool notNulltargetAndChanged;
        private int currentSelected = 0;
        private int previousSelected = 0;
        private ErrorReporter errorRaporter;
        


        private static GUIContent OUTPUT = new GUIContent("Gameobject Output", "field receiving input from the keyboard (Text,InputField,TextMeshPro)");
        private static GUIContent OUTPUT_LENGTH = new GUIContent("Output Length", "Maximum output text length");
        private const string OUTPUT_TYPE = "Choose Output Script Type";
        private const string TEXT = "text";
        private const string WARNING = "Gamoeboject Output is not set, or There is no script with text on current gameobject";

        private void Awake () {
            keybaordStatus = target as KeyboardStatus;
            EmptyArrays();
            if(keybaordStatus.targetGameobject != null) {
                GetComponentsName();
            }
        }

        public override void OnInspectorGUI () {
            keybaordStatus = target as KeyboardStatus;
            keybaordStatus.maxOutputLength = EditorGUILayout.IntField(OUTPUT_LENGTH, keybaordStatus.maxOutputLength);
            CheckOutputGameboject();
            DrawPopupList();
            if(!IsReflectionPossible()) {
                ErrorReporter.Instance.SetMessage(WARNING,ErrorReporter.Status.Warning);
            }else {
                //ErrorReporter.Instance.Reset();
            }
            HandleValuesChanges();
        }

        private void GetComponentsName () {
            filtred = keybaordStatus.targetGameobject.GetComponents<Component>()
                .Where(x => x.GetType().GetProperty(TEXT) != null).ToArray();
            scriptsNames = filtred.Select(x => x.GetType().ToString()).ToArray<String>();
            currentSelected = 0;
            notNulltargetAndChanged = true;
        }

        private void DrawPopupList () {
            GUI.enabled = IsReflectionPossible();
            currentSelected = EditorGUILayout.Popup(OUTPUT_TYPE, currentSelected, scriptsNames);

            if(previousSelected != currentSelected) {//if popup value was chaned 
                notNulltargetAndChanged = true;
            }
            previousSelected = currentSelected;

            if(IsReflectionPossible() && notNulltargetAndChanged) {
                notNulltargetAndChanged = false;
                keybaordStatus.typeHolder = filtred[currentSelected];
                keybaordStatus.targetGameobject = filtred[currentSelected].gameObject;
                keybaordStatus.output = (string)filtred[currentSelected]
                    .GetType().GetProperty(TEXT).GetValue(filtred[currentSelected], null);
            }

        }

        private void CheckOutputGameboject () {
            EditorGUI.BeginChangeCheck();
            keybaordStatus.targetGameobject = EditorGUILayout.ObjectField(OUTPUT, keybaordStatus.targetGameobject, typeof(GameObject), true) as GameObject;
            if(keybaordStatus.targetGameobject != null && EditorGUI.EndChangeCheck()) { //if not null and changed
                GetComponentsName();
            }
            if(keybaordStatus.targetGameobject == null && EditorGUI.EndChangeCheck()) {// if set to null and changed
                EmptyArrays();
            }
        }

        private void EmptyArrays () {
            filtred = new Component[0];
            scriptsNames = new string[0];
        }

        public bool IsReflectionPossible () {
            return keybaordStatus.targetGameobject != null && filtred.Length > 0;
        }

        private void HandleValuesChanges () {
            if(GUI.changed) {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                EditorUtility.SetDirty(keybaordStatus);
            }
        }
    }

}