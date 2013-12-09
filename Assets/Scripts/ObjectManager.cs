using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour
{

    public GameObject camera;
    public GameObject[] player;
    public GameObject[] Spawnpoints;
    public GameObject[] AISpawnPoints;
    //string[] Tags = { "blue", "red", "yellow", "green" };


    public GameObject[] cameras = new GameObject[4];
    // Use this for initialization
    void Start()
    {
        
        for (int i = 0; i < GameManager.playerCount; i++)
        {
            //setPlayer
            GameObject ply = (GameObject)Instantiate(player[i], Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
            ply.SendMessage("SetPlayerNumber", i, SendMessageOptions.DontRequireReceiver);
            ply.SendMessage("SetSpawnPoint", Spawnpoints[i].transform, SendMessageOptions.DontRequireReceiver);
            //ply.tag = Tags[i];
            //set camera
            GameObject cam = (GameObject)Instantiate(camera, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
            cam.SendMessage("SetTarget", ply.transform, SendMessageOptions.DontRequireReceiver);

            cameras[i] = cam;
            

            if (GameManager.playerCount == 2)
            {
                cam.camera.pixelRect = new Rect(0f, 0f, 0.5f * Screen.width,  Screen.height);
            }
            if (GameManager.playerCount == 2 && i == 1)
            {
                cam.camera.pixelRect = new Rect(0.5f * Screen.width, 0f, 0.5f * Screen.width, Screen.height);
            }
            else if (GameManager.playerCount > 2)
            {
                switch (i)
                {
                    case 0:
                        cam.camera.pixelRect = new Rect(0f, 0f, 0.5f * Screen.width, 0.5f * Screen.height);
                        break;
                    case 1:
                        cam.camera.pixelRect = new Rect(0.5f * Screen.width, 0f, 0.5f * Screen.width, 0.5f * Screen.height);
                        break;
                    case 2:
                        cam.camera.pixelRect = new Rect(0.5f * Screen.width, 0.5f * Screen.height, 0.5f * Screen.width, 0.5f * Screen.height);
                        break;
                    case 3:
                        cam.camera.pixelRect = new Rect(0f, 0.5f * Screen.height, 0.5f * Screen.width, 0.5f * Screen.height);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
}
