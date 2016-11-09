using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
/// <summary>
/// Setup class derived by all classes who are part of keyboard 
/// so those variables are easy accesable evrywhere
/// </summary>
public abstract class KeyboardComponent: MonoBehaviour {
    //if you want to reload keys double click BuildInEditorMode
    //on inspector in KeyboardCreator


    //Special signs feel free to change
   
    public static readonly string SPACE = "SPACE";
    public static readonly string BACK = "Back";
    public static readonly string ABC = "ABC";
    public static readonly string QEH = "?!#";
    public static readonly string UP = "UP";
    public static readonly string LOW = "low";

    public static readonly int CENTERITEM = 15;

    //Name of layer to cast must be same as layer in editor
    public static readonly string LAYER_TO_CAST = "KeyboardItem";

    //feel free to change (don't write strings in place of
    //special signs change variable data)
    public static readonly string[] allLetters = new string[]
    {
        "q","w","e","r","t","y","u","i","o","p",
        "a","s","d","f","g","h","j","k","l",
        UP,"z","x","c","v","b","n","m",QEH,SPACE,BACK
    };

    //feel free to change (don't write strings in place of
    //special signs change variable data)
    public static readonly string[] allLettersUpper = new string[]
   {
        "Q","W","E","R","T","Y","U","I","O","P",
        "A","S","D","F","G","H","J","K","L",
        LOW,"Z","X","C","V","B","N","M",QEH,SPACE,BACK
    };

    //feel free to change (don't write strings in place of
    //special signs change variable data)
    public static readonly string[] allSpecials = new string[]
    {
        "1","2","3","4","5","6","7","8","9","0",
        "@","#","£","_","&","-","+","(",")",
        "*","\"","'",":",";","/","!","?",
        ABC,SPACE,BACK
    };
    
    //don't change this
    public static readonly float[] rowLetters = new float[] { 10f, 9f, 8f,6f };

}
