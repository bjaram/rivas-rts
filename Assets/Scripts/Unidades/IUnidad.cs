using UnityEngine;

public interface IUnidad
{
    void Mover(Vector3 destino);
    void Atacar(GameObject objetivo);
    void RecibirDanio(int cantidad);
    int ObtenerVida();
}