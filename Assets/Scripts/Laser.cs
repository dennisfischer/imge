using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
    public float LaserSpeed;
    public int Damage = 10;
    public GameObject Sparks;
    public int TeamNumber;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        

        transform.Translate(Vector3.forward * Time.deltaTime * LaserSpeed, Space.Self);
	}

    void OnCollisionEnter(Collision other)
    {
        //Time.timeScale = 0;
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player playerhit = (Player)other.gameObject.GetComponent<Player>();
            playerhit.SetHealth(-Damage);
        }
        
        //other.gameObject.SendMessage("SetHealth", -Damage, SendMessageOptions.DontRequireReceiver);
        Instantiate(Sparks, transform.position, transform.rotation);
        Destroy(gameObject);
    }

   

}
