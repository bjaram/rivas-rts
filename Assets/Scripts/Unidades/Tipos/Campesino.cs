using UnityEngine;

public class Campesino : MonoBehaviour, IUnidad
{
    private int vida = 100;
    private float velocidad = 75f;

    public void Mover(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    public void Atacar(GameObject objetivo)
    {
        // Lógica de ataque cuerpo a cuerpo
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Destroy(gameObject);
    }

    public int ObtenerVida() => vida;
}