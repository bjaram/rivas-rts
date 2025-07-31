using System;
using UnityEngine;

public class UnidadFactory : MonoBehaviour
{
    public static UnidadFactory Instance { get; private set; }

    public GameObject peonPrefab;
    public GameObject campesinoPrefab;
    public GameObject esperancitaPrefab;
    public GameObject morenitoPrefab;
    public GameObject chamanPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public IUnidad CrearUnidad(UnidadType tipo, Vector3 posicion)
    {
        GameObject prefab = tipo switch
        {
            UnidadType.Peon => peonPrefab,
            UnidadType.Campesino => campesinoPrefab,
            UnidadType.Esperancita => esperancitaPrefab,
            UnidadType.Morenito => morenitoPrefab,
            UnidadType.Chaman => chamanPrefab,
            _ => null
        };

        if (prefab == null)
        {
            Debug.LogError($"❌ Prefab no asignado para la unidad: {tipo}");
            return null;
        }

        GameObject unidadGO = Instantiate(prefab, posicion, Quaternion.identity);

        if (!unidadGO.TryGetComponent(out IUnidad unidad))
        {
            Debug.LogError($"❌ El prefab {prefab.name} no implementa IUnidadBase.");
            Destroy(unidadGO);
            return null;
        }

        return unidad;
    }
}