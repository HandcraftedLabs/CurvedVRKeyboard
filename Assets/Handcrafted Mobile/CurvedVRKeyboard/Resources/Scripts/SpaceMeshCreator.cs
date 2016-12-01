using UnityEngine;
using System.Collections.Generic;
namespace CurvedVRKeyboard {

    public class SpaceMeshCreator {


        KeyboardCreator creator;
        List<Vector3> verticiesArray;
        private bool isFrontFace;

        //-----BuildingData-----
        private float boundaryY = 0.5f;
        private float boundaryX = 2f;
        private int verticiesCount = 32;
        private float rowSize = 4f;




        public SpaceMeshCreator ( KeyboardCreator creator ) {
            this.creator = creator;
        }

        /// <summary>
        /// Builds mesh for space bar
        /// </summary>
        /// <param name="renderer"> Renderer to get nesh from</param>
        /// <param name="frontFace"> True if front face needs to be rendered. False if back face</param>
        public void BuildFace ( Renderer renderer, bool frontFace ) {
            isFrontFace = frontFace;
            Mesh mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
            List<int> trainglesArray = new List<int>();
            BuildVerticies();
            BuildQuads(trainglesArray);
            CalculatePosition(verticiesArray);
            renderer.gameObject.GetComponent<MeshFilter>().sharedMesh = RebuildMesh(mesh, verticiesArray, trainglesArray);
        }


        private void BuildVerticies () {
            if(verticiesArray == null) {//lazy initialization
                verticiesArray = new List<Vector3>();
                for(float i = -boundaryX;i <= boundaryX;i += 0.25f) {
                    verticiesArray.Add(new Vector3(i, boundaryY, 0));
                    verticiesArray.Add(new Vector3(i, -boundaryY, 0));
                }
            }
        }

        /// <summary>
        /// Builds triangles from array of integers
        /// </summary>
        /// <param name="trianglesArray"> Array to be builded</param>
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

        /// <summary>
        /// Calculates position for verticies
        /// </summary>
        /// <param name="verticiesArray"> Array of verticies</param>
        private void CalculatePosition ( List<Vector3> verticiesArray ) {
            float offset = 0;
            for(int i = 0;i < verticiesArray.Count;i += 2) {
                Vector3 calculatedVertex = creator.CalculatePositionOnCylinder(rowSize, offset);
                calculatedVertex.z -= creator.centerPointDistance;

                calculatedVertex.y = 0.5f;
                this.verticiesArray[i] = calculatedVertex;

                calculatedVertex.y = -0.5f;
                this.verticiesArray[i + 1] = calculatedVertex;

                offset += 0.25f;
            }
        }

        /// <summary>
        /// Apply all changes 
        /// </summary>
        /// <param name="mesh"> Mesh to be changed</param>
        /// <param name="verticiesArray"> Calculated positions of verticies</param>
        /// <param name="trainglesArray"> Calculated triangles </param>
        /// <returns></returns>
        private Mesh RebuildMesh ( Mesh mesh, List<Vector3> verticiesArray, List<int> trainglesArray ) {
            mesh.vertices = verticiesArray.ToArray();
            mesh.triangles = trainglesArray.ToArray();
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}