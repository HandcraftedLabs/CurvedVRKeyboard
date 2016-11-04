using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class KeyboardCreator: KeyboardComponent {


    //-----------SET IN UNITY --------------

    public float r;
    public float yPos;
    public float degreesStep;
    public float rowHeight;
    public float rotation;
    public float spaceXRotation; // Specialy for space (increase to rotate it around x Axis)
    public bool flat;
    public bool buildInEdition;
    public Camera cam;
    public GameObject keyboard;
    //-----------SET IN UNITY --------------


    private KeyboardItem[] keys;
    private float row;
    private static string name;


    private static bool wasDisabled = false;
    static GameObject gameobjectCopy;

    public void Start () {
        CheckExistance();
        keys = gameobjectCopy.GetComponentsInChildren<KeyboardItem>();
        FillKeysWithLetters();
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

    private void FillKeysWithLetters () {
        for(int i = 0;i < keys.Length;i++) {
            keys[i].Init();
            keys[i].letter.text = allLetters[i];
            PositionSingleLetter(i, keys[i].gameObject.transform);
        }
    }

    private void PositionSingleLetter ( int iteration, Transform child ) {
        //check row and how many keys were palced
        float keysPlaced = CalculateKeysPlacedAndRow(iteration);
        child.position = CalculateVector(rowLetters[(int)row], iteration - keysPlaced);
        child.LookAt(cam.transform);
    }

    private Vector3 CalculateVector ( float rowSize, float offset ) {
        float degree = Mathf.Deg2Rad * ( rotation + rowSize * ( degreesStep / 2 ) - offset * degreesStep );
        float x = Mathf.Cos(degree) * r;
        float z;
        z = flat ? r : Mathf.Sin(degree) * r;
        return new Vector3(x, yPos - row * rowHeight, z);

    }


    /// <summary>
    /// After getting to new row we need to reset offset relative to iteration
    /// so all keys spawn in front of user
    /// </summary>
    /// <param name="iteration"></param>
    /// <returns></returns>
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




