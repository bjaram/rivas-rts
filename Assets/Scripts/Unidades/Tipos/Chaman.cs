using UnityEngine;

public class Chaman : MonoBehaviour, IUnidad, IMovible, IDaniable, ISanador
{
    private int vida = 75;
    private float velocidad = 50f;
    private int cantidadCuracion = 40;
    private float rangoCuracion = 5f;
    private int vidaMaxima;

    public void Mover(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Destroy(gameObject);
    }

    public int ObtenerVida() => vida;    

    public void Curar(IDaniable objetivo)
    {
        if (objetivo == null)
        {
            Debug.LogWarning("⚠️ Objetivo nulo. No se puede curar.");
            return;
        }

        // Verifica si el objetivo está dentro del rango
        if (objetivo is MonoBehaviour mono)
        {
            float distancia = Vector3.Distance(transform.position, mono.transform.position);
            if (distancia > rangoCuracion)
            {
                Debug.Log("⚠️ Objetivo fuera de rango de curación.");
                return;
            }
        }

        // Ejecutar curación
        objetivo.RecibirCuracion(cantidadCuracion);
        Debug.Log($"💚 Chamán curó {cantidadCuracion} puntos a {objetivo}");
    }

    public void RecibirCuracion(int cantidad)
    {
        vida += cantidad;
        if (vida > vidaMaxima)
            vida = vidaMaxima;
    }
}