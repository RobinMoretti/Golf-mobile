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

    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject ball;

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
        trail.transform.position = inputManager.PrimaryPosition();
        trail.GetComponent<TrailRenderer>().Clear();
        StartCoroutine("trailUpdate");

    }
    private void SwipeEnd(Vector3 position, float time){
        endPosition = position;
        endTime = time;
        DetectSwipe();
        StopCoroutine("trailUpdate");
    }

    IEnumerator trailUpdate(){
        while(true){
            yield return new WaitForFixedUpdate();
            trail.transform.position = inputManager.PrimaryPosition();
        }
    }

    private void DetectSwipe(){
        if(Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
         (endTime-startTime) <= maximumTime){
            Debug.Log("swiped = " );
            Vector3 Direction = endPosition - startPosition;
            Debug.DrawLine(startPosition, endPosition, Color.blue, 5f);

            ball.GetComponent<Rigidbody>().AddExplosionForce(
                Vector2.Distance(startPosition, endPosition), 
                ball.transform.position, 1);

        }
    }
}
