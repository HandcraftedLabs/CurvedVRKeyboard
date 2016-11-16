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
    private KeyboardItem kitemCurrent;

    private string clickHandle;

    void Start () {
        keyboardStatus = gameObject.GetComponent<KeyboardStatus>();
        layer = 1 << LayerMask.NameToLayer(LAYER_TO_CAST);
    }

    void Update () {
        RayCastKeyboard();
    }

    
    private void RayCastKeyboard () {
        ray = new Ray(raycastingCamera.transform.position, raycastingCamera.transform.forward);
        //if somthing was hit
        if(Physics.Raycast(ray, out hit, rayLength, layer)) {
            KeyboardItem focusedKitem = hit.transform.gameObject.GetComponent<KeyboardItem>();
            
            //if there is none current
            if(kitemCurrent == null) {
                kitemCurrent = focusedKitem;
            }

            //if previous is different
            if(focusedKitem != kitemCurrent) {
                kitemCurrent.stopHovering();
                kitemCurrent = focusedKitem;
            }

            kitemCurrent.hovering();
            //if clicked
            if(Input.GetButtonDown(clickHandle)) {
                kitemCurrent.click();
                keyboardStatus.HandleClick(kitemCurrent);
            }



        }
        //if no target hit and lost focus on item
        else if(kitemCurrent != null) {
            kitemCurrent.stopHovering();
            kitemCurrent = null;
        }
    }

    public void SetRayLength (float rayLength) {
        this.rayLength = rayLength;
    }

    public void SetCamera(Camera pivot ) {
        this.raycastingCamera = pivot;
    }

    public void SetClickButton(string clickHandler ) {
        this.clickHandle = clickHandler;
    }
}
