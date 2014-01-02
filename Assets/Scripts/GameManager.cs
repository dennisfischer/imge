using UnityEngine;
using System.Collections;

public class GameManager
{
    private static GameManager instance;
    public int playerCount = 0;
    public int TeamSize = 5;
    private int[] TeamScores= {0,0,0,0};

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

    public void SetPlayerCount(int c)
    {
        playerCount = c;
    }

    public void setTeamScores(int i)
    {
        TeamScores[i]++;
    }

    public int getTeamScores(int i)
    {
        return TeamScores[i];
    }


}
