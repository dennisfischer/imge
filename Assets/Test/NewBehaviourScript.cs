using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
    public int life = 100;
	
    public float radius = 10f;
    public float power = 10.0F;
    void Start()
    {
        
    }
	// Update is called once per frame
	void Update () {
/*
        if (Input.anyKeyDown)
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                if (hit && hit.rigidbody)
                    hit.rigidbody.AddExplosionForce(power, explosionPos, radius, 3.0F);

            }
        }
            */
        if (Input.anyKey)
        {
            life--;
            if (life < 0)
                life = 100;
            gameObject.GetComponent<AnimateSprite>().Animate((float)(life/100f));
            
        }

	}
}
