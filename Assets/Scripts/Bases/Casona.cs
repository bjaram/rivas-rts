using UnityEngine;

public class Casona : MonoBehaviour, IBase
{
    [Header("Vida")]
    [SerializeField] private int saludMaxima = 1000;
    [SerializeField] private int saludActual;

    [Header("Spawn")]
    [SerializeField] private Transform puntoSpawn;

    private EstadoBase estado = EstadoBase.Activa; // ya activa al instanciar

    private void Awake()
    {
        saludActual = saludMaxima;
    }

    // IBase
    public void Construir()
    {
        // No-op en esta versión: la casona ya nace construida
    }

    public void RecibirDanio(int cantidad)
    {
        saludActual -= cantidad;
        if (saludActual <= 0)
        {
            saludActual = 0;
            estado = EstadoBase.Destruida;
            Debug.Log("💥 Casona destruida.");
        }
    }

    public void ProducirUnidad(UnidadType tipo)
    {
        if (estado != EstadoBase.Activa)
        {
            Debug.LogWarning("⚠️ Casona no activa, no puede producir.");
            return;
        }

        if (tipo != UnidadType.Peon)
        {
            Debug.LogWarning("ℹ️ En esta versión la casona solo produce Peones.");
            return;
        }

        if (UnidadFactory.Instance == null)
        {
            Debug.LogError("❌ UnidadFactory no disponible.");
            return;
        }

        Vector3 spawnPos = puntoSpawn != null ? puntoSpawn.position : transform.position + Vector3.right * 1.5f;
        var unidad = UnidadFactory.Instance.CrearUnidad(UnidadType.Peon, spawnPos);
        if (unidad != null) Debug.Log("✅ Peón creado desde Casona.");
    }

    public EstadoBase ObtenerEstado() => estado;

    // Utilidad para UI
    public Transform GetSpawnPoint() => puntoSpawn;
}