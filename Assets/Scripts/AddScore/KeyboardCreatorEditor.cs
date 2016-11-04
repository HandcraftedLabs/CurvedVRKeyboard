using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(KeyboardCreator))]
[CanEditMultipleObjects]
public class KeyboardCreatorEditor: Editor {

   public bool reload = false; 

   private void UpdateKeyboard () {
        KeyboardCreator creator = target as KeyboardCreator;
        if(creator.buildInEditorMode) {
            creator.Start();
        }
    } 

    void OnEnable () {
        EditorApplication.update += UpdateKeyboard;
    }
    
    void onDisable () {
        EditorApplication.update -= UpdateKeyboard;
    }


}
