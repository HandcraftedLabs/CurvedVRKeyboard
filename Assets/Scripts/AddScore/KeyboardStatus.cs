using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardStatus: KeyboardComponent {

    //-----------SET IN UNITY --------------
    public KeyboardItem[] keys;
    public GameObject keyboard;
    public Text name;
    //-----------SET IN UNITY --------------

    private bool latters = true;
    private bool lower = true;

    private static readonly string BLANKSPACE = "_";
    private static readonly int MAXLETTERS = 8;

    private int currentLetter = -1;

    public void HandleClick ( KeyboardItem clicked ) {
        string value = clicked.getValue();

        if(value.Equals(QEH) || value.Equals(ABC)) {//if special signs
            ChangeSpecialLetters();
        } else if(value.Equals(UP) || value.Equals(LOW)) {//if upper/lower
            LowerUpperKeys();
        } else if(value.Equals(SPACE)) {// if space
            TypeKey(' ');
        } else if(value.Equals(BACK)) {// if backspace
            BackspaceKey();
        }
        else {//normalLetter
            TypeKey(value[0]);
        }
    }

    public int GetCurrentLetter () {
        return currentLetter;
    }


    private void ChangeSpecialLetters () {
        latters = !latters;
        int i = 0;
        if(latters) {
            foreach(KeyboardItem keyItem in keys) {
                keyItem.letter.text = allLetters[i];
                i++;
            }
        } else {// special signs
            foreach( KeyboardItem keyItem in keys) {
                keyItem.letter.text = allSpecials[i];
                i++;
            }
        }
    }

    private void LowerUpperKeys () {
        lower = !lower;
        int i = 0;
        if(lower) {
            foreach(KeyboardItem keyItem in keys) {
                keyItem.letter.text = allLetters[i];
                i++;
            }
        } else {// special signs
            foreach(KeyboardItem keyItem in keys) {
                keyItem.letter.text = allLettersUpper[i];
                i++;
            }
        }
    }

    private void BackspaceKey () {
        if(currentLetter >= 0) {
            name.text = name.text.Remove(currentLetter, 1).Insert(currentLetter, BLANKSPACE);
            currentLetter--;
        }
    }

    private void TypeKey ( char key ) {
        if(currentLetter < MAXLETTERS - 1) {//starts from -1
            currentLetter++;
            name.text = name.text.Remove(currentLetter, 1).Insert(currentLetter, key.ToString());
        }
    }
}

