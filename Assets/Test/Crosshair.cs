using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
    public Transform target;
    public int TeamNumber;

    
	// Use this for initialization
	void Start () {
        if (TeamNumber == 0)
        {
            gameObject.layer = 8;
            gameObject.renderer.material.SetColor("_TintColor", Color.blue);
        }
        else if (TeamNumber == 1)
        {
            gameObject.layer = 9;
            gameObject.renderer.material.SetColor("_TintColor", Color.red);
        }
        else if (TeamNumber == 2)
        {
            gameObject.layer = 10;
            gameObject.renderer.material.SetColor("_TintColor", Color.yellow);
        }
        else if (TeamNumber == 3)
        {
            gameObject.layer = 11;
            gameObject.renderer.material.SetColor("_TintColor", Color.green);
        }
	}
   public  void SetTarget(Transform theTarget)
    {
        target = theTarget;
    }

	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.up, 60f * Time.deltaTime, Space.Self);
        if(target)
         transform.position = target.position;
	}
}
