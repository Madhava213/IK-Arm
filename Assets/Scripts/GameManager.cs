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
    private float armLength = 0;

    public Transform TargetPos;
    private Vector3 lastTargetPos;

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

        // Arm Length
        armLength = Arm.transform.localScale.z;

        // Instantiate lastTargetPos
        lastTargetPos = new Vector3(0, 0, 0);

        // Spawn
        foreach (Transform child in Base.transform) {
            GameObject.Destroy(child.gameObject);
        }
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
            emptyObj.transform.position = new Vector3(0, 0 + ((i+1) * (armLength)), 0);
            // emptyObj.transform.Rotate(-90, 0, 0);
            emptyObj.transform.LookAt(TargetPos);

            // Create Arm
            GameObject newArm = Instantiate(Arm, new Vector3(0,0 + baseRadius + (i * (armLength)),0), Quaternion.identity);
            newArm.transform.Rotate(90, 0, 0);
            newArm.transform.SetParent(emptyObj.transform);
            Arms.SetValue(newArm, i);


            // Create Joint
            if(i < numJoints){
                GameObject newJoint = Instantiate(Joint, new Vector3(0,0 + ((i+1) * (armLength)),0), Quaternion.identity);
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

        if(TargetPos.position.Equals(lastTargetPos)){
            FabrikIK();
        }
        lastTargetPos = TargetPos.position;
    }

    void FabrikIK(){
        for (int z = 0; z < 20; z++)
        {
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
                GroupPositions[i] = GroupPositions[i + 1] + dir * armLength;
            }
	
	        // Forward
            for (int i = 0; i < numArms; i++) {
                if(i == 0){
                    Vector3 dir = (GroupPositions[i] - Base.transform.position).normalized;
                    GroupPositions[i] = Base.transform.position + dir * armLength;
                }
                else{
                    Vector3 dir = (GroupPositions[i] - GroupPositions[i-1]).normalized;
                    GroupPositions[i] = GroupPositions[i-1] + dir * armLength;
                }
            }


            // Reassign Positions
            for (var i = 0; i < Groups.Length; i++)
            {
                Groups[i].transform.position = GroupPositions[i];

                if(i == Groups.Length-1){
                    Groups[i].transform.LookAt(TargetPos);

                    // Arms Reset
                    Vector3 armDir = (Groups[i].transform.position - Groups[i-1].transform.position) / 2.0f;
                    Arms[i].transform.position = Groups[i-1].transform.position + armDir;
                    Arms[i].transform.LookAt(Groups[i].transform.position);
                }
                else if(i == 0){
                    // Arms Reset
                    Vector3 armDir = (Groups[i].transform.position - Base.transform.position) / 2.0f;
                    Arms[i].transform.position = Base.transform.position + armDir;
                    Arms[i].transform.LookAt(Groups[i].transform.position);
                }
                else{
                    Groups[i].transform.LookAt(Groups[i+1].transform);

                    // Arms Reset
                    Vector3 armDir = (Groups[i].transform.position - Groups[i-1].transform.position) / 2.0f;
                    Arms[i].transform.position = Groups[i-1].transform.position + armDir;
                    Arms[i].transform.LookAt(Groups[i].transform.position);
                }
                
            }

        }
    }
}
