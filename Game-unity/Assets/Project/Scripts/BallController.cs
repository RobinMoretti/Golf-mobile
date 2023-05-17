using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{   
    [SerializeField] private float shotStrenght = 2;
    [SerializeField] private ForceMode forceMode;

    private new Rigidbody rigidbody;
    private bool wasShoot = false;

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void shot(Vector3 direction, float strenght){
        if(wasShoot == false){
            direction.y = 0;
            rigidbody.AddForce(direction * strenght * shotStrenght, forceMode);
        }

        StartCoroutine(surveilBallSpeed());
        wasShoot = true;
    }

    IEnumerator surveilBallSpeed(){
        yield return new WaitForSeconds(0.2f);
        
        while(rigidbody.velocity.magnitude > 0.10f){
            yield return new WaitForSeconds(0.1f);
        }

        // stop the ball
        while(rigidbody.velocity.magnitude > 0.001f){
            rigidbody.velocity *= 0.7f;
            rigidbody.angularVelocity *= 0.7f;
            yield return new WaitForSeconds(0.1f);
        }

        wasShoot = false;

        yield return null;
    }


}
