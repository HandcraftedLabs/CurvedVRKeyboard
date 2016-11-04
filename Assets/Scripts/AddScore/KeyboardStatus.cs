using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardStatus: KeyboardComponent {

    //-----------SET IN UNITY --------------
    [Tooltip("Textfiled on which letters will be written")]
    public Text Output;
    [Tooltip("Max letters aviable to Type")]
    public int maxLetters;
    //-----------SET IN UNITY --------------

    private KeyboardItem[] keys;
    private bool areLettersActive = true;
    private bool isLowercase = true;

    private static readonly string BLANKSPACE = " ";

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
        } else {//normalLetter
            TypeKey(value[0]);
        }
    }



    private void ChangeSpecialLetters () {
        areLettersActive = !areLettersActive;
        string[] ToDisplay = areLettersActive ? allLetters : allSpecials;
        for(int i = 0;i < keys.Length;i++) {
            keys[i].setLetterText(ToDisplay[i]);
        }
    }

    private void LowerUpperKeys () {
        isLowercase = !isLowercase;
        string[] ToDisplay = isLowercase ? allLetters : allLettersUpper;
        ChangeKeysDisplayied(ToDisplay);
    }

    private void ChangeKeysDisplayied ( string[] ToDisplay ) {
        for(int i = 0;i < keys.Length;i++) {
            keys[i].setLetterText(ToDisplay[i]);
        }
    }

    private void BackspaceKey () {
        if(currentLetter >= 0) {
            Output.text = Output.text.Remove(currentLetter, 1);
            currentLetter--;
        }
    }

    private void TypeKey ( char key ) {
        if(currentLetter < maxLetters - 1) {//starts from -1
            currentLetter++;
            Output.text = Output.text.Insert(currentLetter, key.ToString());
        }
    }

    public void SetKeys ( KeyboardItem[] keys ) {
        this.keys = keys;
    }

}

