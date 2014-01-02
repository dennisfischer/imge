using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
    public float LaserSpeed;
    public int Damage = 50;
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
            if (playerhit.PlayerHP > 0)
            {
                playerhit.SetHealth(-Damage);
                if (playerhit.PlayerHP <= 0 && (other.gameObject.GetComponent<Check>().TeamNumber != TeamNumber))
                {
                    GameManager.Instance.setTeamScores(TeamNumber);
                }
            }


           
        }

        else if (other.gameObject.GetComponent<AI>() != null)
        {
            AI playerhit = (AI)other.gameObject.GetComponent<AI>();
            if (playerhit.PlayerHP > 0)
            {
                playerhit.SetHealth(-Damage);
                if (playerhit.PlayerHP <= 0 && (other.gameObject.GetComponent<Check>().TeamNumber != TeamNumber))
                {
                    GameManager.Instance.setTeamScores(TeamNumber);
                }
            }



        }
        
        //other.gameObject.SendMessage("SetHealth", -Damage, SendMessageOptions.DontRequireReceiver);
        Instantiate(Sparks, transform.position, transform.rotation);
        Destroy(gameObject);
    }

   

}
