using UnityEngine;
using System.Collections;

public class GameManager
{
    private static GameManager instance;
    public static int playerCount = 0;
    public static int TeamSize = 10;
    public static int[] TeamScores= {500,500,500,500};

    private GameManager() {
        if (instance != null)
            return;
        else
            instance = this;
    
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    public static void SetPlayerCount(int c)
    {
        playerCount = c;
    }

    public static void setTeamScores(int i)
    {
        TeamScores[i]--;
    }


}
