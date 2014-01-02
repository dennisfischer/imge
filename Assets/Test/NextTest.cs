using UnityEngine;
using System.Collections;

public class NextTest : MonoBehaviour {
    public float sensorRange = 150f;
    public float sensorAngle = 60f;
    public float updateFrequency = 0f;
    public bool oneTarget = false;

    public Transform currentTarget;
    Transform mTrans;
    float mNextUpdate = 0f;
    float mTimeSinceTarget = 0f;
    Vector3 mTurn = Vector3.zero;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        float time = Time.time;

        if (mNextUpdate < time)
        {
            mNextUpdate = time + updateFrequency;

            // Find the most optimal target ahead of the missile
           

            if (currentTarget != null)
            {
                // Calculate local space direction
                Vector3 dir = (currentTarget.transform.position - mTrans.position);
                float dist = dir.magnitude;

                dir *= 1.0f / dist;
                dir = Quaternion.Inverse(mTrans.rotation) * dir;

                // Make the missile turn slower if it's far away from the target, and faster when it's close
                float turnSensivitity = 0.5f + 2.5f * (1.0f - dist / sensorRange);

                // Calculate the turn amount based on the direction
                mTurn.x = Mathf.Clamp(dir.y * turnSensivitity, -1f, 1f);
                mTurn.y = Mathf.Clamp(dir.x * turnSensivitity, -1f, 1f);

                // Locked on target
                mTimeSinceTarget = 0f;
            }
            else
            {
                // No target lock -- keep track of how long it has been
                mTimeSinceTarget += updateFrequency + Time.deltaTime;
            }

            
        }
	}
}
