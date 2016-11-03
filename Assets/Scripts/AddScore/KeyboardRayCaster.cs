using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyboardRayCaster: KeyboardComponent {





    //-----------SET IN UNITY --------------
    public Camera cam;
    public float rayLength;
    public KeyboardStatus keyboardStatus;
    //-----------SET IN UNITY --------------

    private Ray ray;
    private RaycastHit hit;
    private LayerMask layer;


    private GameObject keyboard;
    private KeyboardItem kitemprevious;
    private KeyboardItem kitemCurrent;

    void Start () {
        layer = 1 << LayerMask.NameToLayer(LAYER_TO_CAST);
        keyboard = GameObject.Find(KEYBOARD);
    }

    void Update () {
        RayCastKeyboard();
    }

    private void RayCastKeyboard () {
        ray = new Ray(cam.transform.position, cam.transform.forward);
        if(Physics.Raycast(ray, out hit, rayLength, layer)) {
            KeyboardItem focusedKitem = hit.transform.gameObject.GetComponent<KeyboardItem>();

            if(kitemCurrent == null) {//if there is none selected
                kitemCurrent = focusedKitem;
            }
            if(focusedKitem != kitemCurrent) {//if previous is different
                kitemCurrent.stopHovering();
                kitemCurrent = focusedKitem;
            }
            kitemCurrent.hovering();
            if(Input.GetButtonDown("fire1")) {//if clicked
                kitemCurrent.click();
                keyboardStatus.HandleClick(kitemCurrent);
            }

        } else if(kitemCurrent != null) {//if no target hit and lost focus on item
            kitemCurrent.stopHovering();
            kitemCurrent = null;
        }
    }
}
