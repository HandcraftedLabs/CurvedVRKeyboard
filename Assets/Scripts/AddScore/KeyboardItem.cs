using System;
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

    
    private Renderer quadFront;
    private Renderer quadBack;
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
            Renderer[]  quadRenderers = GetComponentsInChildren<Renderer>();
            quadFront = quadRenderers[0];
            quadBack = quadRenderers[1];
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

    public void ManipulateMesh () {
        //Mesh mesh = new Mesh();
        string traingles = "triangles: ";
        string verticies = "verticies: ";

            Mesh mesh = quadFront.gameObject.GetComponent<MeshFilter>().sharedMesh;
          
            foreach(int trian in mesh.triangles) {
                traingles += " " + trian + " /";
            }
            foreach(Vector3 vertic in mesh.vertices) {
                verticies += " " + vertic.ToString() + " /";
            }

            List<Vector3> verticiesarray = new List<Vector3>(mesh.vertices);
            List<int> trainglesarray = new List<int>(mesh.triangles);

            verticiesarray.RemoveRange(4, verticiesarray.Capacity - 4);
            trainglesarray.RemoveRange(6, trainglesarray.Capacity - 6);
            verticiesarray.Add(new Vector3(-2f, 0.5f, 0));
            trainglesarray.Add(3);
            trainglesarray.Add(0);
            trainglesarray.Add(4);

            verticiesarray.Add(new Vector3(-2f, -0.5f, 0f));
            trainglesarray.Add(0);
            trainglesarray.Add(5);
            trainglesarray.Add(4);

            verticiesarray.Add(new Vector3(2f, 0.5f, 0f));
            trainglesarray.Add(2);
            trainglesarray.Add(1);
            trainglesarray.Add(6);

            verticiesarray.Add(new Vector3(2f, -0.5f, 0f));
            trainglesarray.Add(6);
            trainglesarray.Add(7);
            trainglesarray.Add(2);


            mesh.vertices = verticiesarray.ToArray();
            mesh.triangles = trainglesarray.ToArray();
            mesh.RecalculateNormals();

            quadFront.gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;


            traingles = "triangles: ";
            verticies = "verticies: ";
            foreach(int trian in mesh.triangles) {
                traingles += " " + trian + " /";
            }
            foreach(Vector3 vertic in mesh.vertices) {
                verticies += " " + vertic.ToString() + " /";
            }
       
        

        Debug.Log(traingles);
        Debug.Log(verticies);
        //mesh.vertices = newVertices;
        //mesh.uv = newUV;
        //mesh.triangles = newTriangles;
    }


}




