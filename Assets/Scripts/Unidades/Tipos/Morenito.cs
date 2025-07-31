using UnityEngine;

public class Morenito : MonoBehaviour, IDaniable, IMovible, IAtacante
{
    private int vida = 120;
    private float velocidad = 2f;

    private IComportamientoAtaque comportamientoAtaque;
    private int vidaMaxima;

    private void Start()
    {
        comportamientoAtaque = new AtaquePunno(); 
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
