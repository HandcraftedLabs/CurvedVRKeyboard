using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WiEditorTest: EditorWindow {

    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;


    [MenuItem("Window/My Window")]

    public static void ShowWindow () {
        EditorWindow.GetWindow(typeof(WiEditorTest));
    }

    void OnGUI () {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }

    public void OnInspectorUpdate () {
        Debug.Log("inspectorUpdate");
    }

    public void Awake () {
        Debug.Log("awake");
    }
    public void OnProjectChange () {
        Debug.Log("change");
    }

    //public  void OnProjectChange () {
    //    Object q = Selection.activeObject;
    //    Debug.Log("qwewe");
    //}

}
