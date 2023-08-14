using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
   [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    private bool isDead;
    public Action<Health> OnDie;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        CurrentHealth.Value = MaxHealth;
    }

    // Update is called once per frame

    public void Damage(int damage)
    {
        ModifyHealth(-damage);
    }

    public void Heal(int heal)
    {
        ModifyHealth(heal);
    }

    public void ModifyHealth(int value)
    {
        if (isDead) { return; }

        int newHealth = CurrentHealth.Value + value;
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);
        if(CurrentHealth.Value == 0) 
        {
            OnDie?.Invoke(this);
            isDead = true;

        }
    }
}
