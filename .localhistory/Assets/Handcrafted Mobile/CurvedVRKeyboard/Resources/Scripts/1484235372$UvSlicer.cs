
using System.Collections.Generic;
using UnityEngine;

namespace CurvedVRKeyboard {
    

    class UvSlicer {
        private Sprite spaceSprite;
        private Vector3 VerticalVector;

        public float left = -2;
        public float right = 2;
        public float top = 0.5f;
        public float bot = -0.5f;

        public float percentageLeft = 0;
        public float percentageRight= 1;
        public float percentageTop = 1;
        public float percentageBot = 0;


        private float virtualX ;
        private float virtualY ;

        public UvSlicer(Sprite spaceSprite ,int virtualX, int virtualY ) {
            this.spaceSprite = spaceSprite;
            this.virtualX = virtualX; 
            this.virtualY = virtualY;
            CalculateBorders(spaceSprite);
        }


        private void CalculateBorders ( Sprite spaceSprite ) {

            if(spaceSprite != null) {
                percentageLeft = ( spaceSprite.border.x / spaceSprite.bounds.size.x ) / 100f;
                left = (spaceSprite.border.x/virtualX * 4f) -2f;

                percentageRight = 1f - ( ( spaceSprite.border.z ) / spaceSprite.bounds.size.x ) / 100f;
                right = (1f - spaceSprite.border.z/virtualX) * 4f - 2f;

                percentageBot = ( spaceSprite.border.y / spaceSprite.bounds.size.y ) / 100f;
                bot = (spaceSprite.border.y )/ virtualY - 0.5f;


                percentageTop = 1f - ( ( spaceSprite.border.w ) / spaceSprite.bounds.size.y ) / 100f;
                top = (1f - spaceSprite.border.w / virtualY )- 0.5f;

            }
            //Debug.Log("");
            //Debug.Log("");
            //Debug.Log(string.Format("Top {0} : bot {1} : left {2} , right{3}", top, bot, left, right));
            //Debug.Log(string.Format("Top% {0} : bot% {1} : left% {2} , right% {3}",
            //    percentageTop, percentageBot, percentageLeft, percentageRight));
        }


        public bool CheckVerticalBorders (float current,float spacing) {
            if(current <= left && left <= current + spacing) { // left border shall be added??
                VerticalVector = new Vector3(left, 0, 0);
                return true;
            }
            if(right > current && right <= current + spacing) {
                VerticalVector = new Vector3(right, 0, 0);
                return true;
            }
            return false;
        }
        
        public Vector3 GetVerticalVector () {
            return VerticalVector;
        }


        public Vector2[] BuildUV (List<Vector3> verticiesArray, float boundaryX ,float boundaryY) {
            Vector2[] calculatedUV = new Vector2[verticiesArray.Count];
            float xRange = boundaryX * 2f;
            float leftSize = Mathf.Abs(left - (-boundaryX)) + Mathf.Epsilon;
            float rightSize = Mathf.Abs(boundaryX - right)+ Mathf.Epsilon;
            float midSize = boundaryX * 2 - leftSize - rightSize;
            calculatedUV[0] = new Vector2(0f, 1f);
            calculatedUV[1] = new Vector2(0f, percentageTop);
            calculatedUV[2] = new Vector2(0f, percentageBot);
            calculatedUV[3] = new Vector2(0f, 0f);


            for(int row=4; row<verticiesArray.Count; row+=4) { 
                if(verticiesArray[row].x <= left) {
                    float positionInLowScale =  verticiesArray[row].x + boundaryX ;
                    float percentageInLowScale = positionInLowScale / leftSize; //dzielenie Przez jebane 0 kurwa
                    float percentageReal = Mathf.Lerp(0f, percentageLeft, percentageInLowScale);
                    calculatedUV[row] = new Vector2(percentageReal, 1f);
                    calculatedUV[row + 1] = new Vector2(percentageReal, percentageTop);
                    calculatedUV[row + 2] = new Vector2(percentageReal, percentageBot);
                    calculatedUV[row + 3] = new Vector2(percentageReal, 0f);
                }else if (verticiesArray[row].x >= right) {
                    float positionInLowScale = verticiesArray[row].x - right;
                    float percentageInLowScale = positionInLowScale / rightSize;
                    float percentageReal = Mathf.Lerp(percentageRight, 1f, percentageInLowScale);
                    calculatedUV[row] = new Vector2(percentageReal, 1f);
                    calculatedUV[row + 1] = new Vector2(percentageReal, percentageTop);
                    calculatedUV[row + 2] = new Vector2(percentageReal, percentageBot);
                    calculatedUV[row + 3] = new Vector2(percentageReal, 0f);
                }else {
                    float positionInLowScale = verticiesArray[row].x - left;
                    float percentageInLowScale = positionInLowScale / midSize;
                    float percentageReal = Mathf.Lerp(percentageLeft, percentageRight, percentageInLowScale);
                    calculatedUV[row] = new Vector2(percentageReal, 1f);
                    calculatedUV[row + 1] = new Vector2(percentageReal, percentageTop);
                    calculatedUV[row + 2] = new Vector2(percentageReal, percentageBot);
                    calculatedUV[row + 3] = new Vector2(percentageReal, 0f);
                }

            }
            return calculatedUV;
        }
    }
}
