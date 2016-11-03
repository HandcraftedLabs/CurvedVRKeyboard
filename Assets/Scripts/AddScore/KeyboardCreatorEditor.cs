using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(KeyboardCreator))]
[CanEditMultipleObjects]
public class KeyboardCreatorEditor: Editor {


   private void CallbackFucntion () {
     //   Debug.Log("equwet");
    } 

    void OnEnable () {
        EditorApplication.update += CallbackFucntion;
    }
    
    void onDisable () {
        EditorApplication.update -= CallbackFucntion;
    }

}
