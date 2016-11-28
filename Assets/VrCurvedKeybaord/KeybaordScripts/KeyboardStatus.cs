using UnityEngine;
using UnityEngine.UI;

public class KeyboardStatus : KeyboardComponent {

    //-----------SET IN UNITY --------------
    [Tooltip("Text field receiving input from the keyboard")]
    public Text output;
    [Tooltip("Maximum output text length")]
    public int maxOutputLength;
    //-----------SET IN UNITY --------------

    private KeyboardItem[] keys;
    private bool areLettersActive = true;
    private bool isLowercase = true;

    private static readonly char BLANKSPACE = ' ';
    /// <summary>
    /// Handles click on keyboarditem
    /// </summary>
    /// <param name="clicked">keyboard item clicked</param>
    public void HandleClick(KeyboardItem clicked) {
        string value = clicked.GetValue();

        if(value.Equals(QEH) || value.Equals(ABC)) { // special signs
            ChangeSpecialLetters();
        }
        else if(value.Equals(UP) || value.Equals(LOW)) { // upper/lower case
            LowerUpperKeys();
        }
        else if(value.Equals(SPACE)) {// if space
            TypeKey(BLANKSPACE);
        }
        else if(value.Equals(BACK)) {// if backspace
            BackspaceKey();
        }
        else {//normalLetter
            TypeKey(value[0]);
        }
    }

    /// <summary>
    /// Displays special signs
    /// </summary>
    private void ChangeSpecialLetters() {
        areLettersActive = !areLettersActive;
        string[] ToDisplay = areLettersActive ? allLettersLowercase : allSpecials;
        for(int i = 0; i < keys.Length; i++) {
            keys[i].SetKeyText(ToDisplay[i]);
        }
    }
    /// <summary>
    /// Changes between lower and upper keys
    /// </summary>
    private void LowerUpperKeys() {
        isLowercase = !isLowercase;
        string[] ToDisplay = isLowercase ? allLettersLowercase : allLettersUppercase;
        ChangeKeysDisplayied(ToDisplay);
    }

    private void ChangeKeysDisplayied(string[] ToDisplay) {
        for(int i = 0; i < keys.Length; i++) {
            keys[i].SetKeyText(ToDisplay[i]);
        }
    }

    private void BackspaceKey() {
        if(output.text.Length >= 1)
            output.text = output.text.Remove(output.text.Length - 1, 1);
    }

    private void TypeKey(char key) {
        if(output.text.Length < maxOutputLength)
            output.text = output.text + key.ToString();
    }

    public void SetKeys(KeyboardItem[] keys) {
        this.keys = keys;
    }
}

