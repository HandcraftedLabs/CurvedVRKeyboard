using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(KeyboardCreator))]
[CanEditMultipleObjects]
public class KeyboardCreatorEditor: Editor {

    private readonly string DISTANCE = "Distance";
    private readonly string COLUM_NDISTANCE = "Distance between columns";
    private readonly string KEY_SPACE_ROWS = "Distance between rows";
    private readonly string ROTATION= "Rotation around central point";
    private readonly string FLAT = "Flatten keybaord?";
    private readonly string TEMPORARY_IN_EDIOTR_MODE = "Build at runtime?";
    private readonly string PIVOT = "Pivot object";
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
        if(keyboard.PivotTransform != null) {
            keyboard.ManageKeys();
        }
        keyboardScale = keyboard.transform.localScale;

    }


    public override void OnInspectorGUI () {
        keyboard.CheckExistance();
        keyboard.PivotTransform = EditorGUILayout.ObjectField(PIVOT, keyboard.PivotTransform, typeof(Transform), true) as Transform;
        HandleScaleChange();
        // if there is a pivot object users are allowed to modify values
        if(keyboard.PivotTransform != null) {
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
            if(keyboard.transform.localScale.x != keyboardScale.x) {
                keyboardScale.x = keyboard.transform.localScale.x;
                keyboardScale.z = keyboard.transform.localScale.x;
            } else if(keyboard.transform.localScale.z != keyboardScale.z) {
                keyboardScale.z = keyboard.transform.localScale.z;
                keyboardScale.x = keyboard.transform.localScale.z;
            }
            keyboardScale.y = keyboard.transform.localScale.y;
            keyboard.transform.localScale = keyboardScale;
        }
    }

    private void DrawMemebers () {
        keyboard.Radious = EditorGUILayout.FloatField(DISTANCE, keyboard.Radious);


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
            keyboard.PivotTransform = Camera.allCameras[0].transform;
        }else {
            noCameraFound = true;
        }  
    }




}
