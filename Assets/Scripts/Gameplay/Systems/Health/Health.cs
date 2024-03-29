using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

//Based on Nathan Farrer's project
/*
 Lari Basangov (me) has implemented:
- EnemyDie();
- EnemyDieClientRpc();
- PlayerDie();
- UpdatePlayerScripts();
 */
public class Health : NetworkBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    private bool isDead;
    public Action<Health> OnDie;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            CurrentHealth.Value = MaxHealth;
            OnDie += EnemyDie;
            OnDie += PlayerDie;
        }
    }
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnDie -= EnemyDie;
            OnDie -= PlayerDie;
        }
    }
    private void EnemyDie(Health health)
    {
        if (!IsServer) { return; }
        Debug.Log("EnemyDie method invoked");
        if (health.CurrentHealth.Value <= 0)
        {
<<<<<<< HEAD
            if (gameObject.CompareTag("Player"))
            {
                Debug.Log("EnemyDie mehtod -- player");
                // isDead = true;
                gameObject.SetActive(false);
                gameObject.transform.position = Vector3.zero;
                gameObject.SetActive(true);
                health.CurrentHealth.Value = 100;
                isDead = false;
            }
            else if (gameObject.CompareTag("Enemy"))
=======
            if (gameObject.CompareTag("Enemy"))
>>>>>>> sessionleaderboard-fixes
            {
                Debug.Log("EnemyDie method -- enemy");
                EnemyDieClientRpc();
                Destroy(gameObject);

            }

        }

    }

    private void PlayerDie(Health health)
    {
        if (!IsServer) { return; }
        if (health.CurrentHealth.Value <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                BulletShooter shooterScript = gameObject.GetComponent<BulletShooter>();
                PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();
                isDead = true;
                UpdatePlayerScripts(shooterScript, movement);

            }

        }

    }

    // Update is called once per frame

    public void Damage(int damage)
    {
        ModifyHealth(-damage);
    }

    public void Damage(int damage, GameObject playerToScore)
    {
        ModifyHealth(-damage);
        PlayerScore playerScore = playerToScore.GetComponent<PlayerScore>();
        playerScore.score.Value += 15;
    }

    public void Heal(int heal)
    {
        ModifyHealth(heal);
    }

    public void ModifyHealth(int value)
    {
        if (!isDead)
        {
            int newHealth = CurrentHealth.Value + value;
            CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);
            Debug.Log("Changed health of " + this.gameObject.tag);
            if (CurrentHealth.Value == 0)
            {
                OnDie?.Invoke(this);
                isDead = true;

            }
        }
    }

    private void UpdatePlayerScripts(BulletShooter shooter, PlayerMovement movement)
    {
        shooter.SetIsDead(isDead);
        movement.SetIsDead(isDead);
    }

    [ClientRpc]
    void EnemyDieClientRpc()
    {
        EffectsManager.Instance.ShowParticlesOnDestroy(gameObject.transform.position);
    }
}
