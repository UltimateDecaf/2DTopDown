using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    public string playerName;
    public string playerAuthId;

    public PlayerData(string playerName, string playerAuthId)
    {
        this.playerName = playerName;
        this.playerAuthId = playerAuthId;
    }
}
