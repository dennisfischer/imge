using UnityEngine;
using System.Collections;
using System;

public class TestPlayer : MonoBehaviour {
   public float Speed = 5f;
   int targetNumber = 0;
   GameObject target;
   public GameObject indicator;
   public ArrayList SeekerList = new ArrayList();
    void Start()
    {
        indicator.renderer.enabled = false;
       
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIt");
        if (other.tag == gameObject.tag)
            return;
        SeekerList.Add(other.gameObject);
        if (SeekerList.Count == 1)
        {
            indicator.renderer.enabled = true;
            indicator.GetComponent<Crosshair>().SetTarget(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        if (other.tag == gameObject.tag)
            return;
        
        SeekerList.Remove(other.gameObject);
        if (SeekerList.Count == 0)
        {
            target = null;
            indicator.renderer.enabled = false;
        }
        else if (target == other.gameObject && SeekerList.Count > 0)
        {
            targetNumber = UnityEngine.Random.Range(0, SeekerList.Count);
            target = (GameObject)SeekerList[targetNumber];
            indicator.GetComponent<Crosshair>().SetTarget(target.transform);
        }
        
    }
    void FixedUpdate()
    {
        rigidbody.AddRelativeForce(Vector3.forward * Speed * Input.GetAxis("Vertical"));
        rigidbody.AddRelativeForce(Vector3.right * Speed * Input.GetAxis("Horizontal"));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && SeekerList.Count>0)
        {
            targetNumber++;
            targetNumber %= SeekerList.Count;
            target = (GameObject)SeekerList[targetNumber];
            indicator.GetComponent<Crosshair>().SetTarget(target.transform);
        }


       

        
    }

 
}
