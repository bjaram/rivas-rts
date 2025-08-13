using UnityEngine;
using UnityEngine.AI;

public class UnidadFactory : MonoBehaviour
{
    public static UnidadFactory Instance { get; private set; }

    public GameObject peonPrefab;
    public GameObject campesinoPrefab;
    public GameObject esperancitaPrefab;
    public GameObject morenitoPrefab;
    public GameObject chamanPrefab;

    [SerializeField] private float navmeshMaxSampleDistance = 2.0f;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ✅ NUEVO: para Builder/Director
    public GameObject GetPrefab(UnidadType tipo)
    {
        return tipo switch
        {
            UnidadType.Peon => peonPrefab,
            UnidadType.Campesino => campesinoPrefab,
            UnidadType.Esperancita => esperancitaPrefab,
            UnidadType.Morenito => morenitoPrefab,
            UnidadType.Chaman => chamanPrefab,
            _ => null
        };
    }

    // Mantén este para otros casos (UI crear peón, etc.)
    public IUnidad CrearUnidad(UnidadType tipo, Vector3 posicionDeseada)
    {
        var prefab = GetPrefab(tipo);
        if (prefab == null)
        {
            Debug.LogError($"❌ Prefab no asignado para la unidad: {tipo}");
            return null;
        }

        // Asegurar NavMesh cercano
        Vector3 spawn = posicionDeseada;
        if (NavMesh.SamplePosition(posicionDeseada, out NavMeshHit hit, navmeshMaxSampleDistance, NavMesh.AllAreas))
            spawn = hit.position;

        GameObject go = Instantiate(prefab, spawn, Quaternion.identity);
        if (!go.TryGetComponent(out IUnidad unidad))
        {
            Debug.LogError($"❌ El prefab {prefab.name} no implementa IUnidad.");
            Destroy(go);
            return null;
        }

        return unidad;
    }
}