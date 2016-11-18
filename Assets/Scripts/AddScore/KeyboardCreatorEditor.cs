using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(KeyboardCreator))]
[CanEditMultipleObjects]
public class KeyboardCreatorEditor: Editor {

    private readonly string CURVATURE = "Curvature";
    private readonly string CAMERA = "Camera";
    private readonly string CLICK_INPUT_COMMAND = "Click input Name";
    private readonly string DEFAULT_MATERIAL = "Default Material";
    private readonly string HOVERING_MATERIAL = "Hovering  Material";
    private readonly string CLICKED_MATERIAL = "Clicked Material";
    private readonly string FIND_CAMERA = "No camera found. Press to find";
    private readonly string NO_CAMERA_ERROR = "Camera was not found. Add a camera to scene";

    private KeyboardCreator keyboardCreator;
    private Vector3 keyboardScale;
    private bool noCameraFound = false;



    private void Awake () {
        keyboardCreator = target as KeyboardCreator;
        if(keyboardCreator.RaycastingCamera != null) {
            keyboardCreator.ManageKeys();
        }
        keyboardScale = keyboardCreator.transform.localScale;
    }


    public override void OnInspectorGUI () {
        keyboardCreator.RaycastingCamera = EditorGUILayout.ObjectField(CAMERA, keyboardCreator.RaycastingCamera, typeof(Camera), true) as Camera;
        HandleScaleChange();
        // if there is a pivot object users are allowed to modify values
        if(keyboardCreator.RaycastingCamera != null) {
            DrawMemebers();
        } else {
            CameraFinderGui();
        }
        if(GUI.changed) {
            EditorUtility.SetDirty(keyboardCreator);
        }
    }

    private void HandleScaleChange () {
        float neededXScale = float.NaN;
        if(keyboardCreator.transform.localScale.x != keyboardScale.x) { // x scale changed
            neededXScale = keyboardCreator.transform.localScale.x;
        } else if(keyboardCreator.transform.localScale.z != keyboardScale.z) { // z scale changed
            neededXScale = keyboardCreator.transform.localScale.z;
        }

        if(!float.IsNaN(neededXScale)) {
            ChangeScale(neededXScale, keyboardCreator.transform.localScale.y);
        }

            
    }

    private void ChangeScale ( float horiziontalScale, float y ) {
        keyboardScale.x = keyboardScale.z = horiziontalScale;
        keyboardScale.y = y;
        keyboardCreator.transform.localScale = keyboardScale;
    }

    private void DrawMemebers () {
        //so value of curvature is always between [0,1]
        int curvatureValue = EditorGUILayout.IntSlider(new GUIContent(CURVATURE + " (%)"), (int)( keyboardCreator.Curvature * 100.0f ), 0, 100);
        keyboardCreator.Curvature = Mathf.Clamp01((float)curvatureValue / 100.0f);
        keyboardCreator.ClickHandle = EditorGUILayout.TextField(CLICK_INPUT_COMMAND, keyboardCreator.ClickHandle);
        keyboardCreator.KeyDefaultMaterial = EditorGUILayout.ObjectField(DEFAULT_MATERIAL, keyboardCreator.KeyDefaultMaterial, typeof(Material), true) as Material;
        keyboardCreator.KeyHoveringMaterial = EditorGUILayout.ObjectField(HOVERING_MATERIAL, keyboardCreator.KeyHoveringMaterial, typeof(Material), true) as Material;
        keyboardCreator.KeyPressedMaterial = EditorGUILayout.ObjectField(CLICKED_MATERIAL, keyboardCreator.KeyPressedMaterial, typeof(Material), true) as Material;
    }


    private void CameraFinderGui () {
        bool clicked = GUILayout.Button(FIND_CAMERA);
        if(clicked)
            SearchForCamera();
        //if after button press there is no camera show warning
        if(noCameraFound) {
            GUILayout.Label(NO_CAMERA_ERROR);
        }
    }


    private void SearchForCamera () {
        //if there is camera
        if(Camera.allCameras.Length != 0) {
            noCameraFound = false;
            keyboardCreator.RaycastingCamera = Camera.allCameras[0];
        }else {
            noCameraFound = true;
        }  
    }




}
