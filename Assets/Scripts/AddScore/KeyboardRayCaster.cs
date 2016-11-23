using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyboardRayCaster: KeyboardComponent {
    private Camera raycastingCamera;

    private float rayLength;
    private Ray ray;
    private RaycastHit hit;
    private LayerMask layer;

    private KeyboardStatus keyboardStatus;
    private KeyboardItem keyItemCurrent;

    private string clickInputName;

    void Start () {
        keyboardStatus = gameObject.GetComponent<KeyboardStatus>();
        layer = 1 << LayerMask.NameToLayer(LAYER_TO_CAST);
    }

    void Update () {
        RayCastKeyboard();
    }

    
    private void RayCastKeyboard () {
        ray = new Ray(raycastingCamera.transform.position, raycastingCamera.transform.forward);

        if(Physics.Raycast(ray, out hit, rayLength, layer)) { // if any key was hit
            KeyboardItem focusedKeyItem = hit.transform.gameObject.GetComponent<KeyboardItem>();

            if(focusedKeyItem != null) { 
                ChangeCurrentKeyItem(focusedKeyItem);
                keyItemCurrent.Hovering();

                // if key clicked
                if(Input.GetButtonDown(clickInputName)) {
                    keyItemCurrent.Click();
                    keyboardStatus.HandleClick(keyItemCurrent);
                }
            }
        }
        // if no target hit and lost focus on item
        else if(keyItemCurrent != null) {
            ChangeCurrentKeyItem(null);
        }
    }

    void ChangeCurrentKeyItem(KeyboardItem key) {
        if (keyItemCurrent != null)
            keyItemCurrent.StopHovering();
        keyItemCurrent = key;
    }

    public void SetRayLength (float rayLength) {
        this.rayLength = rayLength;
    }

    public void SetCamera(Camera pivot) {
        this.raycastingCamera = pivot;
    }

    public void SetClickButton(string clickHandler) {
        this.clickInputName = clickHandler;
    }
}
