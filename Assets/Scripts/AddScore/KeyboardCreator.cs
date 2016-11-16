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
    private float spaceKeyOffsetRotation;
    [SerializeField]
    private Material keyDefaultMaterial;
    [SerializeField]
    private Material keyHoldMaterial;
    [SerializeField]
    private Material keyPressedMaterial;

    //-----------SET IN UNITY --------------

    private KeyboardItem[] keys;
    private float row;
    private static string name;

    private static bool wasDisabled = false;
    public static GameObject gameobjectCopy;

    //-------private Calculations--------
    private readonly float defaultSpaceingColumns = 56.3f;
    private readonly float defaultSpacingRows = 0.96f;
    private readonly float defaultRotation = 90f;


    public void Start () {
        CheckExistance();
        ManageKeys();
        ChangeMaterialOnKeys();
    }

    public void ManageKeys () {
        if(keys == null) {
            keys = gameobjectCopy.GetComponentsInChildren<KeyboardItem>();
        }
        FillAndPlaceKeys();
    }


    private void SetComponents () {
        KeyboardRayCaster rayCaster = gameobjectCopy.GetComponent<KeyboardRayCaster>();
        rayCaster.SetRayLength(CurvatureToDistance() + 1f);
        rayCaster.SetCamera(RaycastingCamera);
        rayCaster.SetClickButton(ClickHandle);
        KeyboardStatus status = gameobjectCopy.GetComponent<KeyboardStatus>();
        status.SetKeys(keys);
    }

    //gameobject can be destroyed when we are stoping gameplay
    //(we are using gameobject out of gameplay also to manupulate it on scene)
    //since the gameobject can't be set by "gameobject ==" we need a copy
    //of gameobject. When gameobject is destroied we assign new copy
    //if somone would refer to gameobject error would be thrown
    public void CheckExistance () {
        if(wasDisabled) {
            wasDisabled = false;
            gameobjectCopy = GameObject.Find(name);
        }
        //gameobject name changed or no name asigned yet
        if(gameobjectCopy == null || name == null || !name.Equals(gameobjectCopy.name)) {
            gameobjectCopy = gameObject;
            name = gameObject.name;
        }
        SetComponents();
    }

    private void FillAndPlaceKeys () {
        for(int i = 0;i < keys.Length;i++) {
            keys[i].Init();
            keys[i].setLetterText(allLetters[i]);
            PositionSingleLetter(i, keys[i].gameObject.transform);
        }
    }

    private void PositionSingleLetter ( int iteration, Transform keyTrnsform ) {
        //check row and how many keys were palced
        float keysPlaced = CalculateKeysPlacedAndRow(iteration);
        Vector3 position = CalculatePositionCirlce(rowLetters[(int)row], iteration - keysPlaced);
        position = AdditionalTransformations(keyTrnsform, position);
        LookAtTransformations(keyTrnsform, position);
        RotationTransformations(keyTrnsform);

    }

    private void RotationTransformations ( Transform keyTrnsform ) {
        keyTrnsform.RotateAround(gameobjectCopy.transform.position, Vector3.forward, gameobjectCopy.transform.rotation.eulerAngles.z);
        keyTrnsform.RotateAround(gameobjectCopy.transform.position, Vector3.right, gameobjectCopy.transform.rotation.eulerAngles.x);
        keyTrnsform.RotateAround(gameobjectCopy.transform.position, Vector3.up, gameobjectCopy.transform.rotation.eulerAngles.y);
    }

    private void LookAtTransformations ( Transform keyTrnsform, Vector3 position ) {
        Vector3 lookAT = new Vector3(gameobjectCopy.transform.position.x, position.y, gameobjectCopy.transform.position.z - ( CurvatureToDistance() * gameobjectCopy.transform.localScale.x ));
        keyTrnsform.LookAt(lookAT);
    }

    private Vector3 AdditionalTransformations ( Transform keyTrnsform, Vector3 position ) {
        //radiousCalculations
        position += gameobjectCopy.transform.position;
        position.z -= CurvatureToDistance();
        //scaleCalculations
        position = position + ( ( -gameobjectCopy.transform.position + position ) * ( gameobjectCopy.transform.localScale.x - 1 ) );
        position.y = position.y / gameobjectCopy.transform.localScale.x;

        keyTrnsform.position = position;
        return position;
    }

    private Vector3 CalculatePositionCirlce ( float rowSize, float offset ) {
        //row size - offset of current letter position
        float degree = Mathf.Deg2Rad * ( defaultRotation + rowSize * SpacingBetweenKeys/2 - offset * SpacingBetweenKeys);

        float x = Mathf.Cos(degree) * CurvatureToDistance();
        float z = Mathf.Sin(degree) * CurvatureToDistance();
        return new Vector3(x, -row * RowSpacing, z);
    }

    // After getting to new row we need to reset offset relative to iteration
    // so all keys spawn in front of user centered
    private float CalculateKeysPlacedAndRow ( int iteration ) {
        float keysPlaced = 0;
        if(iteration < rowLetters[0]) {//if is in firstrow
            keysPlaced = 0;
            row = 0;
        } else if(iteration < rowLetters[0] + rowLetters[1]) {//if is secondrow
            keysPlaced = rowLetters[0];
            row = 1;
        } else if(iteration < rowLetters[0] + rowLetters[1] + rowLetters[2]) {//thirdrow
            keysPlaced = rowLetters[0] + rowLetters[1];
            row = 2;
        }
        //now are special signs they need to be set manually (their offset) 
        else if(iteration == rowLetters[0] + rowLetters[1] + rowLetters[2]) {//?!#
            keysPlaced = rowLetters[0] + rowLetters[1] + rowLetters[2];
        } else if(iteration == rowLetters[0] + rowLetters[1] + rowLetters[2] + 1) {//space
            keysPlaced = rowLetters[0] + rowLetters[1] + rowLetters[2] - 1.5f;
        } else if(iteration == rowLetters[0] + rowLetters[1] + rowLetters[2] + 2) { // backspace
            keysPlaced = rowLetters[0] + rowLetters[1] + rowLetters[2] - 3f;
        }
        //set third row for special keys
        if(iteration >= rowLetters[0] + rowLetters[1] + rowLetters[2]) {
            row = 3;
        }
        return keysPlaced;
    }

    public void OnDisable () {
        wasDisabled = true;
    }



    private void ChangeMaterialOnKeys () {
        foreach(KeyboardItem key in keys) {
            key.setMaterials(KeyDefaultMaterial, KeyHoldMaterial, KeyPressedMaterial);
        }
    }


    private float CurvatureToDistance () {
        return Mathf.Tan(curvature *1.57f) + 3;
        
    }



    //---------------PROPERTIES----------------



    public float Curvature {
        get {
            return curvature;
        }
        set {
            if(curvature != value) {
                curvature = value;                
                ManageKeys();
            }

        }
    }

    public float SpacingBetweenKeys {
        get {
            return defaultSpaceingColumns / CurvatureToDistance() ;
        }
    }

    public float RowSpacing {
        get {
            return defaultSpacingRows * gameobjectCopy.transform.localScale.y;
        }
    }


    public Material KeyDefaultMaterial {
        get {
            return keyDefaultMaterial;
        }

        set {
            if(KeyDefaultMaterial != value) {
                keyDefaultMaterial = value;
                ChangeMaterialOnKeys();
            }

        }
    }

    public Material KeyHoldMaterial {
        get {
            return keyHoldMaterial;
        }

        set {
            if(keyHoldMaterial != value) {
                keyHoldMaterial = value;
                ChangeMaterialOnKeys();
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
                ChangeMaterialOnKeys();
            }

        }
    }

    public Camera RaycastingCamera {
        get {
            return raycastingCamera;
        }

        set {
            if(raycastingCamera != value) {
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



