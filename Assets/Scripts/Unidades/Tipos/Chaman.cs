using UnityEngine;

public class Chaman : MonoBehaviour, IUnidad
{
    private int vida = 75;
    private float velocidad = 50f;

    public void Mover(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    public void Atacar(GameObject objetivo)
    {
        // Lógica de soporte (curar aliados)
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Destroy(gameObject);
    }

    public int ObtenerVida() => vida;
}