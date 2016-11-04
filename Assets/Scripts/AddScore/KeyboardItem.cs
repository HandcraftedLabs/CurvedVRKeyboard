using UnityEngine;
using System.Collections;
using UnityEngine.UI;



[ExecuteInEditMode]
public class KeyboardItem: KeyboardComponent {

    private Text letter;
    private float clickPlayLimit = 0.15f;
    private float transitionTime = 0.01f;
    private float clickTimer = 0f;
    private bool clicked = false;
    private Animator animator;
    private Material keyDefaultMaterial;
    private Material keyHoldMaterial;
    private Material keyPressedMaterial;

    private Renderer renderer;

    public void Awake () {
        Init();
    }

    public void Init () {
        //check if was not destroyed
        if(letter == null || animator == null) {
            letter = gameObject.GetComponentInChildren<Text>();
            animator = gameObject.GetComponent<Animator>();
            renderer = gameObject.GetComponent<Renderer>();
        }
    }


    public void hovering () {
        if(!clicked) {
           changeMaterial(keyHoldMaterial);
            Debug.Log("error2?");
        } else {//wait for some time 
            HoldClick();
        }
    }

    private void HoldClick () {
        clickTimer += Time.deltaTime;
        changeMaterial(keyPressedMaterial);
        if(clickTimer >= clickPlayLimit) {
            clicked = false;
            clickTimer = 0f;
        }
    }

    public void stopHovering () {
        changeMaterial(keyDefaultMaterial);
    }

    public void click () {
        Debug.Log(letter);
        clicked = true;
        changeMaterial(keyPressedMaterial);
    }

    public string getValue () {
        return letter.text;
    }

    public void setLetterText (string value) {
        letter.text = value;
    }

    private void changeMaterial(Material material ) {
        renderer.material = material;
        Debug.Log("error?");
    }

    public void setMaterials (Material keyDefaultMaterial, Material keyHoldMaterial, Material keyPressedMaterial ) {
        this.keyDefaultMaterial = keyDefaultMaterial;
        this.keyHoldMaterial = keyHoldMaterial;
        this.keyPressedMaterial = keyPressedMaterial;
    }
}
