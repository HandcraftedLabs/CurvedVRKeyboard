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

    public SpaceMeshCreator(KeyboardCreator creator ) {
        this.creator = creator;
       
    }

    public void BuildBackface(Renderer renderer,bool frontFace) {
        isFrontFace = frontFace;
        Mesh mesh = renderer.GetComponent<MeshFilter>().sharedMesh;

        BuildVerticies();
        List<int> trainglesArray = new List<int>();
        BuildQuads(trainglesArray);

        CalculatePosition(verticiesArray);
        renderer.gameObject.GetComponent<MeshFilter>().sharedMesh = RebuildMesh(mesh, verticiesArray, trainglesArray);
    }

 
    private void BuildVerticies () {
        if(verticiesArray == null) {
            verticiesArray = new List<Vector3>();
            verticiesArray.Add(zero);
            verticiesArray.Add(one);
            verticiesArray.Add(two);
            verticiesArray.Add(three);

            for(float i = -1.5f;i <= 2f;i += 0.25f) {
                verticiesArray.Add(new Vector3(i, boundaryY, 0));
                verticiesArray.Add(new Vector3(i, -boundaryY, 0));
            }
        }
    }

  

    private Mesh RebuildMesh ( Mesh mesh, List<Vector3> verticiesarray, List<int> trainglesarray ) {
        mesh.vertices = verticiesarray.ToArray();
        mesh.triangles = trainglesarray.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }
    //use function from KeybaordCreator
    private void CalculatePosition ( List<Vector3> verticiesarray ) {
        float offset = 0;
        for(int i = 0; i < verticiesarray.Count;i += 2) {
            verticiesarray[i] = CalculatePositionOnCylinder(verticiesarray[i], creator, offset) - new Vector3(0, 0, creator.centerPointDistance);
            verticiesarray[i + 1] = CalculatePositionOnCylinder(verticiesarray[i + 1], creator, offset) - new Vector3(0, 0, creator.centerPointDistance);
            offset += 0.25f;
        }
    }

    private  void BuildQuads (List<int> trianglesArray ) {
        BuildFirstQuad(trianglesArray);
        if(isFrontFace) {
            for(int i = 2;i < 32;i += 2) {
                trianglesArray.Add(i + 2);
                trianglesArray.Add(i + 1);
                trianglesArray.Add(i);

                trianglesArray.Add(i + 1);
                trianglesArray.Add(i + 2);
                trianglesArray.Add(i + 3);
            }
        }
        else{
            for(int i = 2;i < 32;i += 2) {
                trianglesArray.Add(i);
                trianglesArray.Add(i + 1);
                trianglesArray.Add(i + 2);

                trianglesArray.Add(i + 3);
                trianglesArray.Add(i + 2);
                trianglesArray.Add(i + 1);
            }
        }
    }

    private  void BuildFirstQuad (List<int> trainglesarray ) {
        if(isFrontFace) {
            trainglesarray.Add(2);
            trainglesarray.Add(1);
            trainglesarray.Add(0);

            trainglesarray.Add(1);
            trainglesarray.Add(2);
            trainglesarray.Add(3);
        } else {
            trainglesarray.Add(0);
            trainglesarray.Add(1);
            trainglesarray.Add(2);

            trainglesarray.Add(3);
            trainglesarray.Add(2);
            trainglesarray.Add(1);
        }
    }

    public Vector3 CalculatePositionOnCylinder ( Vector3 lastVert, KeyboardCreator creator, float offset ) {
        float rowSize = 4f;
        float degree = Mathf.Deg2Rad * ( 90f + rowSize * creator.SpacingBetweenKeys / 2 - offset * creator.SpacingBetweenKeys );
        float x = Mathf.Cos(degree) * creator.centerPointDistance;
        float z = Mathf.Sin(degree) * creator.centerPointDistance;
        return new Vector3(x, lastVert.y, z);
    }
}
