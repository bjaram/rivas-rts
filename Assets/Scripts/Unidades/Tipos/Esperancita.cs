using UnityEngine;

public class Esperancita : MonoBehaviour, IUnidad
{
    private int vida = 50;
    private float velocidad = 100f;

    public void Mover(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    public void Atacar(GameObject objetivo)
    {
        // Lógica de ataque a distancia (lanzar chancleta)
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Destroy(gameObject);
    }

    public int ObtenerVida() => vida;
}