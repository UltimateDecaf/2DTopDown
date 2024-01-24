using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


//Created by Lari Basangov

//This script instantiatess a particle effect prefab after killing an enemy, then it is destroyed. 
public class EffectsManager : NetworkBehaviour
{
    [SerializeField] private GameObject enemyDeathParticleEffectPrefab;
    public static EffectsManager Instance;

    public override void OnNetworkSpawn()
    {
        Instance = this;
    }
    public void ShowParticlesOnDestroy(Vector3 position)
    {
      GameObject go = Instantiate(enemyDeathParticleEffectPrefab, position, Quaternion.identity);
      Invoke("DestroyEffect", 1f);
    }

    private void DestroyEffect()
    {
        GameObject go = GameObject.FindWithTag("ParticleEffects");
        Destroy(go);
    }
}
