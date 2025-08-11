using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMilitar : MonoBehaviour, IBase
{
    [Header("Estado / Vida")]
    [SerializeField] private int saludMaxima = 800;
    [SerializeField] private int saludActual = 800;
    [SerializeField] private EstadoBase estado = EstadoBase.Activa;

    [Header("Entrenamiento (cola)")]
    [SerializeField] private int capacidadSlots = 2;          // peones simultáneamente
    [SerializeField] private float tiempoEntrenamiento = 10f; // segundos por peón
    [Tooltip("Si está activo, cualquier Peón con DeseaEntrenar que entre al trigger se encola automáticamente.")]
    [SerializeField] private bool aceptarAutomatico = true;

    [Header("Puntos (recomendado crear como hijos)")]
    [SerializeField] private Transform puntoEntrada;  // añade un Collider isTrigger aquí o en el objeto raíz
    [SerializeField] private Transform puntoInterno;  // donde “guardas” al peón mientras entrena
    [SerializeField] private Transform puntoSalida;   // donde aparece la nueva unidad

    private readonly Queue<Peon> colaEspera = new();
    private int slotsOcupados = 0;

    private void Reset()
    {
        // Crear puntos por defecto si no están
        if (puntoEntrada == null)
        {
            var go = new GameObject("PuntoEntrada");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(0, 0, -2f);
            puntoEntrada = go.transform;
        }
        if (puntoInterno == null)
        {
            var go = new GameObject("PuntoInterno");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(0, 0, 0.5f);
            puntoInterno = go.transform;
        }
        if (puntoSalida == null)
        {
            var go = new GameObject("PuntoSalida");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(0, 0, 2f);
            puntoSalida = go.transform;
        }
    }

    private void Awake()
    {
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        estado = EstadoBase.Activa;
    }

    // ======================================================
    // IBase
    // ======================================================
    public void Construir()
    {
        // En este flujo la Base Militar aparece construida (ya sin sombra).
        // Dejar no-op para compatibilidad con la interfaz.
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
            // Opcional: cancelar colas, destruir pendientes, etc.
        }
    }

    /// <summary>
    /// Producción directa de unidades NO Peón (opcional). 
    /// Mantiene tu contrato previo: la base no produce Peones, sólo especializadas.
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
            Debug.LogWarning("ℹ️ La Base Militar no produce Peones. Para Peones usa la Casona.");
            return;
        }

        if (UnidadFactory.Instance == null)
        {
            Debug.LogError("❌ UnidadFactory.Instance no disponible.");
            return;
        }

        Vector3 pos = puntoSalida != null ? puntoSalida.position : transform.position + Vector3.forward * 1.5f;
        var u = UnidadFactory.Instance.CrearUnidad(tipo, pos);
        if (u != null) Debug.Log($"✅ Unidad {tipo} creada desde Base Militar.");
    }

    public EstadoBase ObtenerEstado() => estado;

    // Getter para el punto de entrada (lo usa Peon.EnviarAEntrenamiento)
    public Transform GetPuntoEntrada() => puntoEntrada;

    // ======================================================
    // ENTRENAMIENTO (cola de Peones -> unidades aleatorias)
    // ======================================================

    private void OnTriggerEnter(Collider other)
    {
        if (!aceptarAutomatico || estado != EstadoBase.Activa) return;

        var peon = other.GetComponent<Peon>();
        if (peon == null) return;

        // Aceptar sólo peones que vienen a entrenar
        if (peon.DeseaEntrenar)
        {
            TryEncolar(peon);
            Debug.Log($"[BaseMilitar] {peon.name} entró por trigger y fue encolado. Cola={colaEspera.Count}");
        }
    }

    /// <summary>
    /// API para UI: ordena a la base recibir/gestionar el entrenamiento de un peón concreto.
    /// Si aceptarAutomatico es true, el peón caminará al PuntoEntrada y se encolará al cruzar el trigger.
    /// Si aceptarAutomatico es false, la base lo encolará cuando detecte que llegó cerca del PuntoEntrada.
    /// </summary>
    public void EnviarAPeonEntrenar(Peon peon)
    {
        Debug.Log($"[BaseMilitar] Peón {peon.name} enviado a entrenar.");

        if (peon == null || estado != EstadoBase.Activa) return;

        // Ordena al peón ir al punto de entrada y cortar su bucle de recolección
        peon.EnviarAEntrenamiento(this);

        if (!aceptarAutomatico)
        {
            // Si no usamos el trigger auto, encolar manualmente al llegar
            StartCoroutine(EsperarYEncolarSiNoAuto(peon));
        }
    }

    private IEnumerator EsperarYEncolarSiNoAuto(Peon peon)
    {
        // Esperar a que el peón esté lo suficientemente cerca del punto de entrada
        float timeout = 15f; // seguridad
        float t = 0f;

        while (peon != null && puntoEntrada != null)
        {
            float dist = Vector3.Distance(peon.transform.position, puntoEntrada.position);
            if (dist <= 0.6f) break;

            t += Time.deltaTime;
            if (t >= timeout) break;
            yield return null;
        }

        if (peon != null) TryEncolar(peon);
    }

    /// <summary>Encola un Peón para entrenamiento (puedes llamarlo desde UI también).</summary>
    public void TryEncolar(Peon peon)
    {
        if (peon == null || estado != EstadoBase.Activa) return;
        if (colaEspera.Contains(peon)) return;

        // Que deje de recolectar y pase a modo “entrenar”
        peon.PrepararseParaEntrenar(this, puntoEntrada != null ? puntoEntrada.position : transform.position);

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

        // Aparcarlo dentro (apagar agente/renderer mientras “entrena”)
        peon.TeletransportarYCongelar(puntoInterno != null ? puntoInterno.position : transform.position);

        Debug.Log($"[BaseMilitar] Entrenando {peon.name} por {tiempoEntrenamiento} s...");
        yield return new WaitForSeconds(tiempoEntrenamiento);

        // Convertir al azar a una unidad especializada
        var tipo = ElegirTipoAleatorio();
        if (UnidadFactory.Instance == null)
        {
            Debug.LogError("❌ UnidadFactory.Instance no disponible.");
        }
        else
        {
            Vector3 spawn = puntoSalida != null ? puntoSalida.position : transform.position + Vector3.forward * 1.5f;
            UnidadFactory.Instance.CrearUnidad(tipo, spawn);
            Debug.Log($"[BaseMilitar] {peon.name} convertido en {tipo}.");
        }

        // Eliminar el peón original
        if (peon != null) Destroy(peon.gameObject);

        slotsOcupados--;
        ProcesarCola(); // seguir con la cola
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

    // Gizmos para ayudar a ubicar los puntos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; if (puntoEntrada) Gizmos.DrawSphere(puntoEntrada.position, 0.2f);
        Gizmos.color = Color.yellow; if (puntoInterno) Gizmos.DrawSphere(puntoInterno.position, 0.2f);
        Gizmos.color = Color.cyan; if (puntoSalida) Gizmos.DrawSphere(puntoSalida.position, 0.2f);
    }
}