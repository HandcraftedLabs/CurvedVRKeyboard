using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KeyboardItem: KeyboardComponent {
    private Text letter;
    private bool clicked = false;
    private float clickHoldTimer = 0f;
    private float clickHoldTimeLimit = 0.15f;


    private Material keyDefaultMaterial;
    private Material keyHoveringMaterial;
    private Material keyPressedMaterial;

    private SpaceMeshCreator meshCreator;
    public Renderer quadFront;
    private Renderer quadBack;

    private static readonly string QUAD_FRONT = "Front";
    private static readonly string QUAD_BACK = "Back";

    public enum MaterialEnum {
        Default,
        Hovering,
        Pressed
    }

    public void Awake () {
        Init();
    }

    public void Init () {
        // check if was not destroyed
        if(letter == null || quadFront == null || quadBack == null) {
            letter = gameObject.GetComponentInChildren<Text>();
            quadBack = transform.Find(QUAD_BACK).GetComponent<Renderer>();
            quadFront = transform.Find(QUAD_FRONT).GetComponent<Renderer>();
        }
    }

    public void Hovering () {
        if(!clicked) {
            ChangeMaterial(keyHoveringMaterial);
        } else {
            HoldClick();
        }
    }

    private void HoldClick () {
        ChangeMaterial(keyPressedMaterial);

        clickHoldTimer += Time.deltaTime;
        if(clickHoldTimer >= clickHoldTimeLimit) {
            clicked = false;
            clickHoldTimer = 0f;
        }
    }

    public void StopHovering () {
        ChangeMaterial(keyDefaultMaterial);
    }

    public void Click () {
        clicked = true;
        ChangeMaterial(keyPressedMaterial);
    }

    public string GetValue () {
        return letter.text;
    }

    public void SetKeyText ( string value ) {
        if(!letter.text.Equals(value)) {
            letter.text = value;
        }
    }

    private void ChangeMaterial ( Material material ) {
        quadFront.material = material;
        quadBack.material = material;
    }

    public void SetMaterials ( Material keyDefaultMaterial, Material keyHoveringMaterial, Material keyPressedMaterial ) {
        this.keyDefaultMaterial = keyDefaultMaterial;
        this.keyHoveringMaterial = keyHoveringMaterial;
        this.keyPressedMaterial = keyPressedMaterial;
    }

    public void SetMaterial ( MaterialEnum materialEnum, Material newMaterial ) {
        switch(materialEnum) {
            case MaterialEnum.Default:
                keyDefaultMaterial = newMaterial;
                quadFront.material = newMaterial;
                quadBack.material = newMaterial;
                break;
            case MaterialEnum.Hovering:
                keyHoveringMaterial = newMaterial;
                break;
            case MaterialEnum.Pressed:
                keyPressedMaterial = newMaterial;
                break;
        }
    }

    public void ManipulateMesh ( KeyboardCreator creator ) {
        if(meshCreator == null) {//lazy initialization
            meshCreator = new SpaceMeshCreator(creator);
        }
        
        meshCreator.BuildFace(quadBack, false);
        meshCreator.BuildFace(quadFront, true);
       
    }

}



