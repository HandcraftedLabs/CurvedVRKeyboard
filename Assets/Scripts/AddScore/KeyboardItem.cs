using UnityEngine;
using System.Collections;
using UnityEngine.UI;



[ExecuteInEditMode]
public class KeyboardItem: KeyboardComponent {
    //-----------SET IN UNITY --------------
    public Text letter;
    //-----------SET IN UNITY --------------


    private float clickPlayLimit = 0.15f;
    private float transitionTime = 0.01f;
    private float clickTimer = 0f;
    private bool clicked = false;
    private Animator animator;

    public void Awake () {
        Init();

    }

    public void Init () {
        //check if was not destoied
        if(letter == null) {
            letter = gameObject.GetComponentInChildren<Text>();
            animator = gameObject.GetComponent<Animator>();
        }
    }

    private enum STATE {
        NORMAL,
        CHOSEN,
        CLICKED
    }





    void Update () {

    }

    public void hovering () {
        if(!clicked) {
            animator.CrossFade(EnumToString(STATE.CHOSEN), transitionTime);
        } else {//wait for some time 
            clickTimer += Time.deltaTime;
            animator.CrossFade(EnumToString(STATE.CLICKED), transitionTime);
            if(clickTimer >= clickPlayLimit) {
                clicked = false;
                clickTimer = 0f;
            }
        }
    }


    public void stopHovering () {
        animator.CrossFade(EnumToString(STATE.NORMAL), transitionTime);
    }

    public void click () {
        clicked = true;
        animator.CrossFade(EnumToString(STATE.CLICKED), 0.01f);
    }

    public string getValue () {
        return letter.text;
    }

    private string EnumToString ( STATE state ) {
        switch(state) {
            case STATE.NORMAL:
                return "Normal";
            case STATE.CLICKED:
                return "Clicked";
            default:
                return "Chosen";
        }
    }
}
