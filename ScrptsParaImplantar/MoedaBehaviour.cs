using UnityEngine;
using System.Collections;

public class MoedaBehaviour : MonoBehaviour
{
    private ItemSpawner spawner;

    public void SetSpawner(ItemSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnMoedaDestruida();
        }
    }
}
