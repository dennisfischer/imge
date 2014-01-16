using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
    float TurnerSimulatorLeft, TurnerSimulatorRight;
    public int PlayerHP = 100;
    public int TeamNumber;


    public float Thruster;
    

    public GameObject Explosion;
    Transform mySpawnPoint;
    public Renderer[] childObjects = new Renderer[5];

    string[] tags = { "blue", "red", "yellow", "green" };
    public GameObject[] targets = new GameObject[15];
    int targetNumber = 0;
    GameObject target;


    
    public enum PlayerState
    {
        Invincible, Playing
    }
    PlayerState state;

    public void SetPlayerNumber(int i)
    {

        TeamNumber = i;
    }

    public void SetSpawnPoint(Transform spawn)
    {
        mySpawnPoint = spawn;
    }

    public IEnumerator DestroyShip()
    {
        state = PlayerState.Invincible;
        gameObject.GetComponent<Check>().active = false;
        Instantiate(Explosion, transform.position, transform.rotation);
        gameObject.collider.enabled = false;
        foreach (Renderer rend in childObjects)
        {
            rend.enabled = false;
        }
        yield return new WaitForSeconds(3f);
        transform.position = mySpawnPoint.position;
        transform.rotation = mySpawnPoint.rotation;
        PlayerHP = 100;
        SetHealth(0);
        gameObject.collider.enabled = true;
        foreach (Renderer rend in childObjects)
        {
            rend.enabled = true;
        }
        rigidbody.velocity = new Vector3(0,0,0);
        rigidbody.angularVelocity = new Vector3(0,0,0);
        gameObject.GetComponent<Check>().active = true;
        state = PlayerState.Playing;

    }

    public void SetHealth(int health)
    {
        PlayerHP += health;

        if (PlayerHP <= 0)
            StartCoroutine(DestroyShip());
        /*
        if (PlayerHP <= 0)
        {
            StartCoroutine(DestroyShip());     

        }*/
    }

    void OnCollisionEnter(Collision other)
    {
        if (state == PlayerState.Playing)
        {
            if (other.gameObject.tag == "Border" || other.gameObject.tag == "blue" || other.gameObject.tag == "red" || other.gameObject.tag == "yellow" || other.gameObject.tag == "green")
            {
                int Damage = (int)(other.relativeVelocity.magnitude);
                Damage /= 4;
                SetHealth(-Damage);
            }
        }

    }

    [System.Serializable]
    public class Cannon
    {
        public GameObject[] Position;
        public GameObject Projectile;
        public float ReloadTime = 0.5f;

        [HideInInspector]
        public float Timer;


        public void Shoot()
        {
            if (Timer <= 0f )
            {
                foreach (GameObject position in Position)
                {
                    Instantiate(Projectile, position.transform.position, position.transform.rotation);
                    Timer = ReloadTime;
                   
                }
            }
           

        }
        public void Shoot(Transform projTarget)
        {
            if (Timer <= 0f)
            {
                foreach (GameObject position in Position)
                {

                    GameObject Missile = (GameObject)Instantiate(Projectile, position.transform.position, position.transform.rotation);
                    Missile.GetComponent<Missile>().SetTarget(projTarget);
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
    public Cannon MissileCannon;

    [System.Serializable]
    public class MovementSettings
    {
        // What is the maximum speed of this movement?
        public float maxSpeed;

        // What's the acceleration in the positive and negative directions associated with this movement?
        public float positiveAcceleration;
        public float negativeAcceleration;

        // How much drag should we apply when there isn't input for this movement?
        public float dragWhileCoasting;

        // How much drag should we apply to slow down the movement for speeds above maxSpeed?
        public float dragWhileBeyondMaxSpeed;

        // When neither of the above drag factors are in play, how much drag should there normally be?  (Usually very small.)
        public float dragWhileAcceleratingNormally;

        // This function determines which drag variable to use and returns one.
        public float ComputeDrag(float input, Vector3 velocity)
        {
            //Is the input not zero (the 0.01 allows for some error since we're working with floats and they aren't completely precise)
            if (Mathf.Abs(input) > 0.01)
            {
                // Are we greater or less than our max speed? Then return the appropriate drag.
                if (velocity.magnitude > maxSpeed)
                    return dragWhileBeyondMaxSpeed;
                else
                    return dragWhileAcceleratingNormally;
            }
            else
                //If the input is zero, use dragWhileCoasting
                return dragWhileCoasting;
        }
    }

    public MovementSettings positionalMovement;



    // Use this for initialization
    void Start()
    {
        state = PlayerState.Playing;
        LaserCannon.Timer = LaserCannon.ReloadTime;
        MissileCannon.Timer = MissileCannon.ReloadTime;

        int a = 0;
        for (int i = 0; i < 4; i++)
        {
            if (tags[i] == gameObject.tag)
                continue;
            GameObject[]targetsfound = GameObject.FindGameObjectsWithTag(tags[i]);
            for (int j = 0; j < targetsfound.Length; j++)
            {
                targets[j + a*5] = targetsfound[j];
            }

                a++;
        }
    }


    private float timer = 0f;
    public float _time = 10f;
    void Targeting()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            target = targets[Random.Range(0, targets.Length - 1)];
            while(target == null)
                target = targets[Random.Range(0, targets.Length-1)];
            timer = _time;

        }
    }
    // Update is called once per frame
    void Update()
    {
        Targeting();
        while (!target.GetComponent<Check>().active)
        {
            Targeting();
        }
        if (state == PlayerState.Playing && target)
        {
            //transform.LookAt(target.transform);
            Vector3 pos = target.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(pos), 10f * Time.deltaTime);
            
            LaserCannon.CheckTime();
            MissileCannon.CheckTime();
            LaserCannon.Shoot();
            MissileCannon.Shoot(target.transform);
        }
        

    }

    void FixedUpdate()
    {

        if (state == PlayerState.Playing && target)
        {
            float dist = Vector3.Distance(target.transform.position, transform.position);
            // Retrieve input.  Note the use of GetAxisRaw (), which in this case helps responsiveness of the controls.
            // GetAxisRaw () bypasses Unity's builtin control smoothing.
           

            //Use the MovementSettings class to determine which drag constant should be used for the positional movement.
            //Remember the MovementSettings class is a helper class we defined ourselves. See the top of this script.
            rigidbody.drag = positionalMovement.ComputeDrag(Thruster, rigidbody.velocity);

            //Determines which direction the positional and rotational motion is occurring, and then modifies thrust/turn with the given accelerations. 
      

            // Add torque and force to the rigidbody.  Torque will rotate the body and force will move it.
            // Always modify your forces by Time.deltaTime in FixedUpdate (), so if you ever need to change your Time.fixedTime setting,
            // your setup won't break.

            //rigidbody.AddRelativeTorque(Vector3.up * Turn);
            if(dist > 50)
                 rigidbody.AddRelativeForce(Vector3.forward * Thruster * positionalMovement.positiveAcceleration);
            

        }
    }
    
   

}
