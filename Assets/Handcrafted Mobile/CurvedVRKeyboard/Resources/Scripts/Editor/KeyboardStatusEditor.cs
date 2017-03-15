using CurvedVRKeyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CurvedVRKeyboard {

    /// <summary>
    /// Special editor for keyboard status
    /// </summary>
    [CustomEditor(typeof(KeyboardStatus))]
    [CanEditMultipleObjects]
    public class KeyboardStatusEditor: Editor {

        #region GUI_STRINGS
        private static GUIContent OUTPUT = new GUIContent("Gameobject Output", "field receiving input from the keyboard (Text,InputField,TextMeshPro)");
        private static GUIContent OUTPUT_LENGTH = new GUIContent("Output Length", "Maximum output text length");
        private const string OUTPUT_TYPE = "Choose Output Script Type";
        #endregion


        private const string TEXT = "text";

        private KeyboardStatus keybaordStatus;
        private Component[] componentsWithText;
        private string[] scriptsNames;

        private bool notNulltargetAndChanged;
        private int currentSelected = 0;
        private int previousSelected = 0;
        private ErrorReporter errorRaporter;
       
        private void Awake () {
            keybaordStatus = target as KeyboardStatus;
            ClearReflectionData();

            if(keybaordStatus.targetGameobject != null) {
                GetComponentsName();
            }
        }

        /// <summary>
        /// Recovers Components having parameter called "text" attached to target
        /// gameobject. Later it changes them to array of string used in popup
        /// </summary>
        private void GetComponentsName () {
            componentsWithText = keybaordStatus.targetGameobject.GetComponents<Component>()
                .Where(x => x.GetType().GetProperty(TEXT) != null).ToArray();
            scriptsNames = componentsWithText.Select(x => x.GetType().ToString()).ToArray<String>();
            currentSelected = 0;
            notNulltargetAndChanged = true;
        }

        public override void OnInspectorGUI () {
            keybaordStatus = target as KeyboardStatus;
            keybaordStatus.maxOutputLength = EditorGUILayout.IntField(OUTPUT_LENGTH, keybaordStatus.maxOutputLength);            
            DrawTargetGameobjectField();
            DrawPopupList();
            keybaordStatus.isReflectionPossible = IsReflectionPossible();
            HandleValuesChanges();
        }

        private void DrawTargetGameobjectField () {
            EditorGUI.BeginChangeCheck();
            keybaordStatus.targetGameobject = EditorGUILayout.ObjectField(OUTPUT, keybaordStatus.targetGameobject, typeof(GameObject), true) as GameObject;
            if(keybaordStatus.targetGameobject != null && EditorGUI.EndChangeCheck()) { //if not null and changed this frame
                GetComponentsName();
            }
            if(keybaordStatus.targetGameobject == null && EditorGUI.EndChangeCheck()) {// if set to null on this frame
                ClearReflectionData();
            }
        }

        private void DrawPopupList () {
            GUI.enabled = IsReflectionPossible();
            currentSelected = EditorGUILayout.Popup(OUTPUT_TYPE, currentSelected, scriptsNames);

            if(previousSelected != currentSelected) {//if popup value was changed
                notNulltargetAndChanged = true;
            }
            previousSelected = currentSelected;

            if(IsReflectionPossible() && notNulltargetAndChanged) { //if reflection is posiible and popup value was changed this frame
                GetTextParameterViaReflection();
            }

        }

        private void GetTextParameterViaReflection () {
            notNulltargetAndChanged = false;
            keybaordStatus.typeHolder = componentsWithText[currentSelected];
            keybaordStatus.targetGameobject = componentsWithText[currentSelected].gameObject;
            keybaordStatus.output = (string)componentsWithText[currentSelected]
                .GetType().GetProperty(TEXT).GetValue(componentsWithText[currentSelected], null);
        }

        

        private void ClearReflectionData () {
            componentsWithText = new Component[0];
            scriptsNames = new string[0];
        }

        public bool IsReflectionPossible () {
            return keybaordStatus.targetGameobject != null && componentsWithText.Length > 0;
        }

        private void HandleValuesChanges () {
            if(GUI.changed) {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                EditorUtility.SetDirty(keybaordStatus);
            }
        }
    }

}