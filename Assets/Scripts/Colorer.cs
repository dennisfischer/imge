using UnityEngine;
using System.Collections;

public class Colorer : MonoBehaviour {
    public Color color;
	// Use this for initialization
	void Start () {
        gameObject.renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
