using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private PlayerTouchController playerControls;

    private Vector3 startPosition, endPosition;
    private float startTime, endTime;
    private InputManager inputManager;

    [SerializeField]private float minimumDistance = 0.2f;
    [SerializeField]private float maximumTime = 0.8f;

    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject ball;
    [SerializeField] private LayerMask canBeShotLayer;

    private bool playerHitTheBall = false;

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

            // detect if touched
            // int distanceMax = 5;
            if(playerHitTheBall == false){
                Ray ray = Camera.main.ScreenPointToRay(inputManager.getTouchPosition());
                RaycastHit hitInfo;
                if(Physics.Raycast(ray, out hitInfo, canBeShotLayer)){
                    if(hitInfo.collider.gameObject.tag == "Player"){
                        playerHitTheBall = true;
                    }
                }
            }
        }
    }

    private void DetectSwipe(){
        float swipeLength = Vector3.Distance(startPosition, endPosition);
        if(swipeLength >= minimumDistance &&
         (endTime-startTime) <= maximumTime){
            

            if(playerHitTheBall){
                Debug.DrawLine(startPosition, endPosition, Color.blue, 5f);
                // get the shot vertor
                Vector3 direction = endPosition - startPosition;

                ball.GetComponent<BallController>().shot(direction, swipeLength);

                print("fireeeee");
            }
            // ball.GetComponent<Rigidbody>().AddExplosionForce(
            //     Vector2.Distance(startPosition, endPosition), 
            //     ball.transform.position, 1);

        }

        // reset the hiting ball boolean, no more useful
        playerHitTheBall = false;
    }
}
