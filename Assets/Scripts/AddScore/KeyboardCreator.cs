using UnityEngine;

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
        if(IsWithoutErrors()) {
            if(centerPointDistance == -1f) {
                CurvatureToDistance();
            }
            FillAndPlaceKeys();
        }
    }

    public void InitKeys () {
        keys = GetComponentsInChildren<KeyboardItem>();
    }

    private void SetComponents () {
        KeyboardRayCaster rayCaster = GetComponent<KeyboardRayCaster>();
        //TODO Later update it to detect furthest point on update method
        rayCaster.SetRayLength(50.0f);
        rayCaster.SetCamera(RaycastingCamera);
        rayCaster.SetClickButton(ClickHandle);
        KeyboardStatus status = GetComponent<KeyboardStatus>();
        status.SetKeys(keys);
    }


    private void FillAndPlaceKeys () {
        for(int i = 0;i < keys.Length;i++) {
            keys[i].Init();
            keys[i].SetKeyText(allLettersLowercase[i]);
            PositionSingleLetter(i, keys[i].gameObject.transform);
            if(i == 28) {
                keys[i].ManipulateMesh(this);
            }
        }
    }

    private void PositionSingleLetter ( int iteration, Transform keyTransform ) {
        //check row and how many keys were palced
        float keysPlaced = CalculateKeysPlacedAndRow(iteration);
        Vector3 position = CalculatePositionOnCylinder(lettersInRowsCount[(int)row] -1, iteration - keysPlaced);
        position = AdditionalTransformations(keyTransform, position);
        LookAtTransformations(keyTransform, position.y);
        RotationTransformations(keyTransform);

    }

    private void RotationTransformations ( Transform keyTransform ) {
        keyTransform.RotateAround(transform.position, Vector3.forward, transform.rotation.eulerAngles.z);
        keyTransform.RotateAround(transform.position, Vector3.right, transform.rotation.eulerAngles.x);
        keyTransform.RotateAround(transform.position, Vector3.up, transform.rotation.eulerAngles.y);
    }

    private void LookAtTransformations ( Transform keyTransform, float positionY ) {
        float xPos = transform.position.x;
        float yPos = positionY;
        float zOffset = ( centerPointDistance * transform.localScale.x );
        float zPos = transform.position.z - zOffset;
        Vector3 lookAt = new Vector3(xPos, yPos , zPos);
        keyTransform.LookAt(lookAt);
    }

    private Vector3 AdditionalTransformations ( Transform keyTransform, Vector3 keyPosition ) {
        keyPosition += transform.position;
        keyPosition.z -= centerPointDistance;
        float yPositionBackup = keyPosition.y;
        Vector3 fromCenterToKey = ( keyPosition - transform.position );
        float scaleOfX = ( transform.localScale.x - 1 ) ;

        //we move each key along it backward direction by scale
        keyPosition = keyPosition + fromCenterToKey * scaleOfX;
        
        //we modified y in upper calculations restore it 
        keyPosition.y = yPositionBackup;

        keyTransform.position = keyPosition;

        return keyPosition;
    }

    public Vector3 CalculatePositionOnCylinder ( float rowSize, float offset ) {
        //row size - offset of current letter position
        float degree = Mathf.Deg2Rad * ( defaultRotation + rowSize * SpacingBetweenKeys/2 - offset * SpacingBetweenKeys);

        float x = Mathf.Cos(degree) * centerPointDistance;
        float z = Mathf.Sin(degree) * centerPointDistance;
        float y = -row * RowSpacing;
        return new Vector3(x, y, z);
    }

    // After getting to new row we need to reset offset relative to iteration
    // so all keys spawn in front of user centered
    private float CalculateKeysPlacedAndRow ( int iteration ) {
        float keysPlaced = 0;
        row = 0;

        int iterationCounter = 0;
        for(int rowChecked = 0;rowChecked <= 2;rowChecked++) {//for each row
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

    public void ChangeMaterialOnKeys () {
        foreach(KeyboardItem key in keys) {
            key.SetMaterials(KeyDefaultMaterial, KeyHoveringMaterial, KeyPressedMaterial);
        }
    }


    public bool IsWithoutErrors () {
        if(keys.Length != 30) {//is there correct number of keys
            Debug.LogWarning("Can't procced. Number of keys is incorrect. Revert your changes to prefab");
            return false;
        }
        if(keys[28].GetMeshName().Equals(MESH_NAME_SEARCHED)) {//if keys are positioned corectly
            Debug.LogWarning("Can't procced. Space key data is incorrect. Revert your changes to prefab or place keys in correct sequence");
            return false;
        }
        if(GetComponent<KeyboardStatus>().output == null) {
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
            // THERE IS ERROR IN IF LEAVE FOR LATER
            //Debug.Log(curvature);
            //Debug.Log(1f-value);
            if(curvature != 1f - value) {
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
                    key.SetMaterial(KeyboardItem.MaterialEnum.Default, keyDefaultMaterial);
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
    
    public void RebuildMesh () {
        keys[keys.Length - 2].ManipulateMesh(this);
    }
}



