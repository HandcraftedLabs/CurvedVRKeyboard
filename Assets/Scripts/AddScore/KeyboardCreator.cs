using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class KeyboardCreator: KeyboardComponent {


    //-----------SET IN UNITY --------------
    
    public float radious;
    public float yPos;
    public float degreesStep;
    public float rowHeight;
    public float rotation;
    public bool flat;
    public bool buildInEditorMode;
    public Camera cam;
    public int maxLetters;
    //-----------SET IN UNITY --------------


    private KeyboardItem[] keys;
    private float row;
    private static string name;


    private static bool wasDisabled = false;
    static GameObject gameobjectCopy;

    public void Start () {
        CheckExistance();
        keys = gameobjectCopy.GetComponentsInChildren<KeyboardItem>();
        FillAndPlaceKeys();
        KeyboardRayCaster rayCaster = gameobjectCopy.GetComponent<KeyboardRayCaster>();
        rayCaster.SetRayLength(radious+1f);
        rayCaster.SetCamera(cam);
        KeyboardStatus status = gameobjectCopy.GetComponent<KeyboardStatus>();
        status.SetKeys(keys);
        status.SetMaxLetters(maxLetters);
    }

    //gameobject can be destroyed when we are stoping gameplay
    //(we are using gameobject out of gameplay also to manupulate it on scene)
    //since the gameobject can't be set by "gameobject ==" we need a copy
    //of gameobject. When gameobject is destroied we assign new copy
    //if somone would refer to gameobject error would be thrown
    private void CheckExistance () {
        if(wasDisabled) {
            wasDisabled = false;
            gameobjectCopy = GameObject.Find(name);
        }
        //gameobject name changed or no name asigned yet
        if(name == null || !name.Equals(gameobjectCopy.name)) { 
            name = gameObject.name;
            gameobjectCopy = gameObject;
        }
    }

    private void FillAndPlaceKeys () {
        for(int i = 0;i < keys.Length;i++) {
            keys[i].Init();
            keys[i].letter.text = allLetters[i];
            PositionSingleLetter(i, keys[i].gameObject.transform);
        }
    }

    private void PositionSingleLetter ( int iteration, Transform keyTrnsform ) {
        //check row and how many keys were palced
        float keysPlaced = CalculateKeysPlacedAndRow(iteration);
        keyTrnsform.position = CalculatePosition(rowLetters[(int)row], iteration - keysPlaced);
        keyTrnsform.LookAt(cam.transform);
        
    }

    
    private Vector3 CalculatePosition ( float rowSize, float offset ) {
        float degree = Mathf.Deg2Rad * ( rotation + rowSize * ( degreesStep / 2 ) - offset * degreesStep );
        float x = Mathf.Cos(degree) * radious;
        float z;
        z = flat ? radious : Mathf.Sin(degree) * radious;
        return new Vector3(x, yPos - row * rowHeight, z);
    }



    // After getting to new row we need to reset offset relative to iteration
    // so all keys spawn in front of user
    private float CalculateKeysPlacedAndRow ( int iteration ) {
        float keysPlaced = 0;
        if(iteration < rowLetters[0]) {//if is in firstrow
            keysPlaced = 0;
            row = 0;
        } else if(iteration < rowLetters[0] + rowLetters[1]) {//if is secondrow
            keysPlaced = rowLetters[0];
            row = 1;
        } else if(iteration < rowLetters[0] + rowLetters[1] + rowLetters[2]) {//thirdrow
            keysPlaced = rowLetters[0] + rowLetters[1];
            row = 2;
        }
        //now are special signs they need to be set manually (their offset) 
        else if(iteration == rowLetters[0] + rowLetters[1] + rowLetters[2]) {//?!#
            keysPlaced = rowLetters[0] + rowLetters[1] + rowLetters[2];
        } else if(iteration == rowLetters[0] + rowLetters[1] + rowLetters[2] + 1) {//space
            keysPlaced = rowLetters[0] + rowLetters[1] + rowLetters[2] - 1.5f;
        } else if(iteration == rowLetters[0] + rowLetters[1] + rowLetters[2] + 2) { // backspace
            keysPlaced = rowLetters[0] + rowLetters[1] + rowLetters[2] - 3f;
        }
        //set third row for special keys
        if(iteration >= rowLetters[0] + rowLetters[1] + rowLetters[2]) {
            row = 3;
        }
        return keysPlaced;
    }

    public void OnDisable () {
        wasDisabled = true;
    }
}




