
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

        public UvSlicer(Sprite spaceSprite ) {
            this.spaceSprite = spaceSprite;
            CalculateBorders(spaceSprite);

        }

        private void CalculateBorders ( Sprite spaceSprite ) {
            percentageLeft = ( spaceSprite.border.x / spaceSprite.bounds.size.x ) / 100f;
            left = percentageLeft * 4f - 2f;
            percentageRight = 1f - ( ( spaceSprite.border.z ) / spaceSprite.bounds.size.x ) / 100f;
            right = percentageRight * 4f - 2f;
            percentageBot = ( spaceSprite.border.y / spaceSprite.bounds.size.y ) / 100f;
            bot = percentageBot - 0.5f;
            percentageTop = 1f - ( ( spaceSprite.border.w ) / spaceSprite.bounds.size.y ) / 100f;
            top = percentageTop - 0.5f;
        }


        public bool CheckVerticalBorders (float current,float spacing) {
            if(current < left && left < current + spacing) { // left border shall be added??
                VerticalVector = new Vector3(left, 0, 0);
                return true;
            }
            if(right > current && right < current + spacing) {
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
            float leftSize = Mathf.Abs(-boundaryX - left);
            float midSize = boundaryX - Mathf.Abs(left) - Mathf.Abs(right);
            calculatedUV[0] = new Vector2(0f, 1f);
            calculatedUV[1] = new Vector2(0f, percentageTop);
            calculatedUV[2] = new Vector2(0f, percentageBot);
            calculatedUV[3] = new Vector2(0f, 0f);

            for(int row=4; row<verticiesArray.Count; row+=4) {
                
                if(verticiesArray[row].x <= left) {
                    float positionInLowScale =  verticiesArray[row].x + boundaryX ;
                    float percentageInLowScale = positionInLowScale / leftSize;
                    float percentageReal = percentageInLowScale * percentageLeft;
                    calculatedUV[row] = new Vector2(percentageReal, 1f);
                    calculatedUV[row + 1] = new Vector2(percentageReal, percentageTop);
                    calculatedUV[row + 2] = new Vector2(percentageReal, percentageBot);
                    calculatedUV[row + 3] = new Vector2(percentageReal, 0f);
                }else if (verticiesArray[row].x <= right) {
                    float percentageReal = ( verticiesArray[row].x + 2 ) / 4;
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
