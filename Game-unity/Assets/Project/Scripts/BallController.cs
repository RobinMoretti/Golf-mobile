using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{   
    [SerializeField] private float shotStrenght = 2;
    [SerializeField] private ForceMode forceMode;

    private Rigidbody rigidbody;

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void shot(Vector3 direction, float strenght){

        direction.y = 0;
        rigidbody.AddForce(direction * strenght * shotStrenght, forceMode);
    }
}
