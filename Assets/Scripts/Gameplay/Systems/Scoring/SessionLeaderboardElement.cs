using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct SessionLeaderboardElement : INetworkSerializable, IEquatable<SessionLeaderboardElement>
{
    public FixedString32Bytes PlayerName;
    public int Score;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref Score);
    }
    public bool Equals(SessionLeaderboardElement other)
    {
       return PlayerName.Equals(other.PlayerName) &&
            Score == other.Score;
    }



  
}
