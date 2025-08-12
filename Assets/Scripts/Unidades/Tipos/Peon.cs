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
    /// <summary>Indica si este peón ha sido ordenado a entrenar en la Base.</summary>
    public bool DeseaEntrenar { get; private set; } = false;

    /// <summary>Referencia a la base a la que fue asignado para entrenar (opcional).</summary>
    private BaseMilitar baseObjetivo;

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
    // Flujo de ENTRENAMIENTO
    // =========================

    /// <summary>
    /// Orden externa: este peón debe desplazarse a una Base Militar para entrenar.
    /// Deja de recolectar y navega hacia la base (al objeto raíz de la base o su punto de entrada según tu setup).
    /// </summary>
    public void EnviarAEntrenamiento(BaseMilitar baseMilitar)
    {

        if (baseMilitar == null) return;

        baseObjetivo = baseMilitar;
        DeseaEntrenar = true;

        // Rompe cualquier ciclo de recolección en curso
        objetivoPlanta = null;
        CancelInvoke(nameof(BuscarRecurso));

        if (agente != null)
        {
            agente.isStopped = false;
            agente.ResetPath();
            agente.stoppingDistance = 0.2f;

            // Ir al punto de entrada si existe; si no, al root de la base
            Vector3 destino = baseMilitar.GetPuntoEntrada() != null
                ? baseMilitar.GetPuntoEntrada().position
                : baseMilitar.transform.position;

            agente.SetDestination(destino);
        }
    }

    /// <summary>
    /// Llamado por BaseMilitar cuando acepta y encola a este Peón.
    /// Detiene navegación para no interferir y lo entrega al control de la base.
    /// </summary>
    public void PrepararseParaEntrenar(BaseMilitar baseRef, Vector3 posEntrada)
    {
        baseObjetivo = baseRef;
        DeseaEntrenar = true;

        if (agente != null)
        {
            agente.isStopped = true;
            agente.ResetPath();
        }
    }

    /// <summary>
    /// Llamado por BaseMilitar justo antes de iniciar el tiempo de entrenamiento.
    /// Mueve y congela al peón; opcionalmente oculta su Renderer para “desaparecerlo” durante el training.
    /// </summary>
    public void TeletransportarYCongelar(Vector3 pos)
    {
        if (agente != null)
        {
            // Warp evita problemas de pathing
            agente.Warp(pos);
            // Deshabilitar agente durante el entrenamiento
            agente.enabled = false;
        }

        if (cachedRenderer != null) cachedRenderer.enabled = false;
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