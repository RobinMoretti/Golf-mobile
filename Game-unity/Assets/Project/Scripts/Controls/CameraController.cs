using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotationY = 0;
    private InputManager inputManager;
    private void Awake() {
        inputManager = InputManager.Instance;
    }

    [SerializeField] private float rotationStrenght = 0.5f;

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
            transform.Rotate(new Vector3(0, -rotationDirection.x * rotationStrenght, 0), Space.World);
            transform.Rotate(new Vector3(rotationDirection.y * rotationStrenght, 0, 0), Space.Self);
            yield return new WaitForEndOfFrame();
        }
        
        rotationDirection = Vector2.zero;
        yield return new WaitForEndOfFrame();
    }

    public void rotateTemporary(float rotationStrength){
        rotationY += rotationStrength;
    }

    private void updateBallRotation(){
        // transform.
    }
}
