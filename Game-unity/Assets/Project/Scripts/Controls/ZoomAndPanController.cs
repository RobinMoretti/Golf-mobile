using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomAndPanController : MonoBehaviour
{
    private PlayerTouchController playerControls;

    private Vector3 startPosition, endPosition;
    private float startTime, endTime;
    private InputManager inputManager;

    [SerializeField] private GameObject trailPrefab;
    private GameObject trail;

    [SerializeField] private CameraController cameraController;

    [SerializeField] private CinemachineVirtualCamera followingCam;
    Cinemachine3rdPersonFollow cinemachine3rdPersonFollow;
    [SerializeField] private float cameraSpeed = 5;



    private void Awake() {
        inputManager = InputManager.Instance;
    }

    private void Start() {
        cinemachine3rdPersonFollow = followingCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void OnEnable() {
        inputManager.OnStartSecondaryTouch += startZoomAndPan;
        inputManager.OnEndSecondaryTouch += stopZoomAndPan;
    }
    private void OnDisable() {
        inputManager.OnStartSecondaryTouch -= startZoomAndPan;
        inputManager.OnEndSecondaryTouch -= stopZoomAndPan;
    }

    private void startZoomAndPan(Vector3 position, float time)
    {   
        print("startZoomAndPan");
        startPosition = position;
        startTime = time;
        trail = Instantiate(trailPrefab, inputManager.PrimaryPosition(), Quaternion.identity);
        StartCoroutine("trailUpdate");
    }

    private void stopZoomAndPan(Vector3 position, float time)
    {
        endPosition = position;
        endTime = time;
        // DetectSwipe();
        StopCoroutine("trailUpdate");
    }

    float zoomStrength = 0.008f;
    IEnumerator trailUpdate(){
        float previousDistance = 0, distance = 0;
        float initialCamDistance = cinemachine3rdPersonFollow.CameraDistance;
        float initialFingerDistance = Vector2.Distance(inputManager.getTouchPosition(1), inputManager.getTouchPosition(2));;

        while(true){
            yield return new WaitForFixedUpdate();
            if(trail) trail.transform.position = inputManager.SecondaryPosition();

            distance = Vector2.Distance(inputManager.getTouchPosition(1), inputManager.getTouchPosition(2));
            
            if(cinemachine3rdPersonFollow.CameraDistance > 0.8f && cinemachine3rdPersonFollow.CameraDistance < 50){
                float targetOffset = Mathf.Abs(distance - initialFingerDistance);

                if(targetOffset < 1f) yield return null;

                // zoom out
                if(distance > previousDistance){
                    float targetDistance = cinemachine3rdPersonFollow.CameraDistance - (targetOffset*zoomStrength);
                    cinemachine3rdPersonFollow.CameraDistance = Mathf.Lerp(cinemachine3rdPersonFollow.CameraDistance, targetDistance, Time.deltaTime * cameraSpeed);
                }
                // zoom in
                else if(distance < previousDistance){
                    float targetDistance = cinemachine3rdPersonFollow.CameraDistance + (targetOffset*zoomStrength);
                    cinemachine3rdPersonFollow.CameraDistance = Mathf.Lerp(cinemachine3rdPersonFollow.CameraDistance, targetDistance, Time.deltaTime * cameraSpeed);
                }

                previousDistance = distance;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
