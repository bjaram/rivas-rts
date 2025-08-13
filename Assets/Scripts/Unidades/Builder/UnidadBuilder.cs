using UnityEngine;
using UnityEngine.AI;

public class UnidadBuilder : IUnidadBuilder
{
    private GameObject _prefab;
    private Vector3 _spawnPos;
    private Quaternion _spawnRot = Quaternion.identity;
    private int _vida = 100;
    private float _vel = 3f;
    private Material _mat;

    // Radio de búsqueda de NavMesh para el spawn
    private const float NAVMESH_PICK_RADIUS = 5f;

    public void Reset()
    {
        _prefab = null;
        _spawnPos = Vector3.zero;
        _spawnRot = Quaternion.identity;
        _vida = 100;
        _vel = 3f;
        _mat = null;
    }

    public void SetPrefab(GameObject basePrefab) => _prefab = basePrefab;

    public void SetSpawn(Vector3 position, Quaternion rotation)
    {
        _spawnPos = position;
        _spawnRot = rotation;
    }

    public void SetStats(int vida, float velocidad)
    {
        _vida = vida;
        _vel = velocidad;
    }

    public void SetApariencia(Material materialOpcional = null)
    {
        _mat = materialOpcional;
    }

    public IUnidad Build()
    {
        if (_prefab == null)
        {
            Debug.LogError("UnidadBuilder: prefab no asignado.");
            return null;
        }

        // 1) Asegurar spawn sobre NavMesh
        Vector3 finalSpawn = _spawnPos;
        if (NavMesh.SamplePosition(_spawnPos, out var hit, NAVMESH_PICK_RADIUS, NavMesh.AllAreas))
            finalSpawn = hit.position;
        else
            Debug.LogWarning($"UnidadBuilder: no hay NavMesh cerca de {_spawnPos}. Se intenta instanciar igual.");

        var go = Object.Instantiate(_prefab, finalSpawn, _spawnRot);

        // 2) Warp del NavMeshAgent si quedó fuera
        if (go.TryGetComponent<NavMeshAgent>(out var agent))
        {
            if (!agent.isOnNavMesh)
            {
                if (NavMesh.SamplePosition(go.transform.position, out hit, NAVMESH_PICK_RADIUS, NavMesh.AllAreas))
                {
                    agent.enabled = false;
                    go.transform.position = hit.position;
                    agent.enabled = true;
                    if (!agent.isOnNavMesh) agent.Warp(hit.position);
                }
            }
        }

        if (!go.TryGetComponent<IUnidad>(out var unidad))
        {
            Debug.LogError($"UnidadBuilder: el prefab {_prefab.name} no implementa IUnidad.");
            Object.Destroy(go);
            return null;
        }

        // (Opcional) aplicar stats si tus componentes exponen setters
        // if (go.TryGetComponent<IDaniable>(out var daniable)) daniable.SetVidaBase(_vida);
        // if (go.TryGetComponent<IMovible>(out var mov)) mov.SetVelocidad(_vel);

        if (_mat != null)
        {
            var rend = go.GetComponentInChildren<Renderer>();
            if (rend != null) rend.material = _mat;
        }

        return unidad;
    }
}
