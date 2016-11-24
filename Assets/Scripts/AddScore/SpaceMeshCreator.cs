using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceMeshCreator{


    KeyboardCreator creator;
    List<Vector3> verticiesArray;
    private bool isFrontFace;


    //-------------BuildingData--------------
    private Vector3 zero = new Vector3(-2f, 0.5f, 0);
    private Vector3 one = new Vector3(-2f, -0.5f, 0f);
    private Vector3 two = new Vector3(-1.75f, 0.5f, 0f);
    private Vector3 three = new Vector3(-1.75f, -0.5f, 0f);

    private float boundaryY = 0.5f;
    private float boundaryX = 2f;
    private int verticiesCount = 32;
    private float rowSize = 4f;



    public SpaceMeshCreator(KeyboardCreator creator ) {
        this.creator = creator;  
    }

    public void BuildFace(Renderer renderer,bool frontFace) {
        isFrontFace = frontFace;
        Mesh mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
        List<int> trainglesArray = new List<int>();

        BuildVerticies();
        BuildQuads(trainglesArray);

        CalculatePosition(verticiesArray);
        renderer.gameObject.GetComponent<MeshFilter>().sharedMesh = RebuildMesh(mesh, verticiesArray, trainglesArray);
    }

 
    private void BuildVerticies () {
        if(verticiesArray == null) {//lazy initialization reusable
            verticiesArray = new List<Vector3>();
            for(float i = -boundaryX;i <= boundaryX;i += 0.25f) {
                verticiesArray.Add(new Vector3(i, boundaryY, 0));
                verticiesArray.Add(new Vector3(i, -boundaryY, 0));
            }
        }
    }

    private void BuildQuads ( List<int> trianglesArray ) {
        if(isFrontFace) {
            for(int i = 0;i < verticiesCount;i += 2) {
                trianglesArray.Add(i + 2);
                trianglesArray.Add(i + 1);
                trianglesArray.Add(i);

                trianglesArray.Add(i + 1);
                trianglesArray.Add(i + 2);
                trianglesArray.Add(i + 3);
            }
        } else {
            for(int i = 0;i < verticiesCount;i += 2) {
                trianglesArray.Add(i);
                trianglesArray.Add(i + 1);
                trianglesArray.Add(i + 2);

                trianglesArray.Add(i + 3);
                trianglesArray.Add(i + 2);
                trianglesArray.Add(i + 1);
            }
        }
    }



    private void CalculatePosition ( List<Vector3> verticiesArray ) {
        float offset = 0;
        for(int i = 0; i < verticiesArray.Count;i += 2) {
            Vector3 calculatedVertex = creator.CalculatePositionOnCylinder(rowSize, offset);
            calculatedVertex.z -= creator.centerPointDistance;
            
            calculatedVertex.y = 0.5f;
            this.verticiesArray[i] = calculatedVertex;

            calculatedVertex.y = -0.5f;
            this.verticiesArray[i + 1] = calculatedVertex;

            offset += 0.25f;
        }
    }

    private Mesh RebuildMesh ( Mesh mesh, List<Vector3> verticiesArray, List<int> trainglesArray ) {
        mesh.vertices = verticiesArray.ToArray();
        mesh.triangles = trainglesArray.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }
}
