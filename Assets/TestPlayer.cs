using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour {
    public int PlayerHP = 100;
    public Transform Spawnpoint;
    float Speed = 0f;
    float Thruster;
    float Turn;
    public float RotationAngle = 60f;
    public float RotationSpeed;
    float yRot, xRot;


    public void SetSpawnPoint(Transform spawn)
    {
        Spawnpoint = spawn;
    }

    public void SetHealth(int health)
    {
        PlayerHP += health;
    }

    [System.Serializable]
    public class Cannon
    {
        public GameObject[] Position;
        public GameObject Projectile;
        public float ReloadTime = 0.5f;
        public float Timer;

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
    public MovementSettings rotationalMovement;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {


        yRot = Input.GetAxisRaw("Horizontal") * RotationAngle;
        xRot = Input.GetAxisRaw("Vertical") * RotationAngle;
        transform.Rotate(Vector3.up, Time.deltaTime * yRot * RotationSpeed);
        //transform.Rotate(Vector3.forward * Time.deltaTime * zRot);
        transform.Rotate(Vector3.right, Time.deltaTime * xRot * RotationSpeed);



        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.rotation.y, zRot)), Time.deltaTime);
        //transform.Rotate(Vector3.up * Time.deltaTime * (LeftThruster - RightThruster) * ThrusterRotation);

        LaserCannon.CheckTime();
        if (Input.GetButton("Jump"))
        {
            LaserCannon.Shoot();
        }

    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.R) && Speed <= 1f)
        {
            Speed = Speed + 0.05f;

        }
        if (Input.GetKey(KeyCode.Q) && Speed > 0f)
        {
            Speed = Speed - 0.05f;
        }


        Thruster = Speed;
        // Retrieve input.  Note the use of GetAxisRaw (), which in this case helps responsiveness of the controls.
        // GetAxisRaw () bypasses Unity's builtin control smoothing.
        //Use the MovementSettings class to determine which drag constant should be used for the positional movement.
        //Remember the MovementSettings class is a helper class we defined ourselves. See the top of this script.
        rigidbody.drag = positionalMovement.ComputeDrag(Thruster, rigidbody.velocity);

        //Then determine which drag constant should be used for the angular movement.
        rigidbody.angularDrag = rotationalMovement.ComputeDrag(Turn, rigidbody.angularVelocity);

        //Determines which direction the positional and rotational motion is occurring, and then modifies thrust/turn with the given accelerations. 
        Thruster *= (Thruster > 0.0) ? positionalMovement.positiveAcceleration : positionalMovement.negativeAcceleration;
        Turn *= (Turn > 0.0) ? rotationalMovement.positiveAcceleration : rotationalMovement.negativeAcceleration;

        // Add torque and force to the rigidbody.  Torque will rotate the body and force will move it.
        // Always modify your forces by Time.deltaTime in FixedUpdate (), so if you ever need to change your Time.fixedTime setting,
        // your setup won't break.

        rigidbody.AddRelativeTorque(Vector3.up * Turn);
        rigidbody.AddRelativeForce(Vector3.forward * Thruster, ForceMode.Acceleration);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10,10,100,50), Speed.ToString());
    }
    void Reset()
    {
        // Set some nice default values for our MovementSettings.
        // Of course, it is always best to tweak these for your specific game.
        positionalMovement.maxSpeed = 3.0f;
        positionalMovement.dragWhileCoasting = 3.0f;
        positionalMovement.dragWhileBeyondMaxSpeed = 4.0f;
        positionalMovement.dragWhileAcceleratingNormally = 0.01f;
        positionalMovement.positiveAcceleration = 50.0f;

        // By default, we don't have reverse thrusters.
        positionalMovement.negativeAcceleration = 0.0f;

        rotationalMovement.maxSpeed = 2.0f;
        rotationalMovement.dragWhileCoasting = 32.0f;
        rotationalMovement.dragWhileBeyondMaxSpeed = 16.0f;
        rotationalMovement.dragWhileAcceleratingNormally = 0.1f;

        // For rotation, acceleration is usually the same in both directions.
        // It could make for interesting unique gameplay if it were significantly
        // different, however!
        rotationalMovement.positiveAcceleration = 50.0f;
        rotationalMovement.negativeAcceleration = 50.0f;
    }

}
