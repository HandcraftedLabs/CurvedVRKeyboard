using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
/// <summary>
/// Special inspector for Keybaord
/// </summary>
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
        keyboardCreator.InitKeys();
        if(keyboardCreator.RaycastingCamera != null) {
            keyboardCreator.ManageKeys();
        }
        keyboardScale = keyboardCreator.transform.localScale;
    }


    public override void OnInspectorGUI () {
        keyboardCreator.RaycastingCamera = EditorGUILayout.ObjectField(CAMERA, keyboardCreator.RaycastingCamera, typeof(Camera), true) as Camera;

        keyboardCreator.checkErrors();

  

        HandleScaleChange();
        if(keyboardCreator.RaycastingCamera != null) {// If there is a camera
            DrawMemebers();
        } else {
            CameraFinderGui();
        }
        if(ErrorReporter.Instance.IsComunicatAviable()) {
            Color standard = GUI.color;
            GUI.color = ( ErrorReporter.Instance.IsErrorPresent() ) ? Color.red : Color.yellow;
            GUIStyle s = new GUIStyle(EditorStyles.textField);
            s.normal.textColor = Color.black;
            EditorGUILayout.LabelField(ErrorReporter.Instance.GetMessage(), s);
        }

        if(GUI.changed) {
            EditorUtility.SetDirty(keyboardCreator);
        }


    }
    /// <summary>
    /// Checks if gameobject (whole keyboard) scale was changed
    /// </summary>
    private void HandleScaleChange () {
        float neededXScale = float.NaN;
        if(keyboardCreator.transform.localScale.x != keyboardScale.x) { // X scale changed
            neededXScale = keyboardCreator.transform.localScale.x;
        } else if(keyboardCreator.transform.localScale.z != keyboardScale.z) { // Z scale changed
            neededXScale = keyboardCreator.transform.localScale.z;
        }

        if(!float.IsNaN(neededXScale)) {// If change was made
            ChangeScale(neededXScale, keyboardCreator.transform.localScale.y);
        }

            
    }
    /// <summary>
    /// Keeps x and z scale bound together. Resizes keybaord
    /// </summary>
    /// <param name="horiziontalScale"> scale in x or z </param>
    /// <param name="y">scale in y</param>
    private void ChangeScale ( float horiziontalScale, float y ) {
        keyboardScale.x = keyboardScale.z = horiziontalScale;
        keyboardScale.y = y;
        keyboardCreator.transform.localScale = keyboardScale;
    }
    /// <summary>
    /// Draw all fields in inspector (if camera is set)
    /// </summary>
    private void DrawMemebers () {
        // Value of curvature is always between [0,1]
        float curvatureValue = EditorGUILayout.IntSlider(new GUIContent(CURVATURE + " (%)"), (int)( keyboardCreator.Curvature * 100.0f ), 0, 100);
        float clamped = Mathf.Clamp01((float)curvatureValue / 100.0f);
        keyboardCreator.Curvature = clamped;
        keyboardCreator.ClickHandle = EditorGUILayout.TextField(CLICK_INPUT_COMMAND, keyboardCreator.ClickHandle);
        keyboardCreator.KeyDefaultMaterial = EditorGUILayout.ObjectField(DEFAULT_MATERIAL, keyboardCreator.KeyDefaultMaterial, typeof(Material), true) as Material;
        keyboardCreator.KeyHoveringMaterial = EditorGUILayout.ObjectField(HOVERING_MATERIAL, keyboardCreator.KeyHoveringMaterial, typeof(Material), true) as Material;
        keyboardCreator.KeyPressedMaterial = EditorGUILayout.ObjectField(CLICKED_MATERIAL, keyboardCreator.KeyPressedMaterial, typeof(Material), true) as Material;
    }

    /// <summary>
    /// Draws camera find button
    /// </summary>
    private void CameraFinderGui () {
        bool clicked = GUILayout.Button(FIND_CAMERA);
        if(clicked)
            SearchForCamera();
       
        if(noCameraFound) { // If after button press there is no camera
            GUILayout.Label(NO_CAMERA_ERROR);
        }
    }

    /// <summary>
    /// Searches for available camera on scene
    /// </summary>
    private void SearchForCamera () {
        if(Camera.allCameras.Length != 0) {//If there is camera on scene
            noCameraFound = false;
            keyboardCreator.RaycastingCamera = Camera.allCameras[0];
        }else {
            noCameraFound = true;
        }  
    }




}
#endif