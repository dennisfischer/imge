using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    Controller contr;
	// Use this for initialization
	void Start () {
        GameManager.Instance.AI = true;
        contr = (Controller)gameObject.GetComponent("Controller");
        contr.Init("COM3");
	}
	
	// Update is called once per frame
	void Update () {
        contr.CheckButtons();
        if (contr.GetButtonDown(5))
        {
            GameManager.Instance.AI = true;
        }
        if (contr.GetButtonDown(6))
        {
            GameManager.Instance.AI = false;
        }

        if (contr.GetButtonDown(2))
        {
            GameManager.Instance.SetPlayerCount(1);
            Application.LoadLevel(1);
        }
        if (contr.GetButtonDown(3))
        {
            GameManager.Instance.SetPlayerCount(2);
            Application.LoadLevel(1);
        }
        if (contr.GetButtonDown(4))
        {
            GameManager.Instance.SetPlayerCount(3);
            Application.LoadLevel(1);
        }
        if (contr.GetButtonDown(1))
        {
            GameManager.Instance.SetPlayerCount(4);
            Application.LoadLevel(1);
        }
        

	}

    void OnGUI()
    {
        /*
        if (GUI.Button(new Rect(10, 10, 100, 50), "1 Player"))
        {
            GameManager.Instance.SetPlayerCount(1);
            Application.LoadLevel(1);
        }
        if (GUI.Button(new Rect(10, 60, 100, 50), "2 Player"))
        {
            GameManager.Instance.SetPlayerCount(2);
            Application.LoadLevel(1);
        }
        if (GUI.Button(new Rect(10, 110, 100, 50), "3 Player"))
        {
            GameManager.Instance.SetPlayerCount(3);
            Application.LoadLevel(1);
        }
        if (GUI.Button(new Rect(10, 160, 100, 50), "4 Player"))
        {
            GameManager.Instance.SetPlayerCount(4);
            contr.TurnOff();
            Application.LoadLevel(1);
        }*/
        
    }
}
