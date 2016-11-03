using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class BoxColliderSizer: MonoBehaviour {

    //protected DHSelectionRadial radial;
    //protected DHReticle reticle;

    //public bool isHovering;
    //private bool firstRun = true;


    //protected Vector2 componentSize;
    //protected RectTransform rectTransform;
    //protected BoxCollider collider;


    //protected float hoverTime = 0f;
    //protected static float hoverTimeLimit = 1.5f;

    //public abstract void NotifyHoverStart ();
    //public abstract void NotifyHoverEnd ();
    //public abstract void NotifyClick ();
    //public abstract void NotifyClickDown ();
    //public abstract void NotifyClickUp ();
    //public abstract void AdditionalInitialization ();

    //public void Start () {

    //    reticle = GameObject.Find(SceneVar.MAIN_CAMERA).GetComponent<DHReticle>();
    //    radial = GameObject.Find(SceneVar.MAIN_CAMERA).GetComponent<DHSelectionRadial>();

    //}

    //public void Awake () {
    //    collider = gameObject.AddComponent<BoxCollider>();
    //    gameObject.AddComponent<VRInteractiveItem>();
    //}

    //public void OnGUI () {
    //    InitComponent();
    //}

    ///// <summary>
    ///// called in onGui method only once (earlier size is not aviable)
    ///// </summary>
    //private void InitComponent () {
    //    if(firstRun) {
    //        firstRun = false;
    //        rectTransform = ( (RectTransform)GetComponent<RectTransform>() );
    //        componentSize = rectTransform.rect.size;
    //        collider.size = new Vector3(componentSize.x, componentSize.y, 0.01f);
    //        AdditionalInitialization();
    //    }
    //}
}


