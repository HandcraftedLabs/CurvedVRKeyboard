using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class KeyboardCreator: KeyboardComponent {


    //-----------SET IN UNITY --------------
    [Tooltip("Distance from camera")]
    public float radious;
    [Tooltip("spaces between keys in degrees horizontal")]
    public float spacingBetweenKeys;
    [Tooltip("distance between keys vertical")]
    public float rowSpacing;
    [Tooltip("rotates keybaord around camera as whole component")]
    public float rotation;
    [Tooltip("moves keyboard verticaly")]
    public float yPos;
    [Tooltip("shall keyboard be flat or rounded?")]
    public bool flat;
    [Tooltip("shall changes be calculated in editor mode \n IMPORTANT TURN THIS OFF IF YOU ARE FINISHED WITH KEYBOARD IT POSITION ALL TIME")]
    public bool buildInEditorMode;
    [Tooltip("Camera object around which keybaord will orbit")]
    public Camera cam;
 


    public float spaceKeyOfsetRotation;


    public Material keyDefaultMaterial;
    public Material keyHoldMaterial;
    public Material keyPressedMaterial;
    //-----------SET IN UNITY --------------


    private KeyboardItem[] keys;
    private float row;
    private static string name;
    private Vector3 spaceOffset;

    private static bool wasDisabled = false;
    static GameObject gameobjectCopy;

    public void Start () {
        CheckExistance();
        keys = gameobjectCopy.GetComponentsInChildren<KeyboardItem>();
        spaceOffset = new Vector3(0, spaceKeyOfsetRotation, 0);
        FillAndPlaceKeys();
        SetComponents();
    }

    private void SetComponents () {
        KeyboardRayCaster rayCaster = gameobjectCopy.GetComponent<KeyboardRayCaster>();
        rayCaster.SetRayLength(radious + 1f);
        rayCaster.SetCamera(cam);
        KeyboardStatus status = gameobjectCopy.GetComponent<KeyboardStatus>();
        status.SetKeys(keys);
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
            keys[i].setMaterials(keyDefaultMaterial, keyHoldMaterial, keyPressedMaterial);
            keys[i].setLetterText(allLetters[i]);
            PositionSingleLetter(i, keys[i].gameObject.transform);
       }
    }

    private void PositionSingleLetter ( int iteration, Transform keyTrnsform ) {
        //check row and how many keys were palced
        float keysPlaced = CalculateKeysPlacedAndRow(iteration);
        keyTrnsform.position = flat?
            CalculatePositionFlat(rowLetters[(int)row], iteration - keysPlaced):
            CalculatePositionCirlce(rowLetters[(int)row], iteration - keysPlaced);
        if(!flat) {
            //since space is kind of bigger than any other key and keys 
            //rotate acording to it center rotation of this key can be 
            //a bit more than any other so there is option to set it manualy 
            if(keyTrnsform.gameObject.GetComponent<KeyboardItem>().getValue().Equals(SPACE)) {
                keyTrnsform.LookAt(cam.transform.position + spaceOffset);
            }else {
                keyTrnsform.LookAt(cam.transform);
            }
            
        }
        else {
            keyTrnsform.RotateAround(cam.transform.position, Vector3.up, -rotation + 90);
            keyTrnsform.eulerAngles = new Vector3(0, -rotation -90, 0);
        }


    }

    private Vector3 CalculatePositionFlat( float rowSize, float offset ) {
        float degree = Mathf.Deg2Rad * ( 90 + rowSize * ( spacingBetweenKeys / 2 ) - offset * spacingBetweenKeys );
        float x = Mathf.Cos(degree) * radious;
        float z;
        z = radious;
        return new Vector3(x, yPos - row * rowSpacing, z);
    }

    private Vector3 CalculatePositionCirlce ( float rowSize, float offset ) {
        float degree = Mathf.Deg2Rad * ( rotation + rowSize * ( spacingBetweenKeys / 2 ) - offset * spacingBetweenKeys );
        float x = Mathf.Cos(degree) * radious;
        float z;
        z =  Mathf.Sin(degree) * radious;
        return new Vector3(x, yPos - row * rowSpacing, z);
    }



    // After getting to new row we need to reset offset relative to iteration
    // so all keys spawn in front of user centered
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




