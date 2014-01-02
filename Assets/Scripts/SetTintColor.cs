using UnityEngine;
using System.Collections;

public class SetTintColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Color guicolor = new Color();
        if (gameObject.layer == 8){
            guicolor = Color.blue;
        }

        else if (gameObject.layer == 9){
            guicolor = Color.red;
        }

        else if (gameObject.layer == 10) {
            guicolor = Color.yellow;    
        }

        else if (gameObject.layer == 11) {
            guicolor = Color.green;
        }

            
        guicolor.a = 80f / 255f;

        
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if(child.renderer){
                child.renderer.material.SetColor("_TintColor", guicolor);
                child.renderer.material.color = guicolor;
                //child.renderer.material.SetColor("_MainColor", guicolor);
            }
        }
	}
	
	
}
