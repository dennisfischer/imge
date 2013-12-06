using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
    public float LaserSpeed;
    public int Damage = 10;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameObject.collider.enabled)
        {
            StartCoroutine(goThrough());
        }
        transform.Translate(Vector3.forward * Time.deltaTime * LaserSpeed);
	}

    void OnCollisionEnter(Collision other)
    {
       
        Destroy(gameObject);
    }

    IEnumerator goThrough()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.collider.enabled = true;
    }
}
