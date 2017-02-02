using UnityEngine;
using UnityEngine.UI;

namespace CurvedVRKeyboard {
    [SelectionBase]
    public class KeyboardStatus: KeyboardComponent {

        //------SET IN UNITY-------
        [Tooltip("Text field receiving input from the keyboard")]
        public Text output;
        [Tooltip("Maximum output text length")]
        public int maxOutputLength;

        //----CurrentKeysStatus----
        private KeyboardItem[] keys;
        private bool areLettersActive = true;
        private bool isLowercase = true;
        private static readonly char BLANKSPACE = ' ';




        /// <summary>
        /// Handles click on keyboarditem
        /// </summary>
        /// <param name="clicked">keyboard item clicked</param>
        public void HandleClick ( KeyboardItem clicked ) {
            string value = clicked.GetValue();
            if(value.Equals(QEH) || value.Equals(ABC)) { // special signs pressed
                ChangeSpecialLetters();
            } else if(value.Equals(UP) || value.Equals(LOW)) { // upper/lower case pressed
                LowerUpperKeys();
            } else if(value.Equals(SPACE)) {
                TypeKey(BLANKSPACE);
            } else if(value.Equals(BACK)) {
                BackspaceKey();
            } else {// Normal letter
                TypeKey(value[0]);
            }
        }

        /// <summary>
        /// Displays special signs
        /// </summary>
        private void ChangeSpecialLetters () {
            KeyLetterEnum ToDisplay = areLettersActive ? KeyLetterEnum.NonLetters : KeyLetterEnum.LowerCase;
            areLettersActive =!areLettersActive;
            isLowercase = true;
            for(int i = 0;i < keys.Length;i++) {
                keys[i].SetKeyText(ToDisplay);
            }
        }

        /// <summary>
        /// Changes between lower and upper keys
        /// </summary>
        private void LowerUpperKeys () {
            KeyLetterEnum ToDisplay = isLowercase ? KeyLetterEnum.UpperCase : KeyLetterEnum.LowerCase;
            isLowercase = !isLowercase;
            for(int i = 0;i < keys.Length;i++) {
                keys[i].SetKeyText(ToDisplay);
            }
        }

        private void BackspaceKey () {
            if(output.text.Length >= 1)
                output.text = output.text.Remove(output.text.Length - 1, 1);
        }

        private void TypeKey ( char key ) {
            if(output.text.Length < maxOutputLength)
                output.text = output.text + key.ToString();
        }

        public void SetKeys ( KeyboardItem[] keys ) {
            this.keys = keys;
        }
    }
}
