using UnityEngine;
using UnityEngine.UI;
namespace CurvedVRKeyboard {

    public class KeyboardItem: KeyboardComponent {

        private Text letter;
        //------Click-------
        private bool clicked = false;
        private float clickHoldTimer = 0f;
        private float clickHoldTimeLimit = 0.15f;

        //-----Materials-----
        [SerializeField, HideInInspector]
        private Material keyNormalMaterial;
        [SerializeField, HideInInspector]
        private Material keySelectedMaterial;
        [SerializeField, HideInInspector]
        private Material keyPressedMaterial;

        //--Mesh&Renderers---
        private SpaceMeshCreator meshCreator;
        public Renderer quadFront;
        private Renderer quadBack;
        private static readonly string QUAD_FRONT = "Front";
        private static readonly string QUAD_BACK = "Back";

        //---MaterialEnums---
        public enum KeyStateEnum {
            Normal, Selected, Pressed
        }




        public void Awake () {
            Init();
        }

        public void Init () {
            if(letter == null || quadFront == null || quadBack == null) {  // Check if initialized
                letter = gameObject.GetComponentInChildren<Text>();

                Transform back = transform.Find(QUAD_BACK);
                if (back)
                    quadBack = back.GetComponent<Renderer>();
                quadFront = transform.Find(QUAD_FRONT).GetComponent<Renderer>();
            }
        }

        /// <summary>
        /// Handle for hover function
        /// </summary>
        public void Hovering () {
            if(!clicked) {// Is not already being clicked?
                ChangeMaterial(keySelectedMaterial);
            } else {
                HoldClick();
            }
        }

        /// <summary>
        /// handle for click begining
        /// </summary>
        public void Click () {
            clicked = true;
            ChangeMaterial(keyPressedMaterial);
        }

        /// <summary>
        /// handle after click was started
        /// </summary>
        private void HoldClick () {
            ChangeMaterial(keyPressedMaterial);
            clickHoldTimer += Time.deltaTime;
            if(clickHoldTimer >= clickHoldTimeLimit) {// Check if time of click is over
                clicked = false;
                clickHoldTimer = 0f;
            }
        }

        /// <summary>
        /// Handle for hover over
        /// </summary>
        public void StopHovering () {
            ChangeMaterial(keyNormalMaterial);
        }

        /// <summary>
        /// Get value of key text
        /// </summary>
        /// <returns>value of key</returns>
        public string GetValue () {
            return letter.text;
        }

        /// <summary>
        /// Changes value of key text
        /// </summary>
        /// <param name="value"></param>
        public void SetKeyText ( string value ) {
            if(!letter.text.Equals(value)) {
                letter.text = value;
            }
        }

        /// <summary>
        /// Changes material on key
        /// </summary>
        /// <param name="material">material to be displayed</param>
        private void ChangeMaterial ( Material material ) {
            quadFront.sharedMaterial = material;
            if (quadBack)
                quadBack.sharedMaterial = material;
        }

        /// <summary>
        /// Changes materials on all keys
        /// </summary>
        /// <param name="keyNormalMaterial"></param>
        /// <param name="keySelectedMaterial"></param>
        /// <param name="keyPressedMaterial"></param>
        public void SetMaterials ( Material keyNormalMaterial, Material keySelectedMaterial, Material keyPressedMaterial ) {
            this.keyNormalMaterial = keyNormalMaterial;
            this.keySelectedMaterial = keySelectedMaterial;
            this.keyPressedMaterial = keyPressedMaterial;
        }

        /// <summary>
        /// Changes material for state
        /// </summary>
        /// <param name="materialEnum">state of which material will be changed</param>
        /// <param name="newMaterial">new material</param>
        public void SetMaterial ( KeyStateEnum materialEnum, Material newMaterial) {
            switch(materialEnum) {
                case KeyStateEnum.Normal:
                    keyNormalMaterial = newMaterial;
                    quadFront.sharedMaterial = newMaterial;
                    if (quadBack)
                        quadBack.sharedMaterial = newMaterial;
                    break;
                case KeyStateEnum.Selected:
                    keySelectedMaterial = newMaterial;
                    break;
                case KeyStateEnum.Pressed:
                    keyPressedMaterial = newMaterial;
                    break;
            }
        }

        /// <summary>
        /// Changes 'space' bar mesh
        /// </summary>
        /// <param name="creator"></param>
        public void ManipulateSpace ( KeyboardCreator creator, Sprite spaceTexture) {
            if(spaceTexture != null) {
                keyNormalMaterial = new Material(keyNormalMaterial);
                keyNormalMaterial.SetTexture("_MainTex", spaceTexture.texture);
                SetMaterial(KeyStateEnum.Normal, keyNormalMaterial);
                //  keySelectedMaterial.SetTexture(Shader.PropertyToID("_MainTex"), spaceTexture);
                //  keyPressedMaterial.SetTexture(Shader.PropertyToID("_MainTex"), spaceTexture);
                }
                //}else {
                //    SetMaterial(KeyStateEnum.Normal, creator.KeyNormalMaterial);
                //}
                //if(meshCreator == null) {//lazy initialization
                meshCreator = new SpaceMeshCreator(creator,spaceTexture);
            // }
            if (quadBack)
                meshCreator.BuildFace(quadBack, false);
            meshCreator.BuildFace(quadFront, true);
        }

        public string GetMeshName () {
            if(quadFront == null) {
                Init();
            }
            return quadFront.GetComponent<MeshFilter>().sharedMesh.name;
        }
    }
}


