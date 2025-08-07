using UnityEngine;
using UnityEngine.AI;

public class Peon : MonoBehaviour, IUnidad, IMovible, IDaniable, IRecolector
{
    private int vida = 60;
    private float velocidad = 15f;
    private int vidaMaxima = 60;
    private int cargaActual = 0;
    private int cargaMaxima = 50;
    private bool llevandoRecursos = false;
    private bool casonaConstruida = false;

    private NavMeshAgent agente;
    private GameObject objetivoRecoleccion;
    private SitioConstruccionBase objetivoConstruccion;

    private void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.speed = velocidad;

        BuscarNuevaPlanta();
    }

    private void Update()
    {
        if (!agente.pathPending && agente.remainingDistance < 0.5f)
        {
            if (!llevandoRecursos)
            {
                Recolectar();
            }
            else
            {
                EntregarRecursos();
            }
        }
    }

    private void BuscarNuevaPlanta()
    {
        GameObject[] plantas = GameObject.FindGameObjectsWithTag("PlantaPalmera");
        if (plantas.Length == 0)
        {
            Debug.LogWarning("❌ No hay plantas disponibles para recolectar.");
            return;
        }

        objetivoRecoleccion = plantas[Random.Range(0, plantas.Length)];
        agente.SetDestination(objetivoRecoleccion.transform.position);
    }

    private void BuscarSitioConstruccion()
    {
        SitioConstruccionBase[] sitios = FindObjectsByType<SitioConstruccionBase>(FindObjectsSortMode.None);
        foreach (var sitio in sitios)
        {
            if (!sitio.EstaCompleto())
            {
                objetivoConstruccion = sitio;
                agente.SetDestination(sitio.transform.position);
                return;
            }
        }

        Debug.LogWarning("⚠️ No hay sitios de construcción disponibles.");
    }

    private void Recolectar()
    {
        cargaActual = cargaMaxima;
        llevandoRecursos = true;
        BuscarSitioConstruccion();
    }

    private void EntregarRecursos()
    {
        if (objetivoConstruccion != null)
        {
            objetivoConstruccion.RecibirRecursos(this, cargaActual);
            cargaActual = 0;
            llevandoRecursos = false;

            if (!objetivoConstruccion.EstaCompleto())
            {
                BuscarNuevaPlanta();
            }
            else
            {
                if (!casonaConstruida)
                {
                    casonaConstruida = true;
                    BuscarSitioConstruccion(); // Ahora irá a la BaseMilitar
                }
                else
                {
                    BuscarNuevaPlanta(); // Ya construyó Casona y BaseMilitar
                }
            }
        }
    }

    public void Mover(Vector3 destino)
    {
        agente.SetDestination(destino);
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int ObtenerVida()
    {
        return vida;
    }

    public void RecibirCuracion(int cantidad)
    {
        vida += cantidad;
        if (vida > vidaMaxima)
            vida = vidaMaxima;
    }

    public void RecolectarRecursos()
    {
        // Ya está es manejado en Update y Recolectar()
    }
}