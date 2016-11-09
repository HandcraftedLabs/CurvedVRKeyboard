using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
class KeyboardCreator: KeyboardComponent {


    //-----------SET IN UNITY --------------
    [SerializeField]
    private float radious;
    [SerializeField]
    private float spacingBetweenKeys;
    [SerializeField]
    private float rowSpacing;
    [SerializeField]
    private float rotation;

    [SerializeField]
    private bool flat;
    [SerializeField]
    private bool buildInEditorMode;
    [SerializeField]
    private GameObject pivotObject;
    [SerializeField]
    private string clickHandle;
    [SerializeField]
    private float spaceKeyOffsetRotation;

    [SerializeField]
    private Material keyDefaultMaterial;
    [SerializeField]
    private Material keyHoldMaterial;
    [SerializeField]
    private Material keyPressedMaterial;


    private bool wasChanged = false;
    //-----------SET IN UNITY --------------


    private KeyboardItem[] keys;
    private float row;
    private static string name;

    private static bool wasDisabled = false;
    static GameObject gameobjectCopy;


    public void Start () {
        CheckExistance();
        ManageKeys();
        ChangeMaterialOnKeys();
    }

    public void BuildInEditor () {
        if(wasChanged) {
            wasChanged = false;
            CheckExistance();
        }

    }

    private void ManageKeys () {
        if(keys == null) {
            keys = gameobjectCopy.GetComponentsInChildren<KeyboardItem>();
        }
        FillAndPlaceKeys();
    }


    private void SetComponents () {
        KeyboardRayCaster rayCaster = gameobjectCopy.GetComponent<KeyboardRayCaster>();
        rayCaster.SetRayLength(Radious + 1f);
        rayCaster.SetCamera(PivotObject);
        rayCaster.SetClickButton(ClickHandle);
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
            SetComponents();
        }
        //gameobject name changed or no name asigned yet
        if(gameobjectCopy == null || name == null || !name.Equals(gameobjectCopy.name)) {
            gameobjectCopy = gameObject;
            name = gameObject.name;
        }
    }

    private void FillAndPlaceKeys () {
        for(int i = 0;i < keys.Length;i++) {
            keys[i].Init();
            keys[i].setLetterText(allLetters[i]);
            PositionSingleLetter(i, keys[i].gameObject.transform);
       }
    }

    private void PositionSingleLetter ( int iteration, Transform keyTrnsform ) {
        //check row and how many keys were palced
        float keysPlaced = CalculateKeysPlacedAndRow(iteration);
        keyTrnsform.position = Flat?
            CalculatePositionFlat(rowLetters[(int)row], iteration - keysPlaced):
            CalculatePositionCirlce(rowLetters[(int)row], iteration - keysPlaced);

        keyTrnsform.position += PivotObject.transform.position;
        if(!Flat) {
            //since space is kind of bigger than any other key and keys 
            //rotate acording to it center rotation of this key can be 
            //a bit more than any other so there is option to set it manualy 
            if(keyTrnsform.gameObject.GetComponent<KeyboardItem>().getValue().Equals(SPACE)) {
                keyTrnsform.LookAt(PivotObject.transform.position);
            }else {
                keyTrnsform.LookAt(PivotObject.transform);
            }
            
        }
        else {
            keyTrnsform.RotateAround(PivotObject.transform.position, Vector3.up, -Rotation + 90);
            keyTrnsform.eulerAngles = new Vector3(0, -Rotation -90, 0);
        }


    }



    private Vector3 CalculatePositionFlat( float rowSize, float offset ) {
        float degree = Mathf.Deg2Rad * ( 90 + rowSize * ( SpacingBetweenKeys / 2 ) - offset * SpacingBetweenKeys );
        float x = Mathf.Cos(degree) * Radious;
        float z;
        z = Radious;
        return new Vector3(x, 0f - row * RowSpacing, z);
    }

    private Vector3 CalculatePositionCirlce ( float rowSize, float offset ) {
        float degree = Mathf.Deg2Rad * ( Rotation + rowSize * ( SpacingBetweenKeys / 2 ) - offset * SpacingBetweenKeys );
        float x = Mathf.Cos(degree) * Radious;
        float z;
        z =  Mathf.Sin(degree) * Radious;
        return new Vector3(x, 0f - row * RowSpacing, z);
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


    private void ChangeMaterialOnKeys () {
        foreach (KeyboardItem key in keys) {
            key.setMaterials(KeyDefaultMaterial, KeyHoldMaterial, KeyPressedMaterial);
        }
    }






    //---------------PROPERTIES----------------
    public float Radious {
        get {
            return radious;
        }
        set {
            if(radious != value) {
                radious = value;
                wasChanged = true;
                ManageKeys();
            }

        }
    }

    public float SpacingBetweenKeys {
        get {
            return spacingBetweenKeys;
        }
        set {
            if(spacingBetweenKeys != value) {
                wasChanged = true;
                spacingBetweenKeys = value;
                ManageKeys();
            }

        }
    }

    public float RowSpacing {
        get {
            return rowSpacing;
        }
        set {
            if(RowSpacing != value) {
                rowSpacing = value;
                wasChanged = true;
                ManageKeys();
            }
                
        }
    }

    public float Rotation {
        get {
            return rotation;
        }
        set {
            if(rotation != value) {
                rotation = value;
                wasChanged = true;
                ManageKeys();
            }
                
        }
    }

    public bool Flat {
        get {
            return flat;
        }
        set {
            if(flat != value) {
                flat = value;
                wasChanged = true;
                ManageKeys();
            }
                
        }
    }

    public bool BuildInEditorMode {
        get {
            return buildInEditorMode;
        }

        set {
            if(buildInEditorMode != value) {
                buildInEditorMode = value;
                wasChanged = true;
            }
                
        }
    }

    public float SpaceKeyOffsetRotation {
        get {
            return spaceKeyOffsetRotation;
        }

        set {
            if(SpaceKeyOffsetRotation != value) {
                spaceKeyOffsetRotation = value;
                wasChanged = true;
                ManageKeys();
            }
                
        }
    }

    public Material KeyDefaultMaterial {
        get {
            return keyDefaultMaterial;
        }

        set {
            if(KeyDefaultMaterial != value) {
                keyDefaultMaterial = value;
                ChangeMaterialOnKeys();
            }
                
        }
    }

    public Material KeyHoldMaterial {
        get {
            return keyHoldMaterial;
        }

        set {
            if(keyHoldMaterial != value) {
                keyHoldMaterial = value;
                ChangeMaterialOnKeys();
            }
                
        }
    }

    public Material KeyPressedMaterial {
        get {
            return keyPressedMaterial;
        }

        set {
            if(KeyPressedMaterial != value) {
                keyPressedMaterial = value;
                ChangeMaterialOnKeys();
            }
             
        }
    }



    public GameObject PivotObject {
        get {
            return pivotObject;
        }

        set {
            if (pivotObject != value) {
                pivotObject = value;
                wasChanged = true;
            }
                
        }
    }

    public string ClickHandle {
        get {
            return clickHandle;
        }

        set {
            clickHandle =  value ;
        }
    }
}




