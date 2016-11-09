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


 



    public override void OnInspectorGUI () {
        KeyboardCreator keybaord = target as KeyboardCreator;

        keybaord.Radious = EditorGUILayout.FloatField(DISTANCE, keybaord.Radious);
        keybaord.SpacingBetweenKeys = EditorGUILayout.FloatField(COLUM_NDISTANCE, keybaord.SpacingBetweenKeys);
        keybaord.RowSpacing = EditorGUILayout.FloatField(KEY_SPACE_ROWS, keybaord.RowSpacing);
        keybaord.Rotation = EditorGUILayout.FloatField(ROTATION, keybaord.Rotation);
        keybaord.Flat = EditorGUILayout.Toggle(FLAT, keybaord.Flat);
        
        keybaord.PivotObject = EditorGUILayout.ObjectField(PIVOT, keybaord.PivotObject, typeof(GameObject), true) as GameObject;

        if(keybaord.PivotObject != null) {
            keybaord.BuildInEditorMode = EditorGUILayout.Toggle(TEMPORARY_IN_EDIOTR_MODE, keybaord.BuildInEditorMode);
        }else {
            keybaord.BuildInEditorMode = false;
        }
                    
        keybaord.ClickHandle = EditorGUILayout.TextField(CLICKINPUTCOMMAND, keybaord.ClickHandle);
        keybaord.KeyDefaultMaterial = EditorGUILayout.ObjectField(MATERIAL_DEFAULT, keybaord.KeyDefaultMaterial, typeof(Material), true) as Material;
        keybaord.KeyHoldMaterial = EditorGUILayout.ObjectField(MATERIAL_HOLD, keybaord.KeyHoldMaterial, typeof(Material), true) as Material;
        keybaord.KeyPressedMaterial = EditorGUILayout.ObjectField(MATERIAL_CLICKED, keybaord.KeyPressedMaterial, typeof(Material), true) as Material;

        if(GUI.changed)
            EditorUtility.SetDirty(keybaord);
    }




}
