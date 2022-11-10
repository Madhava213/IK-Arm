using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject Base;
    public GameObject Arm;
    public GameObject Joint;
    public GameObject Lifter;
    public GameObject Object;
    private float baseRadius;
    private GameObject[] Arm1;
    private GameObject[] Arm2;
    private GameObject[] Groups1;
    private GameObject[] Groups2;
    private GameObject[] Joints1;
    private GameObject[] Joints2;
    public int numArms = 5;
    private int numJoints;
    private float armLength = 0;

    public Transform TargetPos1;
    public Transform TargetPos2;
    private Vector3 lastTarget1Pos;
    private Vector3 lastTarget2Pos;
    private bool manualMode = false;
    public TextMeshProUGUI manualModeText;

    // Start is called before the first frame update
    void Start()
    {
        // Get Base Radius
        baseRadius = Base.transform.localScale.y / 2;

        // Calculate Number of Joints 
        numJoints = numArms - 1;

        // Instantiate Arms and Joints Array
        Arm1 = new GameObject[numArms];
        Groups1 = new GameObject[numArms];
        Joints1 = new GameObject[numJoints];
        Arm2 = new GameObject[numArms];
        Groups2 = new GameObject[numArms];
        Joints2 = new GameObject[numJoints];

        // Arm Length
        armLength = Arm.transform.localScale.z;

        // Instantiate lastTargetPos
        lastTarget1Pos = new Vector3(0, 0, 0);
        lastTarget2Pos = new Vector3(0, 0, 0);

        // Spawn
        foreach (Transform child in Base.transform) {
            GameObject.Destroy(child.gameObject);
        }
        SpawnArm1();
        SpawnArm2();
    }

    void SpawnArm1(){
        //Spawn Arms and Joints
        for (int i = 0; i < numArms; i++)
        {
            // Empty GameObject for Grouping Arm and Joints
            GameObject emptyObj = new GameObject("Arm1-Joint-" + (i+1) );
            if(i == 0){
                emptyObj.transform.SetParent(Base.transform);
            }
            else{
                emptyObj.transform.SetParent(Groups1[i-1].transform);
            }
            Groups1.SetValue(emptyObj,i);

            // Group position at Joint's / Rotation Offset
            emptyObj.transform.position = new Vector3(0, 0 + ((i+1) * (armLength)), 0);
            emptyObj.transform.LookAt(TargetPos1);

            // Create Arm
            GameObject newArm = Instantiate(Arm, new Vector3(0,0 + baseRadius + (i * (armLength)),0), Quaternion.identity);
            newArm.transform.Rotate(90, 0, 0);
            newArm.transform.SetParent(emptyObj.transform);
            Arm1.SetValue(newArm, i);


            // Create Joint
            if(i < numJoints){
                GameObject newJoint = Instantiate(Joint, new Vector3(0,0 + ((i+1) * (armLength)),0), Quaternion.identity);
                newJoint.transform.SetParent(emptyObj.transform);
                Joints1.SetValue(newJoint, i);
            }

            // Create Lifter
            if (i == numArms - 1) { 
                GameObject newLifter = Instantiate(Lifter, new Vector3(0, 0 + ((i+1) * (armLength)), 0), Quaternion.identity);
                newLifter.transform.SetParent(emptyObj.transform);
            }
        }
    }

    void SpawnArm2(){
        //Spawn Arms and Joints
        for (int i = 0; i < numArms; i++)
        {
            // Empty GameObject for Grouping Arm and Joints
            GameObject emptyObj;
            if(i == 0){
                emptyObj = Groups1[0];
            }
            else{
                emptyObj = new GameObject("Arm2-Joint-" + (i + 1));
                emptyObj.transform.SetParent(Groups2[i-1].transform);
            }
            Groups2.SetValue(emptyObj,i);

            // Group position at Joint's / Rotation Offset
            emptyObj.transform.position = new Vector3(0, 0 + ((i+1) * (armLength)), 0);
            emptyObj.transform.LookAt(TargetPos2);

            if(i == 0){
                Arm2[i] = Arm1[0];
            }
            else{
                // Create Arm
                GameObject newArm = Instantiate(Arm, new Vector3(0,0 + baseRadius + (i * (armLength)),0), Quaternion.identity);
                newArm.transform.Rotate(90, 0, 0);
                newArm.transform.SetParent(emptyObj.transform);
                Arm2.SetValue(newArm, i);
            }


            // Create Joint
            if(i < numJoints){
                if(i == 0){
                    Joints2[i] = Joints1[0];
                }
                else{
                    GameObject newJoint = Instantiate(Joint, new Vector3(0,0 + ((i+1) * (armLength)),0), Quaternion.identity);
                    newJoint.transform.SetParent(emptyObj.transform);
                    Joints2.SetValue(newJoint, i);
                }
            }

            // Create Lifter
            if (i == numArms - 1) { 
                GameObject newLifter = Instantiate(Lifter, new Vector3(0, 0 + ((i+1) * (armLength)), 0), Quaternion.identity);
                newLifter.transform.SetParent(emptyObj.transform);
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
        // Groups1[2].transform.position += new Vector3(x * speed * Time.deltaTime,0,z * speed * Time.deltaTime);
        //////////////////////////////////////

        if(!TargetPos1.position.Equals(lastTarget1Pos)){
            Group1FabrikIK();
        }
        if(!TargetPos2.position.Equals(lastTarget2Pos)){
            Group2FabrikIK();
        }
        lastTarget1Pos = TargetPos1.position;
        lastTarget2Pos = TargetPos2.position;
    }

    void Group1FabrikIK(){
        for (int z = 0; z < 20; z++)
        {

	        // Get all Group1 Positions
            Vector3[] Group1Positions = new Vector3[Groups1.Length];
            for (var i = 0; i < Groups1.Length; i++)
            {
                Group1Positions.SetValue(Groups1[i].transform.position, i);
            }
	
            // GROUP 1
	        // Backward
            Group1Positions[Group1Positions.Length-1] = TargetPos1.position;
            for (int i = Group1Positions.Length - 2; i >= 0; i--) {
                Vector3 dir = (Group1Positions[i + 1] - Group1Positions[i]).normalized;
                Group1Positions[i] = Group1Positions[i + 1] + dir * armLength;
            }
	
	        // Forward
            for (int i = 0; i < numArms; i++) {
                if(i == 0){
                    Vector3 dir = (Group1Positions[i] - Base.transform.position).normalized;
                    Group1Positions[i] = Base.transform.position + dir * armLength;
                }
                else{
                    Vector3 dir = (Group1Positions[i] - Group1Positions[i-1]).normalized;
                    Group1Positions[i] = Group1Positions[i-1] + dir * armLength;
                }
            }

            // Reassign Group1 Positions
            for (var i = 0; i < Groups1.Length; i++)
            {
                Groups1[i].transform.position = Group1Positions[i];

                if(i == Groups1.Length-1){
                    Groups1[i].transform.LookAt(TargetPos1);

                    // Arms Reset
                    Vector3 armDir = (Groups1[i].transform.position - Groups1[i-1].transform.position) / 2.0f;
                    Arm1[i].transform.position = Groups1[i-1].transform.position + armDir;
                    Arm1[i].transform.LookAt(Groups1[i].transform.position);
                }
                else if(i == 0){
                    // Arms Reset
                    Vector3 armDir = (Groups1[i].transform.position - Base.transform.position) / 2.0f;
                    Arm1[i].transform.position = Base.transform.position + armDir;
                    Arm1[i].transform.LookAt(Groups1[i].transform.position);
                }
                else{
                    Groups1[i].transform.LookAt(Groups1[i+1].transform);

                    // Arms Reset
                    Vector3 armDir = (Groups1[i].transform.position - Groups1[i-1].transform.position) / 2.0f;
                    Arm1[i].transform.position = Groups1[i-1].transform.position + armDir;
                    Arm1[i].transform.LookAt(Groups1[i].transform.position);
                }
                
            }
        }
    }

    void Group2FabrikIK()
    {
        for (int z = 0; z < 20; z++)
        {
            // Get all Group2 Positions
            Vector3[] Group2Positions = new Vector3[Groups2.Length];
            for (var i = 0; i < Groups2.Length; i++)
            {
                Group2Positions.SetValue(Groups2[i].transform.position, i);
            }

            // GROUP 2
            // Backward
            Group2Positions[Group2Positions.Length-1] = TargetPos2.position;
            for (int i = Group2Positions.Length - 2; i >= 0; i--) {
                Vector3 dir = (Group2Positions[i + 1] - Group2Positions[i]).normalized;
                Group2Positions[i] = Group2Positions[i + 1] + dir * armLength;
            }
	
	        // Forward
            for (int i = 0; i < numArms; i++) {
                if(i == 0){
                    Vector3 dir = (Group2Positions[i] - Base.transform.position).normalized;
                    Group2Positions[i] = Base.transform.position + dir * armLength;
                }
                else{
                    Vector3 dir = (Group2Positions[i] - Group2Positions[i-1]).normalized;
                    Group2Positions[i] = Group2Positions[i-1] + dir * armLength;
                }
            }

            // Reassign Group2 Positions
            for (var i = 0; i < Groups2.Length; i++)
            {
                Groups2[i].transform.position = Group2Positions[i];

                if(i == Groups2.Length-1){
                    Groups2[i].transform.LookAt(TargetPos2);

                    // Arms Reset
                    Vector3 armDir = (Groups2[i].transform.position - Groups2[i-1].transform.position) / 2.0f;
                    Arm2[i].transform.position = Groups2[i-1].transform.position + armDir;
                    Arm2[i].transform.LookAt(Groups2[i].transform.position);
                }
                else if(i == 0){
                    // Arms Reset
                    Vector3 armDir = (Groups2[i].transform.position - Base.transform.position) / 2.0f;
                    Arm2[i].transform.position = Base.transform.position + armDir;
                    Arm2[i].transform.LookAt(Groups2[i].transform.position);
                }
                else{
                    Groups2[i].transform.LookAt(Groups2[i+1].transform);

                    // Arms Reset
                    Vector3 armDir = (Groups2[i].transform.position - Groups2[i-1].transform.position) / 2.0f;
                    Arm2[i].transform.position = Groups2[i-1].transform.position + armDir;
                    Arm2[i].transform.LookAt(Groups2[i].transform.position);
                }
                
            }

        }
    }

    public void ToggleManualMode(){
        manualMode = !manualMode;
        if(manualMode){
            manualModeText.text = "Manual Mode : ON";
        }
        else{
            manualModeText.text = "Manual Mode : OFF";
        }
    }
}
