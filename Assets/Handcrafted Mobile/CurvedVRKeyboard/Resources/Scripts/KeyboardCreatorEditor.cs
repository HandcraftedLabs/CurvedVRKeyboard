using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
namespace CurvedVRKeyboard {

    /// <summary>
    /// Special inspector for Keybaord
    /// </summary>
    [CustomEditor(typeof(KeyboardCreator))]
    [CanEditMultipleObjects]
    public class KeyboardCreatorEditor: Editor {

        private readonly string CURVATURE = "Curvature";
        private readonly string RAYCASTING_SOURCE = "Raycasting source";
        private readonly string CLICK_INPUT_COMMAND = "Click input name";
        private readonly string DEFAULT_MATERIAL = "Normal material";
        private readonly string SELECTED_MATERIAL = "Selected material";
        private readonly string PRESSED_MATERIAL = "Pressed material";
        private readonly string FIND_SOURCE = "Raycasting source missing. Press to set default camera";
        private readonly string NO_CAMERA_ERROR = "Camera was not found. Add a camera to scene";

        private KeyboardCreator keyboardCreator;
        private ErrorReporter errorReporter;
        private Vector3 keyboardScale;
        private GUIStyle style;
        private bool noCameraFound = false;




        private void Awake () {
            keyboardCreator = target as KeyboardCreator;
            keyboardCreator.InitKeys();
            style = new GUIStyle(EditorStyles.textField);
            if(keyboardCreator.RaycastingSource != null) {
                keyboardCreator.ManageKeys();
            }
            keyboardScale = keyboardCreator.transform.localScale;
        }

        public override void OnInspectorGUI () {
            errorReporter = ErrorReporter.Instance;
            keyboardCreator.checkErrors();
            keyboardCreator.RaycastingSource = EditorGUILayout.ObjectField(RAYCASTING_SOURCE, keyboardCreator.RaycastingSource, typeof(Transform), true) as Transform;
            HandleScaleChange();

            if(keyboardCreator.RaycastingSource != null) {// If there is a raycast source
                DrawMemebers();
                NotifyErrors();
            } else {
                CameraFinderGui();
            }

            if(GUI.changed) {
                EditorUtility.SetDirty(keyboardCreator);
            }
        }

        /// <summary>
        /// Handles label with error.
        /// </summary>
        private void NotifyErrors () {
            if(errorReporter.ShouldMessageBeDisplayed()) {
                GUI.color = ( errorReporter.IsErrorPresent() ) ? Color.red : Color.yellow;
                EditorGUILayout.LabelField(errorReporter.GetMessage(), style);
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
        /// Draw all fields in inspector (if raycast source is set)
        /// </summary>
        private void DrawMemebers () {
            // Value of curvature is always between [0,1]
            float curvatureValue = EditorGUILayout.IntSlider(new GUIContent(CURVATURE + " (%)"), (int)( keyboardCreator.Curvature * 100.0f ), 0, 100);
            float clamped = Mathf.Clamp01((float)curvatureValue / 100.0f);
            keyboardCreator.Curvature = clamped;
            keyboardCreator.ClickHandle = EditorGUILayout.TextField(CLICK_INPUT_COMMAND, keyboardCreator.ClickHandle);
            keyboardCreator.KeyDefaultMaterial = EditorGUILayout.ObjectField(DEFAULT_MATERIAL, keyboardCreator.KeyDefaultMaterial, typeof(Material), true) as Material;
            keyboardCreator.KeyHoveringMaterial = EditorGUILayout.ObjectField(SELECTED_MATERIAL, keyboardCreator.KeyHoveringMaterial, typeof(Material), true) as Material;
            keyboardCreator.KeyPressedMaterial = EditorGUILayout.ObjectField(PRESSED_MATERIAL, keyboardCreator.KeyPressedMaterial, typeof(Material), true) as Material;
        }

        /// <summary>
        /// Draws camera find button
        /// </summary>
        private void CameraFinderGui () {
            bool clicked = GUILayout.Button(FIND_SOURCE);
            if(clicked) {
                SearchForCamera();
            }

            if(noCameraFound) { // After button press there is no camera
                GUILayout.Label(NO_CAMERA_ERROR);
            }
        }

        /// <summary>
        /// Searches for available camera on scene
        /// </summary>
        private void SearchForCamera () {
            if(Camera.allCameras.Length != 0) {//If there is camera on scene
                noCameraFound = false;
                keyboardCreator.RaycastingSource = Camera.allCameras[0].transform;
            } else {
                noCameraFound = true;
            }
        }
    }
}
#endif