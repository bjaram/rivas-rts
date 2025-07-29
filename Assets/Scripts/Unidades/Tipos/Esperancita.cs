using UnityEngine;

public class Esperancita : MonoBehaviour, IUnidad, IMovible, IDaniable, IAtacante
{
    private int vida = 50;
    private float velocidad = 100f;
    private int vidaMaxima;

    private IComportamientoAtaque comportamientoAtaque;

    private void Start()
    {
        comportamientoAtaque = new AtaqueChancleta(); 
    }

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

    public void Atacar(GameObject objetivo)
    {
        comportamientoAtaque.Atacar(objetivo);
    }

    public void EstablecerComportamiento(IComportamientoAtaque nuevoComportamiento)
    {
        comportamientoAtaque = nuevoComportamiento;
    }
    public void RecibirCuracion(int cantidad)
    {
        vida += cantidad;
        if (vida > vidaMaxima)
            vida = vidaMaxima;
    }
}