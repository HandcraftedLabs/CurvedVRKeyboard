using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(KeyboardCreator))]
[CanEditMultipleObjects]
public class KeyboardCreatorEditor: Editor {

   public bool reload = false; 

   private void CallbackFucntion () {
        KeyboardCreator mTarget = target as KeyboardCreator;
        if(mTarget.buildInEditorMode) {
            mTarget.Start();
        }
    } 

    void OnEnable () {
        EditorApplication.update += CallbackFucntion;
    }
    
    void onDisable () {
        EditorApplication.update -= CallbackFucntion;
    }


}
