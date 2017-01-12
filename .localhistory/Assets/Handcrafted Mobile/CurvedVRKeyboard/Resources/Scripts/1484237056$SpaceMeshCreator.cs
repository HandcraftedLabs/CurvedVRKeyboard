using UnityEngine;
using System.Collections.Generic;
using System;

namespace CurvedVRKeyboard {

    public class SpaceMeshCreator {

        KeyboardCreator creator;
        UvSlicer uvSlicer; 

        List<Vector3> verticiesArray;
        private bool isFrontFace;


        //-----BuildingData-----
        private float boundaryY = 0.5f;
        private float boundaryX = 2f;
        private int verticiesCount = 32;
        private float rowSize = 4;
        private float verticiesSpacing;


        
        public SpaceMeshCreator (KeyboardCreator creator,Sprite texture = null) {
            this.creator = creator;
            if(texture != null) {
                Change9Slices(texture,creator.SpaceWidth,creator.SpaceHeight);
            }
        }

        public void ChangeTexture (Sprite texture) {
            Change9Slices(texture, creator.SpaceWidth, creator.SpaceHeight);
        }

        /// <summary>
        /// Builds mesh for space bar
        /// </summary>
        /// <param name="renderer"> Renderer to get nesh from</param>
        /// <param name="frontFace"> True if front face needs to be rendered. False if back face</param>
        public void BuildFace ( Renderer renderer, bool frontFace) {
            verticiesSpacing = rowSize / ( verticiesCount / rowSize );
            isFrontFace = frontFace;
            Mesh mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
            List<int> trainglesArray = new List<int>();

            BuildVerticies();
            BuildQuads(trainglesArray);

            renderer.gameObject.GetComponent<MeshFilter>().sharedMesh = RebuildMesh(mesh, verticiesArray, trainglesArray);

            CalculatePosition(verticiesArray);
            mesh.vertices = verticiesArray.ToArray();
            LogList(verticiesArray);

            mesh.RecalculateNormals();
        }


        private void BuildVerticies () {
            //TODO uncoment this in futuere 
            // if(verticiesArray == null) {//lazy initialization
            verticiesArray = new List<Vector3>();
            for(float currentX = -boundaryX;currentX <= boundaryX;currentX += verticiesSpacing) {
                AddWholeColumn(new Vector3(currentX, 0, 0));
                if(uvSlicer.CheckVerticalBorders(currentX, verticiesSpacing)) {
                    AddWholeColumn(uvSlicer.GetVerticalVector());
                }
            }
            //}
        }


         
        

        private void AddWholeColumn (Vector3 toAdd ) {
            for(int row=0;row<rowSize;row++) {
                verticiesArray.Add(toAdd);
            }
        }


        /// <summary>
        /// Builds triangles from array of integers
        /// </summary>
        /// <param name="trianglesArray"> Array to be builded</param>
        private void BuildQuads ( List<int> trianglesArray ) {


            if(isFrontFace) {
                for(int i = 0;i < 39 ;i++) {
                        trianglesArray.Add(i + 4);
                        trianglesArray.Add(i + 1);
                        trianglesArray.Add(i);

                        trianglesArray.Add(i + 1);
                        trianglesArray.Add(i + 4);
                        trianglesArray.Add(i + 5);
                    if(i % rowSize == 2) {//we must skip every 3rd iteration
                        i++; 
                    }
                }
            }
            //TODO MAKE A BACKQUADS
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
        private void CalculatePosition ( List<Vector3> verticiesArray) {
            float offset = 0;
            for(int i = 0;i < verticiesArray.Count;i += 4) {
                Vector3 calculatedVertex = creator.CalculatePositionOnCylinder(rowSize, offset);

                if(i + 4 < verticiesArray.Count) {//if there is next value in array
                    offset += verticiesArray[i + 4].x - verticiesArray[i].x;
                }

                calculatedVertex.z -= creator.centerPointDistance;
          
                calculatedVertex.y = boundaryY;
                this.verticiesArray[i] = calculatedVertex;

                calculatedVertex.y = uvSlicer.top;
                this.verticiesArray[i + 1] = calculatedVertex;

                calculatedVertex.y = uvSlicer.bot;
                this.verticiesArray[i + 2] = calculatedVertex;

                calculatedVertex.y = -boundaryY;
                this.verticiesArray[i + 3] = calculatedVertex;
      
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
            mesh.triangles = trainglesArray.ToArray();
            mesh.uv = uvSlicer.BuildUV(verticiesArray,boundaryX,boundaryY);         
            return mesh;
        }

        public void Change9Slices (Sprite mainTexture,int width, int height) {
            uvSlicer = new UvSlicer(mainTexture,width,height);
        }

        private void LogList ( List<Vector3> list ) {
            String output = "";
            foreach(Vector3 element in list) {
                output += string.Format("x:{0}, y{1}:, z{2} \n", element.x,element.y,element.z);
            }
            Debug.Log(output);
            Debug.Log(list.Count);
        }

        private void LogArray ( Vector2[] list ) {
            String output = "";
            foreach(Vector3 element in list) {
                output += element.ToString();
                output += " :: ";
            }
            Debug.Log(output);
            Debug.Log(list.Length);
        }
    }


}