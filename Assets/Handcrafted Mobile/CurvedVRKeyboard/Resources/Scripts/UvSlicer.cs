
using System.Collections.Generic;
using UnityEngine;

namespace CurvedVRKeyboard {
    

    class UvSlicer {
        private Sprite spaceSprite;
        private Vector3 VerticalVector;

        public float left = 0.214f;
        public float right = 1 - 0.214f;
        public float top = 0.4f;
        public float bot = -0.4f;

        public float percentageLeft;
        public float percentageRight;
        public float percentageTop;
        public float percentageBot;

        private float virtualX ;
        private float virtualY ;

        public UvSlicer(Sprite spaceSprite ,int virtualX, int virtualY ) {
            this.spaceSprite = spaceSprite;
            this.virtualX = virtualX; 
            this.virtualY = virtualY;
            CalculateBorders(spaceSprite);

        }

        private void CalculateBorders ( Sprite spaceSprite ) {
            percentageLeft = ( spaceSprite.border.x / spaceSprite.bounds.size.x ) / 100f;
            left = (spaceSprite.border.x/virtualX * 4f) -2f;

            percentageRight = 1f - ( ( spaceSprite.border.z ) / spaceSprite.bounds.size.x ) / 100f;
            right = (1f - spaceSprite.border.z/virtualX) * 4f - 2f;

            percentageBot = ( spaceSprite.border.y / spaceSprite.bounds.size.y ) / 100f;
            bot = (spaceSprite.border.y )/ virtualY - 0.5f;

            percentageTop = 1f - ( ( spaceSprite.border.w ) / spaceSprite.bounds.size.y ) / 100f;
            top = (1f - spaceSprite.border.w / virtualY )- 0.5f;
            

            //TODO Change this fast fix to better sollution
            if(left == right) {
                left -=  1;
                right += 1;
            }
            if(top == bot) {
                top -= 1;
                bot += 1;
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
            if(right >= current && right <= current + spacing) {
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
            float leftSize = Mathf.Abs(left - (-boundaryX));
            float rightSize = Mathf.Abs(boundaryX - right);
            float midSize = boundaryX * 2 - leftSize - rightSize;
            calculatedUV[0] = new Vector2(0f, 1f);
            calculatedUV[1] = new Vector2(0f, percentageTop);
            calculatedUV[2] = new Vector2(0f, percentageBot);
            calculatedUV[3] = new Vector2(0f, 0f);
            int l = 0;
            int m = 0;
            int r = 0;
            for(int row=4; row<verticiesArray.Count; row+=4) {
                
                if(verticiesArray[row].x <= left) {
                    float positionInLowScale =  verticiesArray[row].x + boundaryX ;
                    float percentageInLowScale = positionInLowScale / leftSize;
                    float percentageReal = Mathf.Lerp(0f, percentageLeft, percentageInLowScale);
                    calculatedUV[row] = new Vector2(percentageReal, 1f);
                    calculatedUV[row + 1] = new Vector2(percentageReal, percentageTop);
                    calculatedUV[row + 2] = new Vector2(percentageReal, percentageBot);
                    calculatedUV[row + 3] = new Vector2(percentageReal, 0f);
                    l++;
                }else if (verticiesArray[row].x >= right) {
                    float positionInLowScale = verticiesArray[row].x - right;
                    float percentageInLowScale = positionInLowScale / rightSize;
                    float percentageReal = Mathf.Lerp(percentageRight, 1f, percentageInLowScale);
                    calculatedUV[row] = new Vector2(percentageReal, 1f);
                    calculatedUV[row + 1] = new Vector2(percentageReal, percentageTop);
                    calculatedUV[row + 2] = new Vector2(percentageReal, percentageBot);
                    calculatedUV[row + 3] = new Vector2(percentageReal, 0f);
                    r++;
                }else {
                    float positionInLowScale = verticiesArray[row].x - left;
                    float percentageInLowScale = positionInLowScale / midSize;
                    float percentageReal = Mathf.Lerp(percentageLeft, percentageRight, percentageInLowScale);
                    calculatedUV[row] = new Vector2(percentageReal, 1f);
                    calculatedUV[row + 1] = new Vector2(percentageReal, percentageTop);
                    calculatedUV[row + 2] = new Vector2(percentageReal, percentageBot);
                    calculatedUV[row + 3] = new Vector2(percentageReal, 0f);
                    m++;
                }

            }
            Debug.Log(string.Format("vars l:{0} m:{1} r:{2})", l, m, r));
            return calculatedUV;
        }
    }
}
