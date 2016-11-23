using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceMeshCreator{


    Renderer backMesh;
    Renderer frontMesh;
    KeyboardCreator creator;

    public SpaceMeshCreator( Renderer backMesh, Renderer frontMesh, KeyboardCreator creator ) {
        this.backMesh = backMesh;
        this.frontMesh = frontMesh;
        this.creator = creator;
    }

    public void BuildBackface() {
        Mesh mesh = backMesh.GetComponent<MeshFilter>().sharedMesh;
        List<Vector3> verticiesArray = new List<Vector3>();
        List<int> trainglesArray = new List<int>();
        BuildBackQuads(verticiesArray, trainglesArray);
        CalculatePosition(verticiesArray);
        backMesh.gameObject.GetComponent<MeshFilter>().sharedMesh = RebuildMesh(mesh, verticiesArray, trainglesArray);
    }

    public void BuildFrontface () {
        Mesh mesh = frontMesh.GetComponent<MeshFilter>().sharedMesh;
        List<Vector3> verticiesArray = new List<Vector3>();
        List<int> trainglesarray = new List<int>();
        BuildFirstQuadFront(verticiesArray, trainglesarray);
        BuildFrontQuads(verticiesArray, trainglesarray);
        CalculatePosition(verticiesArray);
        frontMesh.gameObject.GetComponent<MeshFilter>().sharedMesh =  RebuildMesh(mesh, verticiesArray, trainglesarray);

    }

    private  void BuildFrontQuads ( List<Vector3> verticiesArray, List<int> trainglesarray ) {
        BuildFirstQuadFront(verticiesArray,trainglesarray);
        for(int i = 2;i < 32;i += 2) {
            float xPos = -1.5f + ( ( i - 2 ) * 0.25f ) / 2f;

            trainglesarray.Add(i + 2);
            trainglesarray.Add(i + 1);
            trainglesarray.Add(i);
            verticiesArray.Add(new Vector3(xPos, 0.5f, 0));

            trainglesarray.Add(i + 1);
            trainglesarray.Add(i + 2);
            trainglesarray.Add(i + 3);
            verticiesArray.Add(new Vector3(xPos, -0.5f, 0));
        }
    }

    private  void BuildFirstQuadFront ( List<Vector3> verticiesArray, List<int> trainglesarray ) {
        verticiesArray.Add(new Vector3(-2f, 0.5f, 0));
        verticiesArray.Add(new Vector3(-2f, -0.5f, 0f));
        verticiesArray.Add(new Vector3(-1.75f, 0.5f, 0f));
        verticiesArray.Add(new Vector3(-1.75f, -0.5f, 0f));
        trainglesarray.Add(2);
        trainglesarray.Add(1);
        trainglesarray.Add(0);
        trainglesarray.Add(1);
        trainglesarray.Add(2);
        trainglesarray.Add(3);
    }

    private Mesh RebuildMesh ( Mesh mesh, List<Vector3> verticiesarray, List<int> trainglesarray ) {
        mesh.vertices = verticiesarray.ToArray();
        mesh.triangles = trainglesarray.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    private void CalculatePosition ( List<Vector3> verticiesarray ) {
        float offset = 0;
        for(int i = 0;i < verticiesarray.Count;i += 2) {
            verticiesarray[i] = CalculatePositionOnCylinder(verticiesarray[i], creator, offset) - new Vector3(0, 0, creator.centerPointDistance);
            verticiesarray[i + 1] = CalculatePositionOnCylinder(verticiesarray[i + 1], creator, offset) - new Vector3(0, 0, creator.centerPointDistance);
            offset += 0.25f;
        }
    }

    private static void BuildBackQuads ( List<Vector3> verticiesArray, List<int> trianglesArray ) {
        BuildFirstBackQuad(verticiesArray, trianglesArray);
        for(int i = 2;i < 32;i += 2) {
            float xPos = -1.5f + ( ( i - 2 ) * 0.25f ) / 2f;

            trianglesArray.Add(i);
            trianglesArray.Add(i + 1);
            trianglesArray.Add(i + 2);
            verticiesArray.Add(new Vector3(xPos, 0.5f, 0));

            trianglesArray.Add(i + 3);
            trianglesArray.Add(i + 2);
            trianglesArray.Add(i + 1);
            verticiesArray.Add(new Vector3(xPos, -0.5f, 0));
        }
    }

    private static void BuildFirstBackQuad ( List<Vector3> verticiesarray, List<int> trainglesarray ) {
        verticiesarray.Add(new Vector3(-2f, 0.5f, 0));
        verticiesarray.Add(new Vector3(-2f, -0.5f, 0f));
        verticiesarray.Add(new Vector3(-1.75f, 0.5f, 0f));
        verticiesarray.Add(new Vector3(-1.75f, -0.5f, 0f));
        trainglesarray.Add(0);
        trainglesarray.Add(1);
        trainglesarray.Add(2);
        trainglesarray.Add(3);
        trainglesarray.Add(2);
        trainglesarray.Add(1);
    }



    




    public Vector3 CalculatePositionOnCylinder ( Vector3 lastVert, KeyboardCreator creator, float offset ) {
        float rowSize = 4f;
        float degree = Mathf.Deg2Rad * ( 90f + rowSize * creator.SpacingBetweenKeys / 2 - offset * creator.SpacingBetweenKeys );
        float x = Mathf.Cos(degree) * creator.centerPointDistance;
        float z = Mathf.Sin(degree) * creator.centerPointDistance;
        return new Vector3(x, lastVert.y, z);
    }
}
