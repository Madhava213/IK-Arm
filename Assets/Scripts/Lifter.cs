using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifter : MonoBehaviour
{
    public bool inContact = false;

    private void Update() {
        if(!inContact){
            this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.collider.tag == "Object"){
            inContact = true;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
