
using System.Collections.Generic;
using UnityEngine;

namespace CurvedVRKeyboard {

    /// <summary>
    /// Creates Keyboard, calculates all necessary positions and rotations
    /// </summary>
    [System.Serializable]
    [ExecuteInEditMode]
    public class KeyboardCreator: KeyboardComponent {

        //-----------SET IN UNITY --------------
        [SerializeField]
        private float curvature;
        [SerializeField]
        private Transform raycastingSource;
        [SerializeField]
        private string clickHandle;
        [SerializeField]
        private Material keyNormalMaterial;
        [SerializeField]
        private Material keySelectedMaterial;
        [SerializeField]
        private Material keyPressedMaterial;
        [SerializeField]
        private Sprite spaceSprite;
        [SerializeField]
        private KeyboardItem space;
        [SerializeField]
        private float referencedPixels = 1f;

        //-------private Calculations---------
        private readonly float defaultSpacingColumns = 56.3f;
        private readonly float defaultSpacingRows = 1.0f;
        private readonly float defaultRotation = 90f;
        public float centerPointDistance = -1f;
        private KeyboardItem[] keys;
        private int row;

        //--------------others----------------
        private ErrorReporter errorReporter;
        private const string MESH_NAME_SEARCHED = "Quad";
        public bool wasStaticOnStart;
        private const int spaceKeyNumber = 28;
        private const float radious = 3;

        //--------------borders of sprite  -----
        private float leftBorder;
        private float rightBorder;
        private float topBorder;
        private float bottomBorder;



        public void Awake () {
            InitKeys();
            ChangeMaterialOnKeys();
            if(!Application.isPlaying) {
                ManageKeys();
            }
            wasStaticOnStart = gameObject.isStatic;
            SetComponents();

        }



        public void ManageKeys () {
            checkErrors();

            if(!errorReporter.IsErrorPresent()) {
                if(centerPointDistance == -1f) {
                    CurvatureToDistance();
                }
                FillAndPlaceKeys();
            }
        }

        public void InitKeys () {
            if(keys == null || KeyboardItem.forceInit) {
                List<KeyboardItem> allKeys = new List<KeyboardItem>(GetComponentsInChildren<KeyboardItem>());
                for(int i = 0;i < allKeys.Count;i++) {
                    allKeys[i].Position = i;
                    allKeys[i].Init();
                }
                space = allKeys[spaceKeyNumber];
                keys = allKeys.ToArray();
            }
            space.ManipulateSpace(this, SpaceSprite);
        }

        /// <summary>
        /// Sets values for other necessary components
        /// </summary>
        private void SetComponents () {
            KeyboardRaycaster rayCaster = GetComponent<KeyboardRaycaster>();
            rayCaster.SetRaycastingTransform(RaycastingSource);
            rayCaster.SetClickButton(ClickHandle);
            rayCaster.SetTarget(gameObject);
            KeyboardStatus status = GetComponent<KeyboardStatus>();
            status.SetKeys(keys);
        }

        /// <summary>
        /// Fills key with text and calculates position 
        /// </summary>
        private void FillAndPlaceKeys () {
            foreach(KeyboardItem key in keys) {
                key.SetKeyText(KeyboardItem.KeyLetterEnum.LowerCase);
                PositionSingleLetter(key);
            }

        }

        /// <summary>
        /// Calculates whole transformation for single key
        /// </summary>
        /// <param name="iteration">index of key to be placed</param>
        /// <param name="keyTransform">key transformation</param>
        private void PositionSingleLetter ( KeyboardItem key ) {
            int iteration = key.Position;
            Transform keyTransform = key.transform;
            // Check row and how many keys were palced
            float keysPlaced = CalculateKeyOffsetAndRow(iteration);
            float degree = CalculateDeggreOfKey(lettersInRowsCount[row] - 1, iteration - keysPlaced);
            
            Vector3 PlacOnCircle = new Vector3(
                 Mathf.Cos(degree) * centerPointDistance,
                 row * -RowSpacing,
                 Mathf.Sin(degree) * centerPointDistance);

            key.transform.localPosition = PlacOnCircle;




            key.transform.RotateAround(Vector3.zero, Vector3.right, gameObject.transform.rotation.x);

            Vector3 LookAtTransform = new Vector3(
             transform.position.x + key.transform.localPosition.x * ( transform.lossyScale.x - 1 ),// - (transform.lossyScale.x  - 1 ) * transform.position.x *2 ,
             transform.position.y,
             transform.position.z + key.transform.localPosition.z * ( transform.lossyScale.z - 1 )) ;

            //LookAtTransform = LookAtTransform - transform.position;
            LookAtTransform = LookAtTransform - transform.position;
            LookAtTransform = transform.rotation * LookAtTransform;
            LookAtTransform = LookAtTransform + transform.position;


            key.transform.LookAt(LookAtTransform);
            if(key.gameObject.GetComponent<KeyboardItem>().Position == 15) {
                GameObject pivot = GameObject.Find("target");
                pivot.transform.position = LookAtTransform;

            }

            Vector3 MoveBackward = new Vector3(
                key.transform.localPosition.x,
                key.transform.localPosition.y,
                key.transform.localPosition.z - centerPointDistance + radious);

            key.transform.localPosition = MoveBackward;

            key.transform.localEulerAngles = new Vector3(0, key.transform.localEulerAngles.y, 0);
        }

        /// <summary>
        /// Applies transformation rotation to key in correct order 
        /// </summary>
        /// <param name="key">key to be transformed</param>
        private void RotationTransformation ( Transform key ) {
            //key.RotateAround(transform.position, Vector3.forward, transform.rotation.eulerAngles.z);
            //key.RotateAround(, Vector3.right, transform.rotation.eulerAngles.x);
            //key.RotateAround(transform.position, Vector3.up, transform.rotation.eulerAngles.y);
        }

        /// <summary>
        /// Makes key look at cylinder center
        /// </summary>
        /// <param name="keyTransform"></param>
        /// <param name="positionY"></param>
        private void LookAtTransformation ( Transform keyTransform, Vector3 position ) {
            float xPos = keyTransform.parent.position.x;
            float yPos = position.y;
            float zPos = 3 - centerPointDistance;
            
            //Debug.Log(zPos);
            Vector3 lookAt = new Vector3(xPos, yPos, zPos);
            
            keyTransform.LookAt(lookAt);
        }

        /// <summary>
        /// </summary>
        /// <param name="rowSize">size of current row</param>
        /// <param name="offset">position of letter in row</param>
        /// <returns>deggre of key</returns>
        public float CalculateDeggreOfKey ( float rowSize, float offset ) {
            return Mathf.Deg2Rad * ( defaultRotation + rowSize * SpacingBetweenKeys / 2 - offset * SpacingBetweenKeys );
        }

        /// <summary> 
        /// Calculates current row and offset of key
        /// </summary>
        /// <param name="iteration"></param>
        /// <returns></returns>
        private float CalculateKeyOffsetAndRow ( int iteration ) {
            float keysPlaced = 0;
            row = 0;
            int iterationCounter = 0;
            for(int rowChecked = 0;rowChecked <= 2;rowChecked++) {
                iterationCounter += lettersInRowsCount[rowChecked];
                if(iteration >= iterationCounter) {
                    keysPlaced += lettersInRowsCount[rowChecked];
                    row++;
                }
            }
            //last row with space requires special calculations
            if(iteration >= iterationCounter) {
                const float offsetBetweenSpecialKeys = 1.5f;
                keysPlaced -= ( iteration - iterationCounter ) * offsetBetweenSpecialKeys;
            }
            return keysPlaced;
        }

        /// <summary>
        /// tan (x * 1,57) - tan is in range of <0,3.14>. With
        /// this approach we can scale it to range <0(0),1(close to infinity)>.
        /// Why + radious = 3?? because virtual radious of our circle is 3 
        /// google (tan(x*1.57) + 3)
        /// Higher values make center position further from keys (straight line)
        /// </summary>
        private void CurvatureToDistance () {
            centerPointDistance = Mathf.Tan(curvature * 1.57f) + radious;
        }

        /// <summary>
        /// Changes materials for all keys
        /// </summary>
        public void ChangeMaterialOnKeys () {
            foreach(KeyboardItem key in keys) {
                key.SetMaterials(KeyNormalMaterial, KeySelectedMaterial, KeyPressedMaterial);
            }
        }

        public void checkErrors () {
            errorReporter = ErrorReporter.Instance;
            errorReporter.Reset();
            if(keys.Length != KEY_NUMBER) {//is there correct number of keys
                errorReporter.SetMessage("Cannot procced. Number of keys is incorrect. Revert your changes to prefab", ErrorReporter.Status.Error);
                return;
            }
            if(space == null) { // project improted over older package crashes without this 
                space = keys[spaceKeyNumber];
            }
            if(space.GetMeshName().Equals(MESH_NAME_SEARCHED)) {//are keys positioned corectly
                errorReporter.SetMessage("Cannot  procced. Space key data is incorrect. Revert your changes to prefab or place keys in correct sequence", ErrorReporter.Status.Error);
                return;
            }
            if(!gameObject.GetComponent<KeyboardStatus>().isReflectionPossible) {
                errorReporter.SetMessage("GameObject Output is not set, or there is no script with \"text\" property on current gameobject", ErrorReporter.Status.Warning);
                return;
            }
            if(wasStaticOnStart && Application.isPlaying) {//is playing and was static when play mode started
                errorReporter.SetMessage("If editng during gameplay is necessary, quit gameplay and remove static flag from keyboard and its children."
                    + " Reamember to set keyboard to static when building", ErrorReporter.Status.Info);
                return;
            }
            CheckKeyArrays();
        }

        /// <summary>
        /// When spacebar material is set it is created as a new material so the reference 
        /// to buttons' material is lost and changing them do not affect spacebar. 
        /// User has to manualy reload material if he changed them in editor
        /// </summary>
        public void ReloadSpaceMaterials () {
            space.SetMaterials(KeyNormalMaterial, KeySelectedMaterial, KeyPressedMaterial);
            space.ManipulateSpace(this, SpaceSprite);
        }


        //---------------PROPERTIES----------------
        public float Curvature {
            get {
                return 1f - curvature;
            }
            set {
                const float errorThreshold = 0.01f;
                //if(Mathf.Abs(curvature - ( 1f - value )) >= errorThreshold) {// Value changed
                    curvature = 1f - value;
                    CurvatureToDistance();
                    ManageKeys();
                    space.ManipulateSpace(this, spaceSprite);
               //} 
            }
        }

        public float SpacingBetweenKeys {
            get {
                return defaultSpacingColumns / centerPointDistance;
            }
        }

        public float RowSpacing {
            get {
                return defaultSpacingRows ;
            }
        }


        public Material KeyNormalMaterial {
            get {
                return keyNormalMaterial;
            }
            set {
                if(KeyNormalMaterial != value) {
                    keyNormalMaterial = value;
                    foreach(KeyboardItem key in keys) {
                        key.SetMaterial(KeyboardItem.KeyMaterialEnum.Normal, keyNormalMaterial);
                    }
                }
            }
        }

        public Material KeySelectedMaterial {
            get {
                return keySelectedMaterial;
            }
            set {
                if(keySelectedMaterial != value) {
                    keySelectedMaterial = value;
                    foreach(KeyboardItem key in keys) {
                        key.SetMaterial(KeyboardItem.KeyMaterialEnum.Selected, keySelectedMaterial);
                    }
                }

            }
        }

        public Material KeyPressedMaterial {
            get {
                return keyPressedMaterial;
            }
            set {
                if(KeyPressedMaterial != value) {
                    keyPressedMaterial = value;
                    foreach(KeyboardItem key in keys) {
                        key.SetMaterial(KeyboardItem.KeyMaterialEnum.Pressed, keyPressedMaterial);
                    }
                }
            }
        }

        public Sprite SpaceSprite {
            get {
                return spaceSprite;
            }
            set {
                //if there was a sprite and now it changed to null
                if(spaceSprite != value && value == null) {
                    spaceSprite = value;
                    space.ManipulateSpace(this, SpaceSprite);
                    space.SetMaterials(KeyNormalMaterial, KeySelectedMaterial, KeyPressedMaterial);
                }
                //if value has changed and it's not null 
                else if(value != null) {
                    if(SpaceSprite != value || AreBordersChanged(value)) {//if new or borders changed
                        spaceSprite = value;
                        ChangeBorders(SpaceSprite.border);
                        space.ManipulateSpace(this, SpaceSprite);
                        space.SetMaterials(KeyNormalMaterial, KeySelectedMaterial, KeyPressedMaterial);
                    }
                }
            }
        }


        public Transform RaycastingSource {
            get {
                return raycastingSource;
            }
            set {
                if(raycastingSource != value) {
                    InitKeys();
                    raycastingSource = value;
                    KeyboardRaycaster rayCaster = GetComponent<KeyboardRaycaster>();
                    rayCaster.SetRaycastingTransform(RaycastingSource);
                }
            }
        }

        public string ClickHandle {
            get {
                return clickHandle;
            }
            set {
                clickHandle = value;
                KeyboardRaycaster rayCaster = GetComponent<KeyboardRaycaster>();
                rayCaster.SetClickButton(clickHandle);
            }
        }

        public float ReferencedPixels {
            get {
                return referencedPixels;
            }
            set {
                if(ReferencedPixels != value) {
                    referencedPixels = value <= 0.01f ? 0.01f : value;
                    space.ManipulateSpace(this, SpaceSprite);
                }
            }
        }
        /// <summary>
        ///  Borders setup changes cannot be automatically detected so we have to do this manually
        /// </summary>
        /// <param name="newBorder"></param>
        private bool AreBordersChanged (Sprite newSprite) {
            Vector4 newBorder = newSprite.border;
            if(leftBorder != newBorder.x || bottomBorder != newBorder.y || rightBorder != newBorder.z || topBorder != newBorder.w) {
                ChangeBorders(newBorder);
                return true;
            }
            return false;
        }

        private void ChangeBorders ( Vector4 newBorder ) {
            leftBorder = newBorder.x;
            bottomBorder = newBorder.y;
            rightBorder = newBorder.z;
            topBorder = newBorder.w;
        }
    }
}



