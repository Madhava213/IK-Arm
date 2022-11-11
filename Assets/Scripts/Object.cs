using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class Object : MonoBehaviour 
{

    private Vector3 screenPoint;
    private Vector3 offset;
    public GameManager manager;

    void Update() {
        if(this.gameObject.transform.parent == null){
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else{
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void OnMouseDown()
    {
        if(manager.manualMode){
            this.gameObject.transform.parent = null;
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            manager.Lifter1.GetComponent<Lifter>().inContact = false;
            manager.Lifter2.GetComponent<Lifter>().inContact = false;
        }

    }

    void OnMouseDrag()
    {
        if (manager.manualMode)
        {

            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }

}