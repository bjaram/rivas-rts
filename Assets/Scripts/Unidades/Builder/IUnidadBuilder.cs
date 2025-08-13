using UnityEngine;

public interface IUnidadBuilder
{
    void Reset();
    void SetPrefab(GameObject basePrefab);
    void SetSpawn(Vector3 position, Quaternion rotation);
    void SetStats(int vida, float velocidad);
    void SetApariencia(Material materialOpcional = null);
    IUnidad Build();
}
