using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BoxColliderSizerButton: BoxColliderSizer {

    //public RectTransform rect;

    //private Image buttonImage;
    //private bool isClicked = false;

    //private Animator animator;
    //public void Start () {
    //    base.Start();
    //    animator = gameObject.GetComponent<Animator>();
    //}



    //// Update is called once per frame
    //void Update () {
    //    CheckHover();
    //}

    //public override void NotifyHoverStart () {
    //    isHovering = true;
    //    hoverTime = 0;
    //    radial.HandleDown();
    //    animator.CrossFade(
    //        AnimationVar.HIGHLIGHTED, AnimationVar.ANIM_TIME_NORMAL
    //        );
    //}

    //public override void NotifyHoverEnd () {
    //    radial.HandleUp();
    //    isHovering = false;
    //    isClicked = false;

    //    if(animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationVar.HIGHLIGHTED)) {//if no error
    //        playAnimation();
    //    }
    //    if(animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationVar.NORMAL)) {//if error ocurred
    //        StartCoroutine(IfError());
           
    //    }
      
    //}


    //public override void NotifyClickDown () {
    //    radial.HandleUp();
    //    GetComponent<Button>().onClick.Invoke();
    //    isClicked = true;
    //}

    //public override void NotifyClick () {

    //}

    //public override void NotifyClickUp () { 
    //}


    //private void CheckHover () {
    //    if(isHovering && !isClicked) {//if hovering and is not clicked yet
    //        hoverTime += Time.deltaTime;
    //        if(hoverTime >= AnimationVar.MENU_CIRCLE_FILL_TIMER) {//is filler full
    //            NotifyHoverEnd();
    //            StatisticTrackers.GameplayStarted();
    //            GetComponent<Button>().onClick.Invoke();
    //        }
    //    }
    //}

    //public override void AdditionalInitialization () {
    //}

    //private IEnumerator IfError () {
    //    yield return new WaitForSeconds(AnimationVar.ANIM_TIME_NORMAL+0.01f);
    //    playAnimation();
    //}

    //public void playAnimation () {
    //    animator.CrossFade(
    //          AnimationVar.NORMAL, AnimationVar.ANIM_TIME_NORMAL
    //          );

    //}
}
