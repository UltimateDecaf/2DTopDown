using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    public string playerName;

    public PlayerData(string playerName)
    {
        this.playerName = playerName;
    }
}
