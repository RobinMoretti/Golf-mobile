using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotationY = 0;

    private void Start() {
        rotationY = transform.rotation.y;
    }

    public void rotate(float rotationStrength){
        // rotationStrength
        Quaternion initialRotation = transform.rotation;
        // initialRotation 
        transform.Rotate(new Vector3(0, rotationStrength, 0), Space.World);
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
