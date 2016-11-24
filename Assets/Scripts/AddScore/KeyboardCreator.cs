using UnityEngine;
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
    private Camera raycastingCamera;
    [SerializeField]
    private string clickHandle;
    [SerializeField]
    private Material keyDefaultMaterial;
    [SerializeField]
    private Material keyHoverMaterial;
    [SerializeField]
    private Material keyPressedMaterial;

    //-----------SET IN UNITY --------------

    private KeyboardItem[] keys;
    private int row;

    //-------private Calculations--------
    private readonly float defaultSpacingColumns = 56.3f;
    private readonly float defaultSpacingRows = 1.0f;
    private readonly float defaultRotation = 90f;
    public float centerPointDistance = -1f;

    private static readonly string MESH_NAME_SEARCHED = "Quad";


    public void Start () {
        ManageKeys();
        ChangeMaterialOnKeys();
        SetComponents();
    }
    public void ManageKeys () {
        if(keys == null) { 
             InitKeys();
        }
        if(CanBuild()) {
            if(centerPointDistance == -1f) {
                CurvatureToDistance();
            }
            FillAndPlaceKeys();
        }
    }

    public void InitKeys () {
        keys = GetComponentsInChildren<KeyboardItem>();
    }
    
    /// <summary>
    /// Sets values for other necessary components
    /// </summary>
    private void SetComponents () {
        KeyboardRayCaster rayCaster = GetComponent<KeyboardRayCaster>();
        rayCaster.SetCamera(RaycastingCamera);
        rayCaster.SetClickButton(ClickHandle);
        rayCaster.SetTarget(gameObject);
        KeyboardStatus status = GetComponent<KeyboardStatus>();
        status.SetKeys(keys);
    }

    /// <summary>
    /// Fills key with text and calculates position 
    /// </summary>
    private void FillAndPlaceKeys () {
        for(int i = 0;i < keys.Length;i++) {
            keys[i].Init();
            keys[i].SetKeyText(allLettersLowercase[i]);
            PositionSingleLetter(i, keys[i].gameObject.transform);
            if(i == 28) {// Space key
                keys[i].ManipulateMesh(this);
            }
        }
    }
    /// <summary>
    /// Calculates whole transformation for single key
    /// </summary>
    /// <param name="iteration">index of key to be placed</param>
    /// <param name="keyTransform">key transformation</param>
    private void PositionSingleLetter ( int iteration, Transform keyTransform ) {
        // Check row and how many keys were palced
        float keysPlaced = CalculateKeysPlacedAndRow(iteration);
        Vector3 positionOnCylinder = CalculatePositionOnCylinder(lettersInRowsCount[(int)row] -1, iteration - keysPlaced);
        positionOnCylinder = AdditionalTransformations(keyTransform, positionOnCylinder);
        LookAtTransformations(keyTransform, positionOnCylinder.y);
        RotationTransformations(keyTransform);

    }
    /// <summary>
    /// Applies transformation rotation to key in correct order 
    /// </summary>
    /// <param name="keyTransform">key to be transformed</param>
    private void RotationTransformations ( Transform keyTransform ) {
        keyTransform.RotateAround(transform.position, Vector3.forward, transform.rotation.eulerAngles.z);
        keyTransform.RotateAround(transform.position, Vector3.right, transform.rotation.eulerAngles.x);
        keyTransform.RotateAround(transform.position, Vector3.up, transform.rotation.eulerAngles.y);
    }

    /// <summary>
    /// Makes key look at cylinder center
    /// </summary>
    /// <param name="keyTransform"></param>
    /// <param name="positionY"></param>
    private void LookAtTransformations ( Transform keyTransform, float positionY ) {
        float xPos = transform.position.x;
        float yPos = positionY;
        float zOffset = ( centerPointDistance * transform.localScale.x );
        float zPos = transform.position.z - zOffset;
        Vector3 lookAt = new Vector3(xPos, yPos , zPos);
        keyTransform.LookAt(lookAt);
    }
    /// <summary>
    /// Applies transformation gameobject(whole keyboard) to each key
    /// </summary>
    /// <param name="keyTransform">key to transform</param>
    /// <param name="positionOnCylinder">position on cylinder</param>
    /// <returns></returns>
    private Vector3 AdditionalTransformations ( Transform keyTransform, Vector3 positionOnCylinder ) {
        positionOnCylinder += transform.position;
        positionOnCylinder.z -= centerPointDistance;
        float yPositionBackup = positionOnCylinder.y;
        Vector3 fromCenterToKey = ( positionOnCylinder - transform.position );
        float scaleOfX = ( transform.localScale.x - 1 ) ;

        //we move each key along it backward direction by scale
        positionOnCylinder = positionOnCylinder + fromCenterToKey * scaleOfX;
        
        //we modified y in upper calculations restore it 
        positionOnCylinder.y = yPositionBackup;

        keyTransform.position = positionOnCylinder;

        return positionOnCylinder;
    }
    /// <summary>
    /// Calculates position of key
    /// </summary>
    /// <param name="rowSize">size of current row</param>
    /// <param name="offset">position of letter in row</param>
    /// <returns>Position of key</returns>
    public Vector3 CalculatePositionOnCylinder ( float rowSize, float offset ) {
        
        float degree = Mathf.Deg2Rad * ( defaultRotation + rowSize * SpacingBetweenKeys/2 - offset * SpacingBetweenKeys);

        float x = Mathf.Cos(degree) * centerPointDistance;
        float z = Mathf.Sin(degree) * centerPointDistance;
        float y = -row * RowSpacing;
        return new Vector3(x, y, z);
    }

    /// <summary> 
    /// Calculates current row and offset of key
    /// </summary>
    /// <param name="iteration"></param>
    /// <returns></returns>
    private float CalculateKeysPlacedAndRow ( int iteration ) {
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
    /// tan (x * 1,57) - tan is in range of <0,3.14>, With
    /// this approach we can scale it to range <0(0),1(close to infinity)>.
    /// + 3 - without tangent at value of 3 (minimum)
    /// keyboard has 180 degree curve higher values make center
    /// be further from keys (straight line)
    /// </summary>
    private void CurvatureToDistance () {
        centerPointDistance = Mathf.Tan(curvature *1.57f) + 3;
    }

    /// <summary>
    /// Changes materials for all keys
    /// </summary>
    public void ChangeMaterialOnKeys () {
        foreach(KeyboardItem key in keys) {
            key.SetMaterials(KeyDefaultMaterial, KeyHoveringMaterial, KeyPressedMaterial);
        }
    }

    /// <summary>
    /// Checks if user didn't delete any items.
    /// Also rises warnings
    /// </summary>
    /// <returns></returns>
    public bool CanBuild () {
        if(keys.Length != 30) {//is there correct number of keys
            Debug.LogWarning("Can't procced. Number of keys is incorrect. Revert your changes to prefab");
            return false;
        }
        if(keys[28].GetMeshName().Equals(MESH_NAME_SEARCHED)) {//are keys positioned corectly
            Debug.LogWarning("Can't procced. Space key data is incorrect. Revert your changes to prefab or place keys in correct sequence");
            return false;
        }
        if(GetComponent<KeyboardStatus>().output == null) { // is output text field set
            Debug.LogWarning("Please set output Text");
        }
        return true;
    }
    //---------------PROPERTIES----------------



    public float Curvature {
        get {
            return 1f - curvature;
        }
        set {
            const float errorThreshold = 0.01f;
            if(Mathf.Abs(curvature - ( 1f - value )) >= errorThreshold) {// Value changed

                curvature = 1f - value;
                CurvatureToDistance();
                ManageKeys();

            }
        }
    }

    public float SpacingBetweenKeys {
        get {
            return defaultSpacingColumns / centerPointDistance ;
        }
    }

    public float RowSpacing {
        get {
            return defaultSpacingRows * transform.localScale.y;
        }
    }


    public Material KeyDefaultMaterial {
        get {
            return keyDefaultMaterial;
        }
        set {
            if(KeyDefaultMaterial != value) {
                keyDefaultMaterial = value;
                foreach(KeyboardItem key in keys) {
                    key.SetMaterial(KeyboardItem.KeyStateEnum.Default, keyDefaultMaterial);
                }

            }
        }
    }

    public Material KeyHoveringMaterial {
        get {
            return keyHoverMaterial;
        }
        set {
            if(keyHoverMaterial != value) {
                keyHoverMaterial = value;
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
            }

        }
    }

    public Camera RaycastingCamera {
        get {
            return raycastingCamera;
        }
        set {
            if(raycastingCamera != value) {
                InitKeys();
                raycastingCamera = value;
            }

        }
    }

    public string ClickHandle {
        get {
            return clickHandle;
        }
        set {
            clickHandle = value;
        }
    }

}



