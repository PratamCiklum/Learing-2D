using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string playerName;
    public string playerId;
    public int highScore;

    public Player(string playerName)
    {
        this.playerName = playerName;
        this.playerId = System.Guid.NewGuid().ToString();
        this.highScore = 0;
    }
    
    public static Player CreatePlayer(string name)
    {
        if(name.Length >= 10)
        {
            Debug.Log("Player name is too long. No Player Created.");
            return null;
        }
        return new Player(name);
    }

    public void SetHighScore(int score)
    {
        if(score > this.highScore)
        {
            this.highScore = score;
        }
    }

}
