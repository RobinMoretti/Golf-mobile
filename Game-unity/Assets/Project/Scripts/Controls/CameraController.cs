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


    private void OnEnable() {
        inputManager.OnMoveJoystick += rotate;
    }
    private void OnDisable() {
        inputManager.OnMoveJoystick -= rotate;
    }   
    private void Start() {
        rotationY = transform.rotation.y;   
    }   

    public void rotate(Vector2 direction){
        // todo: add a constant rotation when moving the joystick
        // todo: fix a roation on one axis at time (if two angle modifying all of them)

        // rotationStrength
        print("rotate");
        // initialRotation 
        transform.Rotate(new Vector3(0, direction.x, 0), Space.World);
        transform.Rotate(new Vector3(direction.y, 0, 0), Space.Self);
        // float deg = 
        // rotationY += rotationStrength;
    }

    public void rotateTemporary(float rotationStrength){
        rotationY += rotationStrength;
    }

    private void updateBallRotation(){
        // transform.
    }
}
