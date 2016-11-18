using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Setup class derived by all classes who are part of keyboard 
/// so those variables are easy accessable everywhere
/// </summary>
public abstract class KeyboardComponent : MonoBehaviour {

    // Special signs. Feel free to change
    public static readonly string SPACE = "SPACE";
    public static readonly string BACK = "Back";
    public static readonly string ABC = "ABC";
    public static readonly string QEH = "?!#";
    public static readonly string UP = "UP";
    public static readonly string LOW = "low";

    public static readonly int CENTER_ITEM = 15;

    // Name of layer to cast; must be the same as the layer set in editor
    public static readonly string LAYER_TO_CAST = "KeyboardItem";

    // Feel free to change (but do not write strings in place of
    // special signs, change variables values instead)
    public static readonly string[] allLettersLowercase = new string[]
    {
        "q","w","e","r","t","y","u","i","o","p",
        "a","s","d","f","g","h","j","k","l",
        UP,"z","x","c","v","b","n","m",
        QEH, SPACE, BACK
    };

    // Feel free to change (but do not write strings in place of
    // special signs, change variables values instead)
    public static readonly string[] allLettersUppercase = new string[]
    {
        "Q","W","E","R","T","Y","U","I","O","P",
        "A","S","D","F","G","H","J","K","L",
        LOW,"Z","X","C","V","B","N","M",
        QEH, SPACE, BACK
    };

    // Feel free to change (but do not write strings in place of
    // special signs, change variables values instead)
    public static readonly string[] allSpecials = new string[]
    {
        "1","2","3","4","5","6","7","8","9","0",
        "@","#","£","_","&","-","+","(",")",
        "*","\"","'",":",";","/","!","?",
        ABC, SPACE, BACK
    };

    // Do not change this
    public static readonly float[] lettersInRowsCount = new float[] { 10f, 9f, 8f, 6f };

}
