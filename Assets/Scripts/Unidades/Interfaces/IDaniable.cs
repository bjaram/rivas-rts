using UnityEngine;

public interface IDaniable
{
    void RecibirDanio(int cantidad);
    int ObtenerVida();
    void RecibirCuracion(int cantidad);
}
