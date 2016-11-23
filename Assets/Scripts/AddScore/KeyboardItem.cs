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

    private float maxX = 0;

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
            Renderer[] quadRenderers = GetComponentsInChildren<Renderer>();
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
        KeyboardCreator keyboardCreator = GameObject.Find("KeyBoard").GetComponent<KeyboardCreator>();
        float curvature = keyboardCreator.Curvature;
        float centerPointDistance = keyboardCreator.centerPointDistance;
        
        

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

        //add 2 verticies 
        verticiesarray.Add(new Vector3(-0.75f, 0.5f, 0));
        trainglesarray.Add(3);
        trainglesarray.Add(0);
        trainglesarray.Add(4);

        verticiesarray.Add(new Vector3(-0.75f, -0.5f, 0f));
        trainglesarray.Add(0);
        trainglesarray.Add(5);
        trainglesarray.Add(4);

        //fill rest
        for(int i = 4;i <= 12;i += 2) {
            float xPos = -1 - ((i-4) * 0.25f)/2f;
            CreateQuadXPlus(xPos, i, trainglesarray, verticiesarray);
        }

        //add 2 verticies
        verticiesarray.Add(new Vector3(0.75f, 0.5f, 0f));
        trainglesarray.Add(16);
        trainglesarray.Add(2);
        trainglesarray.Add(1);

        verticiesarray.Add(new Vector3(0.75f, -0.5f, 0f));
        trainglesarray.Add(2);
        trainglesarray.Add(16);
        trainglesarray.Add(17);

        //fill rest
        for(int i = 16;i <= 24;i += 2) {
            float xPos = 1 + ( ( i - 16 ) * 0.25f ) / 2f;
            CreateQuadXMinus(xPos, i, trainglesarray, verticiesarray);
        }

        Vector3 lastVert = verticiesarray[verticiesarray.Count - 2];
        float offset = 0;
        for(int i = verticiesarray.Count - 1;i >= 16;i = i - 2) {
            verticiesarray[i] = CalculatePositionOnCylinder(verticiesarray[i], keyboardCreator, offset);
            verticiesarray[i - 1] = CalculatePositionOnCylinder(verticiesarray[i - 1], keyboardCreator, offset);
            offset += 0.25f;
        }


        //verticiesarray[verticiesarray.Count - 3] = CalculatePositionOnCylinder(verticiesarray[verticiesarray.Count - 3], keyboardCreator, 0f);
        //verticiesarray[verticiesarray.Count - 4] = CalculatePositionOnCylinder(verticiesarray[verticiesarray.Count - 4], keyboardCreator, 0f);

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

    private void CreateQuadXPlus (float xPos, int firstVertex, List<int> trianglesList, List<Vector3> VertexList) {
        VertexList.Add(new Vector3(xPos, 0.5f, 0f));
        trianglesList.Add(firstVertex);
        trianglesList.Add(firstVertex + 1);
        trianglesList.Add(firstVertex + 2);

        VertexList.Add(new Vector3(xPos, -0.5f, 0f));
        trianglesList.Add(firstVertex + 1);
        trianglesList.Add(firstVertex + 3);
        trianglesList.Add(firstVertex + 2);
    }

    private void CreateQuadXMinus ( float xPos, int firstVertex, List<int> trianglesList, List<Vector3> VertexList ) {
        VertexList.Add(new Vector3(xPos, 0.5f, 0f));
        trianglesList.Add(firstVertex);
        trianglesList.Add(firstVertex + 2);
        trianglesList.Add(firstVertex + 1);

        VertexList.Add(new Vector3(xPos, -0.5f, 0f));
        trianglesList.Add(firstVertex + 1);
        trianglesList.Add(firstVertex + 2);
        trianglesList.Add(firstVertex + 3);
    }


    public Vector3 CalculatePositionOnCylinder (Vector3 lastVert, KeyboardCreator creator,float offset) {
        //row size - offset of current letter position
        float rowSize = 4f;
        float degree = Mathf.Deg2Rad * ( rowSize * creator.SpacingBetweenKeys / 2 - offset * creator.SpacingBetweenKeys );

        float x = Mathf.Cos(degree) * creator.centerPointDistance;
        float z = Mathf.Sin(degree) * creator.centerPointDistance;
        Debug.Log("x: " + x + "z: " + z);
        return new Vector3(x, lastVert.y, z);


    }
}




