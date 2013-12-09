using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
    Transform mySpawnPoint;
    int HP = 100;
    public GameObject Explosion;
    Transform target;

    void setSpawnPoint(Transform spawn)
    {
        mySpawnPoint = spawn;
    }

    public void SetHealth(int health)
    {
        HP += health;
        if (HP <= 0)
        {

            Instantiate(Explosion, transform.position, transform.rotation);
            transform.position = mySpawnPoint.position;
            transform.rotation = mySpawnPoint.rotation;
            HP = 100;

        }
    }

    [System.Serializable]
    public class Cannon
    {
        public GameObject[] Position;
        public GameObject Projectile;
        public float ReloadTime = 0.5f;
        private float Timer;

        public void Shoot()
        {
            if (Timer <= 0f)
            {
                foreach (GameObject position in Position)
                {
                    Instantiate(Projectile, position.transform.position, position.transform.rotation);
                    Timer = ReloadTime;
                }
            }

        }
        public void CheckTime()
        {
            Timer -= Time.deltaTime;
        }
    }

    public Cannon LaserCannon;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
