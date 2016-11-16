using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(KeyboardCreator))]
[CanEditMultipleObjects]
public class KeyboardCreatorEditor: Editor {

    private readonly string CURVATURE = "Curvature";
    private readonly string CAMERA = "Camera";
    private readonly string CLICKINPUTCOMMAND = "Input command";
    private readonly string MATERIAL_DEFAULT = "Material default";
    private readonly string MATERIAL_HOLD = "Material hold";
    private readonly string MATERIAL_CLICKED = "Material clicked";
    private readonly string FIND_CAMERA = "No camera found press to find";
    private readonly string NO_CAMERA_ERROR = "Camera wasn'f found. Add camera to scene";

    private KeyboardCreator keyboard;
    private Vector3 keyboardScale;
    private bool noCameraFound = false;


    private float xScalePrevious;
    private float zScalePrevious;


    public void Awake () {
        keyboard = target as KeyboardCreator;
        keyboard.CheckExistance();
        if(keyboard.RaycastingCamera != null) {
            keyboard.ManageKeys();
        }
        keyboardScale = keyboard.transform.localScale;
    }


    public override void OnInspectorGUI () {
        keyboard.CheckExistance();
        keyboard.RaycastingCamera = EditorGUILayout.ObjectField(CAMERA, keyboard.RaycastingCamera, typeof(Transform), true) as Camera;
        HandleScaleChange();
        // if there is a pivot object users are allowed to modify values
        if(keyboard.RaycastingCamera != null) {
            DrawMemebers();
        } else {
            CameraFinderGui();
        }
        if(GUI.changed) {
            EditorUtility.SetDirty(keyboard);
        }
    }

    private void HandleScaleChange () {
        if(keyboard.transform.hasChanged) {
            //x scale changed
            if(keyboard.transform.localScale.x != keyboardScale.x) {
                keyboardScale.x = keyboard.transform.localScale.x;
                keyboardScale.z = keyboard.transform.localScale.x;
            //z scale changed
            } else if(keyboard.transform.localScale.z != keyboardScale.z) {
                keyboardScale.z = keyboard.transform.localScale.z;
                keyboardScale.x = keyboard.transform.localScale.z;
            }
            keyboardScale.y = keyboard.transform.localScale.y;
            keyboard.transform.localScale = keyboardScale;
        }
    }

    private void DrawMemebers () {
        keyboard.Curvature = EditorGUILayout.FloatField(CURVATURE, keyboard.Curvature);

        keyboard.ClickHandle = EditorGUILayout.TextField(CLICKINPUTCOMMAND, keyboard.ClickHandle);
        keyboard.KeyDefaultMaterial = EditorGUILayout.ObjectField(MATERIAL_DEFAULT, keyboard.KeyDefaultMaterial, typeof(Material), true) as Material;
        keyboard.KeyHoldMaterial = EditorGUILayout.ObjectField(MATERIAL_HOLD, keyboard.KeyHoldMaterial, typeof(Material), true) as Material;
        keyboard.KeyPressedMaterial = EditorGUILayout.ObjectField(MATERIAL_CLICKED, keyboard.KeyPressedMaterial, typeof(Material), true) as Material;
    }


    private void CameraFinderGui () {
        if(GUILayout.Button(FIND_CAMERA)) {
            SearchForCamera();
        }
        //if after button press there is no camera show warning
        if(noCameraFound) {
            GUILayout.Label(NO_CAMERA_ERROR);
        }
    }


    private void SearchForCamera () {
        //if there is camera
        if(Camera.allCameras.Length != 0) {
            noCameraFound = false;
            keyboard.RaycastingCamera = Camera.allCameras[0];
        }else {
            noCameraFound = true;
        }  
    }




}
