using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyboardRayCaster: KeyboardComponent {





    


    private Camera cam;

    private float rayLength;
    private Ray ray;
    private RaycastHit hit;
    private LayerMask layer;

    private KeyboardStatus keyboardStatus;
    private KeyboardItem kitemCurrent;

    void Start () {
        keyboardStatus = gameObject.GetComponent<KeyboardStatus>();
        layer = 1 << LayerMask.NameToLayer(LAYER_TO_CAST);
    }

    void Update () {
        RayCastKeyboard();
    }

    
    private void RayCastKeyboard () {
        ray = new Ray(cam.transform.position, cam.transform.forward);
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
            if(Input.GetMouseButtonDown(0)) {
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

    public void SetCamera(Camera cam ) {
        this.cam = cam;
    }
}
