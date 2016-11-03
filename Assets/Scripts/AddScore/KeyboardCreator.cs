using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class KeyboardCreator: KeyboardComponent {


    //-----------SET IN UNITY --------------
    public KeyboardItem[] keys;
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
    


    private float row;

    public void Start () {
        //.Cast<Transform>().OrderBy(t => t.name)
        int i = 0;
        foreach(KeyboardItem keyItem in keys) {
            if(i < allLetters.Length) {
                keyItem.Init();
                keyItem.letter.text = allLetters[i];
                
            }
            PositionSingleLetter(i, keyItem.gameObject.transform);
            i++;
        }
    }

    private void PositionSingleLetter ( int iteration,Transform child) {
        //check row and how many keys were palced
        float keysPlaced = CalculateKeysPlacedAndRow(iteration);
       
 
        child.position = CalculateVector(rowLetters[(int)row], iteration - keysPlaced);
        child.LookAt(cam.transform);
        //if(iteration != rowLetters[0] + rowLetters[1] + rowLetters[2] + 1) {// all expect space
        //    child.LookAt(cam.tra)
        //}else {
        //    child.rotation = MyUtils.RotateToVector(-child.position + cam.transform.position + new Vector3(0, spaceXRotation, 0));
        //}

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

}




