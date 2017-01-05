using UnityEngine;
using System.Collections.Generic;
using System;

namespace CurvedVRKeyboard {

    public class SpaceMeshCreator {

        KeyboardCreator creator;

        float left = 0.214f;
        float right = 1 - 0.214f ;
        float top = 0.4f;
        float bot = -0.4f;

        List<Vector3> verticiesArray;
        private bool isFrontFace;

        //-----BuildingData-----
        private float boundaryY = 0.5f;
        private float boundaryX = 2f;
        private int verticiesCount = 32;
        private float rowSize = 4f;
        private float verticiesSpacing;



        public SpaceMeshCreator (KeyboardCreator creator,Sprite texture = null) {
            this.creator = creator;
            
        }

        /// <summary>
        /// Builds mesh for space bar
        /// </summary>
        /// <param name="renderer"> Renderer to get nesh from</param>
        /// <param name="frontFace"> True if front face needs to be rendered. False if back face</param>
        public void BuildFace ( Renderer renderer, bool frontFace ) {
            verticiesSpacing = rowSize / ( verticiesCount / 4 );
            isFrontFace = frontFace;
            Mesh mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
            List<int> trainglesArray = new List<int>();
            BuildVerticies();
            LogList(verticiesArray);
            BuildQuads(trainglesArray);
            CalculatePosition(verticiesArray);
            renderer.gameObject.GetComponent<MeshFilter>().sharedMesh = RebuildMesh(mesh, verticiesArray, trainglesArray);
        }


        private void BuildVerticies (float upper = 0.05f,float lower = -0.05f) {

           // if(verticiesArray == null) {//lazy initialization
                verticiesArray = new List<Vector3>();
                for(float i = -boundaryX;i <= boundaryX;i += verticiesSpacing) {
                    for(int row = 0;row < 4;row++) {
                        verticiesArray.Add(new Vector3(i, 0, 0));
                    }
                }
            //}
        }


        /// <summary>
        /// Builds triangles from array of integers
        /// </summary>
        /// <param name="trianglesArray"> Array to be builded</param>
        private void BuildQuads ( List<int> trianglesArray ) {
            if(isFrontFace) {
                for(int i = 0;i < verticiesCount ;i++) {
                        trianglesArray.Add(i + 4);
                        trianglesArray.Add(i + 1);
                        trianglesArray.Add(i);

                        trianglesArray.Add(i + 1);
                        trianglesArray.Add(i + 4);
                        trianglesArray.Add(i + 5);
                    if(i % 4 == 2) {//we must skip every 3rd iteration
                        i++; 
                    }
                }
            }
            //else {
            //    for(int i = 0;i < verticiesCount;i += 2) {
            //        trianglesArray.Add(i);
            //        trianglesArray.Add(i + 1);
            //        trianglesArray.Add(i + 2);

            //        trianglesArray.Add(i + 3);
            //        trianglesArray.Add(i + 2);
            //        trianglesArray.Add(i + 1);
            //    }
            //}
        }


        private Vector2[] BuildUV () {
            Vector2[] uv = new Vector2[verticiesCount + 2];
            float border = 1f / ( verticiesCount / 2f );
            uv[0] = new Vector2(0f, 1f);
            uv[1] = new Vector2(0, 0f);
            uv[2] = new Vector2(border, 1);
            uv[3] = new Vector2(border, 0f);
            for(int i = 0;i < verticiesCount;i += 4) {
                float uvPoint = border * ( i / 2f );
                uv[i] = new Vector2(uvPoint, 1);
                uv[i + 1] = new Vector2(uvPoint, 0);
                uvPoint += border;
                uv[i + 2] = new Vector2(uvPoint, 1);
                uv[i + 3] = new Vector2(uvPoint, 0);
            }
            uv[verticiesCount] = new Vector2(1, 1);
            uv[verticiesCount + 1] = new Vector2(1, 0);
            return uv;
        }



        /// <summary>
        /// Calculates position for verticies
        /// </summary>
        /// <param name="verticiesArray"> Array of verticies</param>
        private void CalculatePosition ( List<Vector3> verticiesArray, float upper = 0.5f, float lower = -0.5f ) {
            float offset = 0;
            for(int i = 0;i < verticiesArray.Count;i += 4) {
                Vector3 calculatedVertex = creator.CalculatePositionOnCylinder(rowSize, offset);
                calculatedVertex.z -= creator.centerPointDistance;

                calculatedVertex.y = 0.5f;
                this.verticiesArray[i] = calculatedVertex;

                calculatedVertex.y = upper;
                this.verticiesArray[i + 1] = calculatedVertex;

                calculatedVertex.y = lower;
                this.verticiesArray[i + 2] = calculatedVertex;

                calculatedVertex.y = -0.5f;
                this.verticiesArray[i + 3] = calculatedVertex;

                offset += verticiesSpacing;
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
            // mesh.uv = BuildUV();
            mesh.RecalculateNormals();
            return mesh;
        }

        public void Change9Slices (Sprite mainTexture) {
            //mainTexture.
            //left = mainTexture.border.x / mainTexture
        }

        private void LogList ( List<Vector3> list ) {
            String output = "";
            foreach(Vector3 element in list) {
                output += element.ToString();
                output += " :: ";
            }
            Debug.Log(output);
        }
    }
}