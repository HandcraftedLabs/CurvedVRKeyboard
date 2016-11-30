using UnityEngine;


public class KeyboardRaycaster: KeyboardComponent {
    private Transform raycastingSource;
    private GameObject target;

    private float rayLength;
    private Ray ray;
    private RaycastHit hit;
    private LayerMask layer;

    private KeyboardStatus keyboardStatus;
    private KeyboardItem keyItemCurrent;

    private string clickInputName;

    void Start () {
        keyboardStatus = gameObject.GetComponent<KeyboardStatus>();
        int layerNumber  = gameObject.layer;
        layer = 1 << layerNumber;
    }

    void Update () {
        rayLength = Vector3.Distance(raycastingSource.position, target.transform.position) * 1.1f;
        RayCastKeyboard();
    }

    /// <summary>
    /// Checks if camera is pointing at any key. 
    /// If it does changes state of key
    /// </summary>
    private void RayCastKeyboard () {
        ray = new Ray(raycastingSource.position, raycastingSource.forward);

        if(Physics.Raycast(ray, out hit, rayLength, layer)) { // If any key was hit
            KeyboardItem focusedKeyItem = hit.transform.gameObject.GetComponent<KeyboardItem>();

            if(focusedKeyItem != null) { 
                ChangeCurrentKeyItem(focusedKeyItem);
                keyItemCurrent.Hovering();

                
                if(Input.GetButtonDown(clickInputName)) {// If key clicked
                    keyItemCurrent.Click();
                    keyboardStatus.HandleClick(keyItemCurrent);
                }
            }
        }
       
        else if(keyItemCurrent != null) {// If no target hit and lost focus on item
            ChangeCurrentKeyItem(null);
        }
    }
   private void ChangeCurrentKeyItem(KeyboardItem key) {
        if (keyItemCurrent != null) {
            keyItemCurrent.StopHovering();
        }
        keyItemCurrent = key;
    }

    public void SetRayLength (float rayLength) {
        this.rayLength = rayLength;
    }

    public void SetRaycastingTransform(Transform raycastingSource) {
        this.raycastingSource = raycastingSource;
    }

    public void SetClickButton(string clickHandler) {
        this.clickInputName = clickHandler;
    }
    public void SetTarget(GameObject target ) {
        this.target = target;
    }
}
