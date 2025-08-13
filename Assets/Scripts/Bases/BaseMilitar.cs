using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMilitar : MonoBehaviour, IBase
{
    [Header("Estado / Vida")]
    [SerializeField] private int saludMaxima = 800;
    [SerializeField] private int saludActual = 800;
    [SerializeField] private EstadoBase estado = EstadoBase.Activa;

    [Header("Entrenamiento (cola simple)")]
    [Tooltip("Número de peones que se entrenan en paralelo")]
    [SerializeField] private int capacidadSlots = 2;
    [Tooltip("Segundos que tarda convertir un peón")]
    [SerializeField] private float tiempoEntrenamiento = 10f;

    [Header("Puntos (hijos recomendados)")]
    [Tooltip("Dónde se aparcan (y opcionalmente ocultan) los peones mientras entrenan")]
    [SerializeField] private Transform holdPoint;
    [Tooltip("Dónde aparece la nueva unidad; colócalo sobre el NavMesh")]
    [SerializeField] private Transform spawnPoint;

    [Header("Conversión")]
    [Tooltip("Si está en true, destruye el Peón tras convertirlo")]
    [SerializeField] private bool destruirPeonAlConvertir = true;

    private readonly Queue<Peon> colaEspera = new();
    private int slotsOcupados = 0;

    private void Reset()
    {
        // Crear puntos por defecto si no están
        if (holdPoint == null)
        {
            var go = new GameObject("HoldPoint");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(0, 0, 0.5f);
            holdPoint = go.transform;
        }
        if (spawnPoint == null)
        {
            var go = new GameObject("SpawnPoint");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(0, 0, 2f);
            spawnPoint = go.transform;
        }
    }

    private void Awake()
    {
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        estado = EstadoBase.Activa;
    }

    // =========================
    // IBase
    // =========================
    public void Construir()
    {
        // En este flujo la base aparece “lista”.
    }

    public void RecibirDanio(int cantidad)
    {
        if (estado == EstadoBase.Destruida) return;

        saludActual -= Mathf.Max(0, cantidad);
        if (saludActual <= 0)
        {
            saludActual = 0;
            estado = EstadoBase.Destruida;
            Debug.Log("💥 Base Militar destruida.");
            // Opcional: cancelar colas, etc.
        }
    }

    /// <summary>
    /// Producción directa de unidades (no Peón). Mantiene contrato previo.
    /// </summary>
    public void ProducirUnidad(UnidadType tipo)
    {
        if (estado != EstadoBase.Activa)
        {
            Debug.LogWarning("⚠️ La Base Militar no está activa.");
            return;
        }
        if (tipo == UnidadType.Peon)
        {
            Debug.LogWarning("ℹ️ La Base Militar no produce Peones. Usa la Casona.");
            return;
        }
        if (UnidadFactory.Instance == null)
        {
            Debug.LogError("❌ UnidadFactory.Instance no disponible.");
            return;
        }

        Vector3 pos = spawnPoint ? spawnPoint.position : transform.position + Vector3.forward * 1.5f;
        var u = UnidadFactory.Instance.CrearUnidad(tipo, pos);
        if (u != null) Debug.Log($"✅ Unidad {tipo} creada desde Base Militar.");
    }

    public EstadoBase ObtenerEstado() => estado;

    // =========================
    // ENTRENAMIENTO (cola simple)
    // =========================

    /// <summary>
    /// API pública que puedes llamar desde UI (por ejemplo: “Entrenar peones seleccionados”).
    /// </summary>
    public void EnviarAPeonEntrenar(Peon peon)
    {
        if (peon == null || estado != EstadoBase.Activa) return;

        // Aparcar y congelar el peón dentro de la base (lo oculta y deshabilita su agente)
        Vector3 hold = holdPoint ? holdPoint.position : transform.position;
        peon.PrepararseParaEntrenarSimple(hold);

        colaEspera.Enqueue(peon);
        ProcesarCola();
    }

    private void ProcesarCola()
    {
        while (slotsOcupados < capacidadSlots && colaEspera.Count > 0)
        {
            var peon = colaEspera.Dequeue();
            StartCoroutine(CorEntrenar(peon));
            slotsOcupados++;
        }
    }

    private IEnumerator CorEntrenar(Peon peon)
    {
        if (peon == null)
        {
            slotsOcupados--;
            yield break;
        }

        Debug.Log($"[BaseMilitar] Entrenando {peon.name} por {tiempoEntrenamiento} s...");
        yield return new WaitForSeconds(tiempoEntrenamiento);

        // Elegir tipo aleatorio y construir usando Director + Builder + Factory
        var tipo = ElegirTipoAleatorio();
        if (UnidadFactory.Instance == null)
        {
            Debug.LogError("❌ UnidadFactory.Instance no disponible.");
        }
        else
        {
            var builder = new UnidadBuilder();
            var director = new UnidadDirector(builder, UnidadFactory.Instance);

            Vector3 spawn = spawnPoint ? spawnPoint.position : transform.position + Vector3.forward * 1.5f;
            var unidad = director.ConstruirUnidad(tipo, spawn, Quaternion.identity);

            if (unidad != null)
                Debug.Log($"[BaseMilitar] {peon.name} convertido en {tipo} (Builder).");
            else
                Debug.LogError("[BaseMilitar] Falló la construcción de la unidad con Builder.");
        }

        if (destruirPeonAlConvertir && peon != null)
        {
            Destroy(peon.gameObject);
        }
        else if (peon != null)
        {
            // Si no lo destruyes, puedes dejarlo desactivado como “consumido”
            peon.gameObject.SetActive(false);
            peon.transform.SetParent(this.transform, worldPositionStays: true);
        }

        slotsOcupados--;
        ProcesarCola();
    }

    private UnidadType ElegirTipoAleatorio()
    {
        UnidadType[] pool =
        {
            UnidadType.Campesino,
            UnidadType.Chaman,
            UnidadType.Esperancita,
            UnidadType.Morenito
        };
        return pool[Random.Range(0, pool.Length)];
    }

    // Gizmos para ubicar puntos fácilmente
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; if (holdPoint) Gizmos.DrawSphere(holdPoint.position, 0.2f);
        Gizmos.color = Color.cyan; if (spawnPoint) Gizmos.DrawSphere(spawnPoint.position, 0.2f);
    }
}