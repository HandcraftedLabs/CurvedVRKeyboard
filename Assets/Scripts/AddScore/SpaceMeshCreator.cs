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
        List<Vector3> verticiesarray = new List<Vector3>();
        List<int> trainglesarray = new List<int>();

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

        for(int i = 2;i < 32;i += 2) {
            float xPos = -1.5f + ( ( i - 2 ) * 0.25f ) / 2f;

            trainglesarray.Add(i);
            trainglesarray.Add(i + 1);
            trainglesarray.Add(i + 2);
            verticiesarray.Add(new Vector3(xPos, 0.5f, 0));

            trainglesarray.Add(i + 3);
            trainglesarray.Add(i + 2);
            trainglesarray.Add(i + 1);
            verticiesarray.Add(new Vector3(xPos, -0.5f, 0));
        }
        float offset = 0;
        for(int i = 0;i < verticiesarray.Count;i += 2) {
            verticiesarray[i] = CalculatePositionOnCylinder(verticiesarray[i], creator, offset) - new Vector3(0, 0, creator.centerPointDistance);
            verticiesarray[i + 1] = CalculatePositionOnCylinder(verticiesarray[i + 1], creator, offset) - new Vector3(0, 0, creator.centerPointDistance);
            offset += 0.25f;
        }


        mesh.vertices = verticiesarray.ToArray();
        mesh.triangles = trainglesarray.ToArray();
        mesh.RecalculateNormals();
        backMesh.gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;

    }




    public void BuildFrontface () {
        Mesh mesh = frontMesh.GetComponent<MeshFilter>().sharedMesh;
        List<Vector3> verticiesarray = new List<Vector3>();
        List<int> trainglesarray = new List<int>();

        verticiesarray.Add(new Vector3(-2f, 0.5f, 0));
        verticiesarray.Add(new Vector3(-2f, -0.5f, 0f));
        verticiesarray.Add(new Vector3(-1.75f, 0.5f, 0f));
        verticiesarray.Add(new Vector3(-1.75f, -0.5f, 0f));
        trainglesarray.Add(2);
        trainglesarray.Add(1);
        trainglesarray.Add(0);
        trainglesarray.Add(1);
        trainglesarray.Add(2);
        trainglesarray.Add(3);

        for(int i = 2;i < 32;i += 2) {
            float xPos = -1.5f + ( ( i - 2 ) * 0.25f ) / 2f;

            trainglesarray.Add(i + 2);
            trainglesarray.Add(i + 1);
            trainglesarray.Add(i);
            verticiesarray.Add(new Vector3(xPos, 0.5f, 0));

            trainglesarray.Add(i + 1);
            trainglesarray.Add(i + 2);
            trainglesarray.Add(i + 3);
            verticiesarray.Add(new Vector3(xPos, -0.5f, 0));
        }
        float offset = 0;
        for(int i = 0;i < verticiesarray.Count;i += 2) {
            verticiesarray[i] = CalculatePositionOnCylinder(verticiesarray[i], creator, offset) - new Vector3(0, 0, creator.centerPointDistance);
            verticiesarray[i + 1] = CalculatePositionOnCylinder(verticiesarray[i + 1], creator, offset) - new Vector3(0, 0, creator.centerPointDistance);
            offset += 0.25f;
        }


        mesh.vertices = verticiesarray.ToArray();
        mesh.triangles = trainglesarray.ToArray();
        mesh.RecalculateNormals();
        frontMesh.gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;

    }




    public Vector3 CalculatePositionOnCylinder ( Vector3 lastVert, KeyboardCreator creator, float offset ) {
        float rowSize = 4f;
        float degree = Mathf.Deg2Rad * ( 90f + rowSize * creator.SpacingBetweenKeys / 2 - offset * creator.SpacingBetweenKeys );
        float x = Mathf.Cos(degree) * creator.centerPointDistance;
        float z = Mathf.Sin(degree) * creator.centerPointDistance;
        return new Vector3(x, lastVert.y, z);
    }
}
