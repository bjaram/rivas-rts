using UnityEngine;
using UnityEngine.AI;

public class Peon : MonoBehaviour, IUnidad, IMovible, IDaniable, IRecolector
{
    [Header("Stats")]
    public int vidaMaxima = 60;
    public float velocidad = 3.5f;
    public int capacidadCarga = 50;
    public float distanciaLlegada = 0.6f;

    private int vida;
    private int cargaActual;
    private NavMeshAgent agente;
    private GameObject objetivoPlanta;

    // ===== Entrenamiento =====
    /// <summary>Indica si este peón ha sido ordenado a entrenar.</summary>
    public bool DeseaEntrenar { get; private set; } = false;

    private Renderer cachedRenderer;

    private void Awake()
    {
        vida = vidaMaxima;
        agente = GetComponent<NavMeshAgent>();
        if (agente != null) agente.speed = velocidad;

        cachedRenderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        BuscarRecurso(); // comienza a recolectar palmeras
    }

    private void Update()
    {
        // Si fue ordenado a entrenar, este peón ya no sigue el bucle de recolección aquí.
        if (DeseaEntrenar || agente == null) return;

        if (!agente.pathPending && agente.remainingDistance <= distanciaLlegada)
        {
            // Llegó a la planta -> recolecta y busca otra
            Recolectar();
            BuscarRecurso();
        }
    }

    // =========================
    // IRecolector
    // =========================
    public void BuscarRecurso()
    {
        if (DeseaEntrenar) return; // si va a entrenar, no buscar más recursos

        const string tag = "PlantaPalmera";
        GameObject[] plantas = GameObject.FindGameObjectsWithTag(tag);

        if (plantas == null || plantas.Length == 0)
        {
            objetivoPlanta = null;
            // Reintentar en 1s si no hay plantas
            Invoke(nameof(BuscarRecurso), 1f);
            return;
        }

        objetivoPlanta = plantas[Random.Range(0, plantas.Length)];
        if (agente != null) agente.SetDestination(objetivoPlanta.transform.position);
    }

    public void Recolectar()
    {
        if (DeseaEntrenar) return;

        cargaActual = capacidadCarga;
        ResourceManager.Instance.Incrementar(RecursoType.Palmeras, cargaActual);
        cargaActual = 0;
    }

    // =========================
    // Flujo de ENTRENAMIENTO (simple en Casona)
    // =========================

    /// <summary>
    /// Llamado por SimpleTrainer.Encolar(peon).
    /// Detiene recolección, apaga el agente y lo “aparca” en el hold point dentro de la Casona.
    /// </summary>
    public void PrepararseParaEntrenarSimple(Vector3 puntoHold)
    {
        DeseaEntrenar = true;

        // Corta el ciclo de búsqueda
        objetivoPlanta = null;
        CancelInvoke(nameof(BuscarRecurso));

        // Congela navegación
        if (agente != null)
        {
            if (agente.enabled)
            {
                agente.isStopped = true;
                agente.ResetPath();
            }
            agente.enabled = false;
        }

        // Mueve al “cuarto” de entrenamiento y, opcional, oculta visualmente
        transform.position = puntoHold;

        if (cachedRenderer != null)
        {
            var rends = GetComponentsInChildren<Renderer>();
            foreach (var r in rends) r.enabled = false;
        }
    }

    /// <summary>
    /// Si no deseas destruir al peón tras el entrenamiento, puedes devolverlo con esto.
    /// (En este flujo lo destruimos, así que es opcional.)
    /// </summary>
    public void LiberarDespuesEntrenamiento()
    {
        if (agente != null)
        {
            agente.enabled = true;
            agente.isStopped = false;
        }

        var rends = GetComponentsInChildren<Renderer>();
        foreach (var r in rends) r.enabled = true;

        DeseaEntrenar = false;
        BuscarRecurso();
    }

    // =========================
    // IUnidad / IMovible / IDaniable
    // =========================
    public void Mover(Vector3 destino)
    {
        if (agente != null && agente.enabled)
        {
            agente.isStopped = false;
            agente.SetDestination(destino);
        }
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Destroy(gameObject);
    }

    public int ObtenerVida() => vida;

    public void RecibirCuracion(int cantidad)
    {
        vida = Mathf.Min(vida + cantidad, vidaMaxima);
    }
}