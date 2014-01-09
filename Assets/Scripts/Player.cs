using UnityEngine;
using System.Collections;

//blue,red, yellow, green
public class Player : MonoBehaviour {
   // float TurnerSimulatorLeft, TurnerSimulatorRight;
    public int PlayerHP = 100;
    public int PlayerNumber;
    public Material GUILife;
    public GUIStyle guiStyle;
    int width, height;
    string[] Controllers = { "COM3", "COM4", "COM5", "COM6" };
    public Controller myController;
    public AnimateSprite HUDLife;
    public AnimateSprite HUDWeapon;
    float RightThruster, LeftThruster;
    float Thruster;
    float Turn;
    public float RotationAngle = 60f;
    public float RotationSpeed;
    float yRot, xRot;
    public GameObject Explosion;
    Transform mySpawnPoint;
    public Renderer[] childObjects = new Renderer[5];

    public AudioClip explosionSound;
    public AudioClip laserSound;
    public AudioClip missileSound;
    public AudioClip impactSound;

   
    int targetNumber = 0;
    GameObject target;
    [HideInInspector]
    public GameObject indicator;
    ArrayList SeekerList = new ArrayList();

    void OnGUI()
    {
        if (GameManager.Instance.playerCount == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - width / 2, height , width, height), PlayerHP.ToString(), guiStyle);
            GUI.Label(new Rect(10, 10, width, height), LaserCannon.CurrentAmmo.ToString(), guiStyle);
            GUI.Label(new Rect(Screen.width - 2 * width, height, width, height), GameManager.Instance.getTeamScores(PlayerNumber).ToString(), guiStyle);
        }
        if (GameManager.Instance.playerCount  == 2)
        {
            if (PlayerNumber == 0)
            {
                GUI.Label(new Rect(Screen.width / 4 - width / 2, height, width, height), PlayerHP.ToString(), guiStyle);

                GUI.Label(new Rect(Screen.width/2 - 2 * width, height, width, height), GameManager.Instance.getTeamScores(PlayerNumber).ToString(), guiStyle);
            }

            else if (PlayerNumber == 1)
            {
                GUI.Label(new Rect(Screen.width / 4 * 3 - width / 2, height, width, height), PlayerHP.ToString(), guiStyle);

                GUI.Label(new Rect(Screen.width - 2 * width, height, width, height), GameManager.Instance.getTeamScores(PlayerNumber).ToString(), guiStyle);
            }

        }

        if (GameManager.Instance.playerCount > 2)
        {
            if (PlayerNumber == 0)
            {
                //Graphics.DrawTexture(new Rect(Screen.width / 4 -100, Screen.height / 2 + 10, 200, 200), textures[SpriteNumber], GUILife);
                GUI.Label(new Rect(Screen.width / 4 - width /2, Screen.height / 2 , width, height), PlayerHP.ToString(), guiStyle);
             
                GUI.Label(new Rect(Screen.width / 2 - 2* width , Screen.height / 2, width, height), GameManager.Instance.getTeamScores(PlayerNumber).ToString(), guiStyle);
            }

            else if (PlayerNumber == 1)
            {
                //Graphics.DrawTexture(new Rect(Screen.width / 4 * 3 -100, Screen.height / 2 + 10, 200, 200), textures[SpriteNumber], GUILife);
                GUI.Label(new Rect(Screen.width / 4 * 3 - width / 2, Screen.height / 2, width, height), PlayerHP.ToString(), guiStyle);

                GUI.Label(new Rect(Screen.width / 2 + width, Screen.height / 2, width, height), GameManager.Instance.getTeamScores(PlayerNumber).ToString(), guiStyle);
            }

            else if (PlayerNumber == 2)
            {
                //Graphics.DrawTexture(new Rect(Screen.width / 4 * 3 -100, 10, 200, 200), textures[SpriteNumber], GUILife);
                GUI.Label(new Rect(Screen.width / 4 * 3 - width / 2, 0, width, height), PlayerHP.ToString(), guiStyle);

                GUI.Label(new Rect(Screen.width / 2 + width, Screen.height / 2 - 1.5f* height, width, height), GameManager.Instance.getTeamScores(PlayerNumber).ToString(), guiStyle);
            }

            else if (PlayerNumber == 3)
            {
                //Graphics.DrawTexture(new Rect(Screen.width / 4 -100, 10, 200, 200), textures[SpriteNumber], GUILife);
                GUI.Label(new Rect(Screen.width / 4 - width / 2, 0, width, height), PlayerHP.ToString(), guiStyle);

                GUI.Label(new Rect(Screen.width / 2 - 2* width, Screen.height / 2 - 1.5f* height, width, height), GameManager.Instance.getTeamScores(PlayerNumber).ToString(), guiStyle);
            }
        }

    }

    public enum PlayerState {
        Invincible, Playing
    }
    PlayerState state;

    public void SetPlayerNumber(int i)
    {
        
        PlayerNumber = i;
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
        rigidbody.velocity = new Vector3(0, 0, 0);
        rigidbody.angularVelocity = new Vector3(0, 0, 0);
        gameObject.GetComponent<Check>().active = true;
        state = PlayerState.Playing;
        
    }

    public void SetHealth(int health)
    {
        if (health < 0)
        {
            audio.PlayOneShot(impactSound);
        }
        PlayerHP += health;


        float LifePercentage = (float)PlayerHP / 100f;
        if (LifePercentage < 0f)
            LifePercentage = 0f;
        else if (LifePercentage > 1f)
            LifePercentage = 1f;
        HUDLife.Animate(LifePercentage);
        if (PlayerHP <= 0)
        {
            StartCoroutine(DestroyShip());
            audio.PlayOneShot(explosionSound);
        }
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
        public int CurrentAmmo = 100;
        public float RechargeRate = 0.25f;
        public int MaxAmmo = 100;

        public bool overheated;
        private float Timer;
        private float Timer2;

        public bool Shoot()
        {
            bool shooted = false;
            if(Timer <= 0f && CurrentAmmo > 0 && !overheated){
               foreach(GameObject position in Position){
                 Instantiate(Projectile, position.transform.position, position.transform.rotation);
                 Timer = ReloadTime;
                 CurrentAmmo--;
                 shooted = true;
              }
            }
            if (CurrentAmmo <= 0)
            {
                overheated = true;
            }

            return shooted;
            
        }
        public bool Shoot(Transform projTarget)
        {
            bool shooted = false;

            if (Timer <= 0f && CurrentAmmo > 0 && !overheated)
            {
                foreach (GameObject position in Position)
                {
                   
                    GameObject Missile =(GameObject) Instantiate(Projectile, position.transform.position, position.transform.rotation);
                    Missile.GetComponent<Missile>().SetTarget(projTarget);
                    Timer = ReloadTime;
                    CurrentAmmo--;
                    shooted = true;
                }
            }
            if (CurrentAmmo <= 0)
            {
                overheated = true;
            }
            return shooted;

        }

        public void Recharge()
        {
            if (Timer2 < 0 && CurrentAmmo< MaxAmmo)
            {
                CurrentAmmo++;
                Timer2 = RechargeRate;

                if (CurrentAmmo == MaxAmmo && overheated)
                {
                    overheated = false;
                }
                    
            }
        }
        
        public void CheckTime()
        {
            Timer -= Time.deltaTime;
            Timer2 -= Time.deltaTime;
            Recharge();
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
    public MovementSettings rotationalMovement;

    enum Weaponstate { Laser, Missile }
    private Weaponstate weaponState;

	// Use this for initialization
	void Start () {
        state = PlayerState.Playing;
        width = Screen.height / 25;
        height = (int)(width / 1.5);
        guiStyle.fontSize = Screen.height / 25;
        if (GameManager.Instance.playerCount < 3)
        {
            guiStyle.fontSize *= 2;
            height = height * 8 /10;
        }
        indicator = GetComponentInChildren<Crosshair>().gameObject;
        indicator.renderer.enabled = false;
        
        myController = (Controller)gameObject.GetComponent("Controller");
        myController.Init(Controllers[PlayerNumber]);
        myController.LEDOn(PlayerNumber);
        
	}
	
	// Update is called once per frame
	void Update () {

        if (myController.GetButtonDown(2) && PlayerNumber == 0)
        {
            Application.LoadLevel(0);
        }
        if (state == PlayerState.Playing)
        {

            myController.CheckSlidersTurners();
            myController.CheckAccelerator();
            myController.CheckButtons();

            if (SeekerList.Count > 0)
            {
                targetNumber = (int)(myController.GetTurner(0) * SeekerList.Count);
                targetNumber %= SeekerList.Count;
                target = (GameObject)SeekerList[targetNumber];
                if (target.transform)
                    indicator.GetComponent<Crosshair>().SetTarget(target.transform);
            }


            if (myController.GetTurner(1) == 1f)
            {
                weaponState = Weaponstate.Missile;
            }
            else if (myController.GetTurner(1) == 0f)
            {
                weaponState = Weaponstate.Laser;
            }
            LeftThruster = myController.GetSlider(0);
            RightThruster = myController.GetSlider(1);


            yRot = (myController.yAcc) * RotationAngle;
            xRot = myController.xAcc * RotationAngle;
            transform.Rotate(Vector3.forward, Time.deltaTime * yRot * RotationSpeed / 2);
            transform.Rotate(Vector3.right, Time.deltaTime * xRot * RotationSpeed);

            LaserCannon.CheckTime();
            MissileCannon.CheckTime();

            if ((myController.GetButton(5) || myController.GetButton(6)) && weaponState == Weaponstate.Laser)
            {
                if (LaserCannon.Shoot())
                {
                    audio.PlayOneShot(laserSound);
                }
            }


            else if ((myController.GetButton(5) || myController.GetButton(6)) && weaponState == Weaponstate.Missile)
            {
                if (target != null)
                {
                    if (MissileCannon.Shoot(target.transform))
                    {
                        audio.PlayOneShot(missileSound);
                    }
                }
                if (target == null)
                {
                    if (MissileCannon.Shoot(null))
                    {
                        audio.PlayOneShot(missileSound);
                    }
                }
                

            }

            if (weaponState == Weaponstate.Laser)
            {
                if (!LaserCannon.overheated)
                    HUDWeapon.Animate(1);
                else if (LaserCannon.overheated)
                    HUDWeapon.Animate(0.75f);
            }

            else if (weaponState == Weaponstate.Missile)
            {
                if (!MissileCannon.overheated)
                    HUDWeapon.Animate(0.4f);
                else if (MissileCannon.overheated)
                    HUDWeapon.Animate(0f);
            }
            /*
            Mathf.Clamp(TurnerSimulatorLeft, 0f, 1f);
            Mathf.Clamp(TurnerSimulatorRight, 0f, 1f);
            if (Input.GetKey(KeyCode.O))
                TurnerSimulatorRight += 0.1f;
            if (Input.GetKey(KeyCode.L))
                TurnerSimulatorRight -= 0.1f;
            if (Input.GetKey(KeyCode.I))
                TurnerSimulatorLeft += 0.1f;
            if (Input.GetKey(KeyCode.K))
                TurnerSimulatorLeft -= 0.1f;

            if (SeekerList.Count > 0)
            {
                targetNumber = (int)(TurnerSimulatorLeft * SeekerList.Count);
                targetNumber %= SeekerList.Count;
                target = (GameObject)SeekerList[targetNumber];
                if(target.transform)
                indicator.GetComponent<Crosshair>().SetTarget(target.transform);
            }
            

            if (TurnerSimulatorRight > 0.5f)
            {
                weaponState = Weaponstate.Missile;
            }
            else if (TurnerSimulatorRight <= 0.5f)
            {
                weaponState = Weaponstate.Laser;
            }

            xRot = Input.GetAxisRaw("Vertical") * RotationAngle;
            transform.Rotate(Vector3.right, Time.deltaTime * xRot * RotationSpeed);
            yRot = Input.GetAxisRaw("Horizontal") * RotationAngle;
            transform.Rotate(Vector3.up, Time.deltaTime * yRot * RotationSpeed / 2);

            LaserCannon.CheckTime();
            MissileCannon.CheckTime();
            LeftThruster = Input.GetAxisRaw("Vertical2");
            RightThruster = Input.GetAxisRaw("Vertical2");

            if (Input.GetKey(KeyCode.Space) && weaponState == Weaponstate.Laser)
            {
                if (LaserCannon.Shoot())
                {
                    audio.PlayOneShot(laserSound);
                }
            }


            else if (Input.GetKey(KeyCode.Space) && weaponState == Weaponstate.Missile)
            {
                if (MissileCannon.Shoot(target.transform))
                {
                    audio.PlayOneShot(missileSound);
                }
                
            }

            if (weaponState == Weaponstate.Laser) {
                if(!LaserCannon.overheated)
                    HUDWeapon.Animate(1);
                else if (LaserCannon.overheated)
                    HUDWeapon.Animate(0.75f);
            }

            else if (weaponState == Weaponstate.Missile)
            {
                if(!MissileCannon.overheated)
                    HUDWeapon.Animate(0.4f);
                else if (MissileCannon.overheated)
                    HUDWeapon.Animate(0f);
            }
               
            */   
            
            /*
            if (myController.GetButton(5) || myController.GetButton(6))
            {
                LaserCannon.Shoot();
            }
             */
        }
        

       
         
	}

    void FixedUpdate()
    {

        if (state == PlayerState.Playing)
        {

            // Retrieve input.  Note the use of GetAxisRaw (), which in this case helps responsiveness of the controls.
            // GetAxisRaw () bypasses Unity's builtin control smoothing.
            Thruster = LeftThruster + RightThruster;
            //Turn = LeftThruster - RightThruster;

            //Use the MovementSettings class to determine which drag constant should be used for the positional movement.
            //Remember the MovementSettings class is a helper class we defined ourselves. See the top of this script.
            rigidbody.drag = positionalMovement.ComputeDrag(Thruster, rigidbody.velocity);

            //Then determine which drag constant should be used for the angular movement.
           // rigidbody.angularDrag = rotationalMovement.ComputeDrag(Turn, rigidbody.angularVelocity);

            //Determines which direction the positional and rotational motion is occurring, and then modifies thrust/turn with the given accelerations. 
            Thruster *= (Thruster > 0.0) ? positionalMovement.positiveAcceleration : positionalMovement.negativeAcceleration;
            //Turn *= (Turn > 0.0) ? rotationalMovement.positiveAcceleration : rotationalMovement.negativeAcceleration;

            // Add torque and force to the rigidbody.  Torque will rotate the body and force will move it.
            // Always modify your forces by Time.deltaTime in FixedUpdate (), so if you ever need to change your Time.fixedTime setting,
            // your setup won't break.

            //rigidbody.AddRelativeTorque(Vector3.up * Turn);
            rigidbody.AddRelativeForce(Vector3.forward * Thruster, ForceMode.Acceleration);

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == gameObject.tag)
            return;
        if (other.isTrigger)
            return;
        if (other.gameObject.name == "Border" || other.gameObject.tag == "Untagged")
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
   

    void OnApplicationQuit()
    {
        myController.TurnOff();
    }
}
