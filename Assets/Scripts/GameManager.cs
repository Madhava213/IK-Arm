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
    private GameObject[] Groups;
    private GameObject[] Joints;
    public int numArms = 5;
    private int numJoints;

    public Transform TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        // Get Base Radius
        baseRadius = Base.transform.localScale.y / 2;

        // Calculate Number of Joints 
        numJoints = numArms - 1;

        // Instantiate Arms and Joints Array
        Arms = new GameObject[numArms];
        Groups = new GameObject[numArms];
        Joints = new GameObject[numJoints];

        SpawnCharacter();
    }

    void SpawnCharacter(){
        //Spawn Arms and Joints
        for (int i = 0; i < numArms; i++)
        {
            // Empty GameObject for Grouping Arm and Joints
            GameObject emptyObj = new GameObject("Arm-Joint-" + (i+1) );
            if(i == 0){
                emptyObj.transform.SetParent(Base.transform);
            }
            else{
                emptyObj.transform.SetParent(Groups[i-1].transform);
            }
            Groups.SetValue(emptyObj,i);

            // Group position at Joint's / Rotation Offset
            emptyObj.transform.position = new Vector3(0, 0 + ((i) * (Arm.transform.localScale.y)), 0);
            emptyObj.transform.Rotate(-90, 0, 0);

            // Create Arm
            GameObject newArm = Instantiate(Arm, new Vector3(0,0 + baseRadius + (i * (Arm.transform.localScale.y)),0), Quaternion.identity);
            newArm.transform.SetParent(emptyObj.transform);
            Arms.SetValue(newArm, i);


            // Create Joint
            if(i < numJoints){
                GameObject newJoint = Instantiate(Joint, new Vector3(0,0 + ((i+1) * (Arm.transform.localScale.y)),0), Quaternion.identity);
                newJoint.transform.SetParent(emptyObj.transform);
                Joints.SetValue(newJoint, i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //////////////////////////////////////
        // DEBUGGING CODE

        // float x = Input.GetAxis("Horizontal");
        // float z = Input.GetAxis("Vertical");
        // float speed = 20;
        // Groups[2].transform.position += new Vector3(x * speed * Time.deltaTime,0,z * speed * Time.deltaTime);
        //////////////////////////////////////

        // Get all Positions
        Vector3[] GroupPositions = new Vector3[Groups.Length];
        for (var i = 0; i < Groups.Length; i++)
        {
            GroupPositions.SetValue(Groups[i].transform.position, i);
        }

        // Backward
        GroupPositions[GroupPositions.Length-1] = TargetPos.position;
        for (int i = GroupPositions.Length - 2; i >= 0; i--) {
            Vector3 dir = (GroupPositions[i + 1] - GroupPositions[i]).normalized;
            GroupPositions[i] = GroupPositions[i + 1] + dir * Arm.transform.localScale.y;
        }

        // Forward
        GroupPositions[0] = Base.transform.position;
        for (int i = 1; i < numArms; i++) {
            Vector3 dir = (GroupPositions[i] - GroupPositions[i-1]).normalized;
            GroupPositions[i] = GroupPositions[i-1] + dir * Arm.transform.localScale.y;
        }

        // Reassign Positions
        for (var i = 0; i < Groups.Length; i++)
        {
            Groups[i].transform.position = GroupPositions[i];
            if(i == Groups.Length-1){
                Groups[i].transform.LookAt(TargetPos);
            }
            else{
                Groups[i].transform.LookAt(Groups[i+1].transform);
            }
        }
        
    }
}
