using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    Controller contr;
	// Use this for initialization
	void Start () {
        contr = (Controller)gameObject.GetComponent("Controller");
        contr.Init("COM3");
	}
	
	// Update is called once per frame
	void Update () {
        contr.CheckButtons();
        if (contr.GetButtonDown(2))
        {
            GameManager.SetPlayerCount(1);
            Application.LoadLevel(1);
        }
        if (contr.GetButtonDown(3))
        {
            GameManager.SetPlayerCount(2);
            Application.LoadLevel(1);
        }
        if (contr.GetButtonDown(4))
        {
            GameManager.SetPlayerCount(3);
            Application.LoadLevel(1);
        }
        if (contr.GetButtonDown(1))
        {
            GameManager.SetPlayerCount(4);
            Application.LoadLevel(1);
        }
        

	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "1 Player"))
        {
            GameManager.SetPlayerCount(1);
            Application.LoadLevel(1);
        }
        if (GUI.Button(new Rect(10, 60, 100, 50), "2 Player"))
        {
            GameManager.SetPlayerCount(2);
            Application.LoadLevel(1);
        }
        if (GUI.Button(new Rect(10, 110, 100, 50), "3 Player"))
        {
            GameManager.SetPlayerCount(3);
            Application.LoadLevel(1);
        }
        if (GUI.Button(new Rect(10, 160, 100, 50), "4 Player"))
        {
            GameManager.SetPlayerCount(4);
            contr.TurnOff();
            Application.LoadLevel(1);
        }
        
    }
}
