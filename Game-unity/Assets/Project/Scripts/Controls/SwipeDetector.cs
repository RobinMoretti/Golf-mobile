using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector3 startPosition, endPosition;
    private float startTime, endTime;
    private InputManager inputManager;

    
    [SerializeField]private float minimumDistance = 0.2f;
    [SerializeField]private float maximumTime = 0.8f;


    private void Awake() {
        inputManager = InputManager.Instance;
    }

    private void OnEnable() {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }
    private void OnDisable() {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector3 position, float time){
        startPosition = position;
        startTime = time;
    }
    private void SwipeEnd(Vector3 position, float time){
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe(){
        if(Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
         (endTime-startTime) <= maximumTime){
            Debug.Log("swiped = " );
            Debug.DrawLine(startPosition, endPosition, Color.blue, 5f);
        }
    }
}
