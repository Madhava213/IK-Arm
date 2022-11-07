using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Base;
    public GameObject Arm;
    public GameObject Joint;
    private float baseRadius;
    private GameObject[] Arms;
    private GameObject[] Joints;
    public int numArms = 5;
    private int numJoints;
    // Start is called before the first frame update
    void Start()
    {
        // Get Base Radius
        baseRadius = Base.transform.localScale.y / 2;

        // Calculate Number of Joints 
        numJoints = numArms - 1;

        // Instantiate Arms and Joints Array
        Arms = new GameObject[numArms];
        Joints = new GameObject[numJoints];

        //Spawn Arms and Joints
        for (int i = 0; i < numArms; i++)
        {
            GameObject newArm = Instantiate(Arm, new Vector3(0,0 + baseRadius + (i * (Arm.transform.localScale.y)),0), Quaternion.identity);
            newArm.transform.SetParent(Base.transform);
            Arms.SetValue(newArm, i);
            
            if(i < numJoints){
                GameObject newJoint = Instantiate(Joint, new Vector3(0,0 + ((i+1) * (Arm.transform.localScale.y)),0), Quaternion.identity);
                newJoint.transform.SetParent(Base.transform);
                Joints.SetValue(newJoint, i);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
