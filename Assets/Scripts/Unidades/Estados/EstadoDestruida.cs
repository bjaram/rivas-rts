using UnityEngine;

public class EstadoDestruida : IEstadoBase
{
    public void Ejecutar(Casona casona)
    {
        Debug.Log("☠️ La casona está destruida.");
    }

    public void ProducirUnidad(Casona casona, UnidadType tipo)
    {
        Debug.Log("❌ No se pueden producir unidades. La casona está destruida.");
    }

    public void RecibirDanio(Casona casona, int cantidad)
    {
        Debug.Log("❌ La casona ya está destruida. Ignorando daño.");
    }
}