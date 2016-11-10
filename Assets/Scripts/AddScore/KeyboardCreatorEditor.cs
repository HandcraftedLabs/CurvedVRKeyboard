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
    private bool noCameraFound = false;


    public void Awake () {
        keyboard = target as KeyboardCreator;
        keyboard.CheckExistance();
        keyboard.transform.position = Vector3.zero;
        if(keyboard.PivotTransform != null) {
            keyboard.ManageKeys();
        }
    }


    public override void OnInspectorGUI () {
        keyboard.PivotTransform = EditorGUILayout.ObjectField(PIVOT, keyboard.PivotTransform, typeof(Transform), true) as Transform;
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


    private void DrawMemebers () {
        keyboard.Radious = EditorGUILayout.FloatField(DISTANCE, keyboard.Radious);
        keyboard.SpacingBetweenKeys = EditorGUILayout.FloatField(COLUM_NDISTANCE, keyboard.SpacingBetweenKeys);
        keyboard.RowSpacing = EditorGUILayout.FloatField(KEY_SPACE_ROWS, keyboard.RowSpacing);
        keyboard.Rotation = EditorGUILayout.FloatField(ROTATION, keyboard.Rotation);
        keyboard.Flat = EditorGUILayout.Toggle(FLAT, keyboard.Flat);

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
