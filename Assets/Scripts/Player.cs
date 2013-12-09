using UnityEngine;
using System.Collections;

//blue,red, yellow, green
public class Player : MonoBehaviour {
    public int PlayerHP = 100;
    public int PlayerNumber;
    string[] Controllers = { "COM3", "COM4", "COM5", "COM6" };
    public Controller myController;
    float RightThruster, LeftThruster;
    float Thruster;
    float Turn;
    public float RotationAngle = 60f;
    public float RotationSpeed;
    float yRot, xRot;
    public float ThrusterRotation = 200f;
    public GameObject Explosion;
    Transform mySpawnPoint;


    void OnGUI()
    {

        if (GameManager.playerCount > 2)
        {
            if (PlayerNumber == 0)
                GUI.Label(new Rect(Screen.width / 4, Screen.height / 2, 100, 50), PlayerHP.ToString());
            else if (PlayerNumber == 1)
                GUI.Label(new Rect(Screen.width / 4 * 3, Screen.height / 2, 100, 50), PlayerHP.ToString());
            else if (PlayerNumber == 2)
                GUI.Label(new Rect(Screen.width / 4 * 3, 0, 100, 50), PlayerHP.ToString());
            else if (PlayerNumber == 3)
                GUI.Label(new Rect(Screen.width / 4, 0, 100, 50), PlayerHP.ToString());
        }

    }
    public void SetPlayerNumber(int i)
    {
        PlayerNumber = i;
    }

    public void SetSpawnPoint(Transform spawn)
    {
        mySpawnPoint = spawn;
    }

    public void SetHealth(int health)
    {
        PlayerHP += health;
        if (PlayerHP <= 0)
        {
            collider.enabled = false;
            Instantiate(Explosion, transform.position, transform.rotation);
            transform.position = mySpawnPoint.position;
            transform.rotation = mySpawnPoint.rotation;
            PlayerHP = 100;
            collider.enabled = true;

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
            if(Timer <= 0f){
               foreach(GameObject position in Position){
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
	void Start () {
        myController = (Controller)gameObject.GetComponent("Controller");
        myController.Init(Controllers[PlayerNumber]);
        myController.LEDOn(PlayerNumber);
	}
	
	// Update is called once per frame
	void Update () {
        myController.CheckSlidersTurners();
        myController.CheckAccelerator();
        myController.CheckButtons();



        LeftThruster = myController.GetSlider(0);
        RightThruster = myController.GetSlider(1);


        yRot = -(myController.yAcc) * RotationAngle;
        xRot = myController.xAcc * RotationAngle;
        transform.Rotate(Vector3.up, Time.deltaTime * yRot * RotationSpeed/2);
        //transform.Rotate(Vector3.forward * Time.deltaTime * zRot);
        transform.Rotate(Vector3.right, Time.deltaTime * xRot * RotationSpeed);
         


        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.rotation.y, zRot)), Time.deltaTime);
        //transform.Rotate(Vector3.up * Time.deltaTime * (LeftThruster - RightThruster) * ThrusterRotation);

        LaserCannon.CheckTime();
        if (myController.GetButton(5) || myController.GetButton(6))
        {
            LaserCannon.Shoot();
        }
         
	}

    void FixedUpdate()
    {
        // Retrieve input.  Note the use of GetAxisRaw (), which in this case helps responsiveness of the controls.
        // GetAxisRaw () bypasses Unity's builtin control smoothing.
        Thruster = LeftThruster + RightThruster;
        Turn = LeftThruster - RightThruster;

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

        //rigidbody.AddRelativeTorque(Vector3.up * Turn);
        rigidbody.AddRelativeForce(Vector3.forward * Thruster, ForceMode.Acceleration);
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

    void OnApplicationQuit()
    {
        //myController.TurnOffLEDs();
    }
}
