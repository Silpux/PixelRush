using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeEventTrigger : EventTrigger{
    [SerializeField] private float minSwipeDist = 50f;

    private Vector2 startPos;
    private bool isSwiping = false;

    public event Action OnSwipeUp;
    public event Action OnSwipeDown;

    public override void OnBeginDrag(PointerEventData eventData){
        startPos = eventData.position;
        isSwiping = true;
    }

    public override void OnDrag(PointerEventData eventData){
        if(!isSwiping){
            return;
        }

        Vector2 diff = eventData.position - startPos;

        if(diff.magnitude >= minSwipeDist){
            if(Mathf.Abs(diff.x) < Mathf.Abs(diff.y)){
                if(diff.y > 0){
                    OnSwipeUp?.Invoke();
                }
                else{
                    OnSwipeDown?.Invoke();
                }
            }
            isSwiping = false;
        }
    }

    public override void OnEndDrag(PointerEventData eventData){
        isSwiping = false;
    }
}