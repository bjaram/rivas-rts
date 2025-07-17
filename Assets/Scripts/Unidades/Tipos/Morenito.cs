using UnityEngine;

public class Morenito : MonoBehaviour, IUnidad
{
    private int vida = 200;
    private float velocidad = 150f;

    public void Mover(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    public void Atacar(GameObject objetivo)
    {
        // Lógica de tanque (golpe fuerte)
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Destroy(gameObject);
    }

    public int ObtenerVida() => vida;
}