using UnityEngine;
using System.Collections;

public class CubeRotation : MonoBehaviour {
    float zRot;
    Controller thisController;
    public float zAngle;


	// Use this for initialization
	void Start () {
        thisController = transform.parent.GetComponent<Player>().myController;
	}
	
	// Update is called once per frame
	void Update () {
        
        zRot = thisController.yAcc * zAngle;
       transform.rotation = Quaternion.Slerp(transform.rotation, transform.parent.transform.rotation * Quaternion.AngleAxis(zRot , Vector3.forward), Time.deltaTime *10);
        

	}
}
