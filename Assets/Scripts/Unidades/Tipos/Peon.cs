using UnityEngine;

public class Peon : MonoBehaviour, IUnidad
{
    private int vida = 60;
    private float velocidad = 80f;

    public void Mover(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    public void Atacar(GameObject objetivo)
    {
        // Los peones no atacan. Esta funci�n puede quedar vac�a o lanzar advertencia.
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
}
