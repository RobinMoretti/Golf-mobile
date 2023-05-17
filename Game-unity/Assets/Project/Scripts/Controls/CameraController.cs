using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private float rotationY = 0;

    private InputManager inputManager;
    private void Awake() {
        inputManager = InputManager.Instance;
    }

    [SerializeField] private float rotationYStrenght = 0.5f;
    [SerializeField] private float rotationXStrenght = 0.5f;
    [SerializeField] private float rotationYOffsetBottom = 10f;
    [SerializeField] private float rotationYOffsetTop = 80f;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private void OnEnable() {
        inputManager.OnMoveJoystick += OnMoveJoystick;
        inputManager.OnStartTouch += onStartTouching;
        inputManager.OnEndTouch += onStopTouching;
    }
    private void OnDisable() {
        inputManager.OnMoveJoystick -= OnMoveJoystick;
        inputManager.OnStartTouch -= onStartTouching;
        inputManager.OnEndTouch -= onStopTouching;
    }   
    private void Start() {
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        rotationY = transform.rotation.y;  
    }

    bool isTouching = false;
    bool joystickIsMoving = false;
    private Vector2 rotationDirection;

    void OnMoveJoystick(Vector2 direction){
        rotationDirection = direction;

        if(joystickIsMoving == false) {
            joystickIsMoving = true;
        }

        if(isTouching && joystickIsMoving){
            StartCoroutine(rotateCam());
        }
    }

    void onStartTouching(Vector3 position, float time){
        if(isTouching == false) {
            isTouching = true;
        }
    }

    void onStopTouching(Vector3 position, float time){
        if(joystickIsMoving == true) {
            joystickIsMoving = false;
        }
        if(isTouching == true) {
            isTouching = false;
        }
    }

    
    IEnumerator rotateCam(){
        while(isTouching == true && joystickIsMoving == true){
            if(transform.eulerAngles.x < rotationYOffsetTop && rotationDirection.y > 0 ){
                transform.Rotate(new Vector3(rotationDirection.y * rotationYStrenght, 0, 0), Space.Self);
            }
            else if (transform.eulerAngles.x > rotationYOffsetBottom && rotationDirection.y < 0){
                transform.Rotate(new Vector3(rotationDirection.y * rotationYStrenght, 0, 0), Space.Self);
            }
            
            transform.Rotate(new Vector3(0, -rotationDirection.x * rotationXStrenght, 0), Space.World);

            yield return new WaitForFixedUpdate();
        }
        
        rotationDirection = Vector2.zero;
        yield return new WaitForFixedUpdate();
    }

    public void rotateTemporary(float rotationStrength){
        rotationY += rotationStrength;
    }

    private void updateBallRotation(){
        // transform.
    }


    public void shakeCam(float strength){
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = strength;
        StartCoroutine(reduceShakingCam());
    }

    IEnumerator reduceShakingCam(){
        while(cinemachineBasicMultiChannelPerlin.m_AmplitudeGain > 0.01f){
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain *= 0.91f;
            yield return new WaitForFixedUpdate();
        }
        
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        yield return null;
    }
}
