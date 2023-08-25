using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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
