using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static int playerCount = 0;
    ObjectManager objManager;

	// Use this for initialization
	void Start () {
        
        GameObject.DontDestroyOnLoad(gameObject);
	}

    public static void SetPlayerCount(int c)
    {
        playerCount = c;
    }

      
                
    

	// Update is called once per frame
	void Update () {
	
	}
}
