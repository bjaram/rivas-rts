using UnityEngine;

public class Peon : MonoBehaviour, IUnidad, IMovible,  IDaniable, IRecolector
{
    private int vida = 60;
    private float velocidad = 80f;
    private bool estaEnZonaRecoleccion = false;
    private RecursoType tipoRecursoActual;
    private int vidaMaxima;

    public void Mover(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
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
        if (!estaEnZonaRecoleccion)
        {
            Debug.Log("⚠️ El Peón no está en una zona válida de recolección.");
            return;
        }

        if (tipoRecursoActual == RecursoType.Palmeras)
        {
            ResourceManager.Instance.Incrementar(RecursoType.Palmeras, 10);
            Debug.Log("🌴 Peón recolectó palmeras.");
        }
        else if (tipoRecursoActual == RecursoType.Cafe)
        {
            ResourceManager.Instance.Incrementar(RecursoType.Cafe, 5);
            Debug.Log("☕ Peón recolectó café.");
        }
        else
        {
            Debug.LogWarning("⚠️ Tipo de recurso desconocido.");
        }
    }
}
