using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{

   


    public  Transform _target;

    public void SetTarget(Transform theTarget)
    {
        _target = theTarget;
    }

    
    void FixedUpdate()
    {
        
        
        //_rigidbody.velocity = transform.forward * _velocity;

        StartCoroutine(Wait());
         

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0f);
        if (_target && _target.gameObject.GetComponent<Check>().active)
        {
            Vector3 pos = _target.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(pos), 4f * Time.deltaTime);
        }
                
    }

}
