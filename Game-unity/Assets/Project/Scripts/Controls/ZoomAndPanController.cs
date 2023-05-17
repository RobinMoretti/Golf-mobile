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

    [SerializeField]private float minimumDistance = 0.2f;
    [SerializeField]private float maximumTime = 0.8f;

    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject ball;
    [SerializeField] private LayerMask canBeShotLayer;
    [SerializeField] private CameraController cameraController;
    private Vector3 ballContactPosition;

    [SerializeField] private CinemachineVirtualCamera followingCam;
    Cinemachine3rdPersonFollow cinemachine3rdPersonFollow;
    [SerializeField] private float cameraSpeed = 4;



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
        startPosition = position;
        startTime = time;
        trail.transform.position = inputManager.SecondaryPosition();
        trail.GetComponent<TrailRenderer>().Clear();
        StartCoroutine("trailUpdate");
    }

    private void stopZoomAndPan(Vector3 position, float time)
    {
        endPosition = position;
        endTime = time;
        // DetectSwipe();
        StopCoroutine("trailUpdate");
    }


    IEnumerator trailUpdate(){
        float previousDistance = 0, distance = 0;
        while(true){
            yield return new WaitForFixedUpdate();
            trail.transform.position = inputManager.SecondaryPosition();
            distance = Vector2.Distance(inputManager.PrimaryPosition(), inputManager.SecondaryPosition());

            // zoom out
            if(distance > previousDistance){
                float targetDistance = cinemachine3rdPersonFollow.CameraDistance - 2;
                cinemachine3rdPersonFollow.CameraDistance = Mathf.Lerp(cinemachine3rdPersonFollow.CameraDistance, targetDistance, Time.deltaTime * cameraSpeed);
            }
            // zoom in
            else if(distance < previousDistance){
                float targetDistance = cinemachine3rdPersonFollow.CameraDistance + 2;
                cinemachine3rdPersonFollow.CameraDistance = Mathf.Lerp(cinemachine3rdPersonFollow.CameraDistance, targetDistance, Time.deltaTime * cameraSpeed);

            }

            previousDistance = distance;

            yield return new WaitForFixedUpdate();
        }
    }
}
