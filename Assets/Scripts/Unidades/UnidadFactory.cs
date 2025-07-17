using UnityEngine;

public class UnidadFactory : MonoBehaviour
{
    public static UnidadFactory Instance { get; private set; }

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

    public void EntrenarUnidad(UnidadType tipo, Vector3 posicion)
    {
        GameObject prefab = tipo switch
        {
            UnidadType.Campesino => campesinoPrefab,
            UnidadType.Esperancita => esperancitaPrefab,
            UnidadType.Morenito => morenitoPrefab,
            UnidadType.Chaman => chamanPrefab,
            _ => null
        };

        if (prefab == null)
        {
            Debug.LogError("Prefab no asignado para el tipo de unidad: " + tipo);
            return;
        }

        Instantiate(prefab, posicion, Quaternion.identity);
    }
}