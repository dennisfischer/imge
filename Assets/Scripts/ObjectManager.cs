using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour
{
    public int spawnOffset = 300;
    public GameObject[] camera;
    public GameObject[] player;
    public GameObject[] AI;
    public GameObject[] Spawnpoints;
    //string[] Tags = { "blue", "red", "yellow", "green" };


    // Use this for initialization
    void Start()
    {

        //Set AI and Players
            for (int i = 0; i < GameManager.Instance.playerCount; i++)
            {
                //setPlayer
                GameObject ply = (GameObject)Instantiate(player[i], Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                Player plyer = ply.GetComponent<Player>();
                plyer.SetPlayerNumber(i);
                plyer.SetSpawnPoint(Spawnpoints[i].transform);

                //ply.tag = Tags[i];
                //set camera
                GameObject cam = camera[i];
                cam.SetActive(true);
                cam.SendMessage("SetTarget", ply.transform, SendMessageOptions.DontRequireReceiver);
                AnimateSprite[] HUDelements = cam.GetComponentsInChildren<AnimateSprite>();
                foreach (AnimateSprite sprite in HUDelements)
                {
                    if (sprite.gameObject.name == "Laser")
                    {
                        ply.GetComponent<Player>().HUDWeapon = sprite;
                    }
                    else if (sprite.gameObject.name == "Life")
                    {
                        ply.GetComponent<Player>().HUDLife = sprite;
                    }

                }
                //Ai SPawn

                GameObject newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(0, spawnOffset, 0), Spawnpoints[i].transform.rotation);
                GameObject ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                AI _ki = ki.GetComponent<AI>();
                _ki.SetPlayerNumber(i);
                _ki.SetSpawnPoint(newSpawn.transform);


                newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(0, -spawnOffset, 0), Spawnpoints[i].transform.rotation);
                ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                _ki = ki.GetComponent<AI>();
                _ki.SetPlayerNumber(i);
                _ki.SetSpawnPoint(newSpawn.transform);

                if (Mathf.Abs(Spawnpoints[i].transform.localPosition.x) == 450)
                {
                    newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(0, 0, -spawnOffset), Spawnpoints[i].transform.rotation);
                    ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                    //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                    _ki = ki.GetComponent<AI>();
                    _ki.SetPlayerNumber(i);
                    _ki.SetSpawnPoint(newSpawn.transform);

                    newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(0, 0, spawnOffset), Spawnpoints[i].transform.rotation);
                    ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                    //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                    _ki = ki.GetComponent<AI>();
                    _ki.SetPlayerNumber(i);
                    _ki.SetSpawnPoint(newSpawn.transform);
                }
                else if(Mathf.Abs(Spawnpoints[i].transform.localPosition.z) == 450)
                {
                    newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(spawnOffset, 0, 0), Spawnpoints[i].transform.rotation);
                    ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                    //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                    _ki = ki.GetComponent<AI>();
                    _ki.SetPlayerNumber(i);
                    _ki.SetSpawnPoint(newSpawn.transform);

                    newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(-spawnOffset, 0, 0), Spawnpoints[i].transform.rotation);
                    ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                    //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                    _ki = ki.GetComponent<AI>();
                    _ki.SetPlayerNumber(i);
                    _ki.SetSpawnPoint(newSpawn.transform);
                }

                if (GameManager.Instance.playerCount == 2)
                {
                    cam.camera.pixelRect = new Rect(0f, 0f, 0.5f * Screen.width, Screen.height);
                }
                if (GameManager.Instance.playerCount == 2 && i == 1)
                {
                    cam.camera.pixelRect = new Rect(0.5f * Screen.width, 0f, 0.5f * Screen.width, Screen.height);
                }
                else if (GameManager.Instance.playerCount > 2)
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
        
         for (int i = 4 - GameManager.Instance.playerCount; i > 0; i--)
         {
             GameObject newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(0, spawnOffset, 0), Spawnpoints[i].transform.rotation);
             GameObject ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
             //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
             AI _ki = ki.GetComponent<AI>();
             _ki.SetPlayerNumber(i);
             _ki.SetSpawnPoint(newSpawn.transform);


            newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(0, -spawnOffset, 0), Spawnpoints[i].transform.rotation);
             ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
             //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
             _ki = ki.GetComponent<AI>();
             _ki.SetPlayerNumber(i);
             _ki.SetSpawnPoint(newSpawn.transform);

             newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
             ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
             //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
             _ki = ki.GetComponent<AI>();
             _ki.SetPlayerNumber(i);
             _ki.SetSpawnPoint(newSpawn.transform);

               
                if (Mathf.Abs(Spawnpoints[i].transform.localPosition.x) == 450)
                {
                    newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(0, 0, -spawnOffset), Spawnpoints[i].transform.rotation);
                    ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                    //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                    _ki = ki.GetComponent<AI>();
                    _ki.SetPlayerNumber(i);
                    _ki.SetSpawnPoint(newSpawn.transform);

                    newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(0, 0, spawnOffset), Spawnpoints[i].transform.rotation);
                    ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                    //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                    _ki = ki.GetComponent<AI>();
                    _ki.SetPlayerNumber(i);
                    _ki.SetSpawnPoint(newSpawn.transform);
                }
                else if (Mathf.Abs(Spawnpoints[i].transform.localPosition.z) == 450)
                {
                    newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(spawnOffset, 0, 0), Spawnpoints[i].transform.rotation);
                    ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                    //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                    _ki = ki.GetComponent<AI>();
                    _ki.SetPlayerNumber(i);
                    _ki.SetSpawnPoint(newSpawn.transform);

                    newSpawn = (GameObject)Instantiate(Spawnpoints[i], Spawnpoints[i].transform.position + new Vector3(-spawnOffset, 0, 0), Spawnpoints[i].transform.rotation);
                    ki = (GameObject)Instantiate(AI[i], newSpawn.transform.position, Spawnpoints[i].transform.rotation);
                    //GameObject tar = (GameObject)Instantiate(targeter, Spawnpoints[i].transform.position, Spawnpoints[i].transform.rotation);
                    _ki = ki.GetComponent<AI>();
                    _ki.SetPlayerNumber(i);
                    _ki.SetSpawnPoint(newSpawn.transform);
                }
                 
            }
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
}
