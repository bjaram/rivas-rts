using UnityEngine;

public class EstadoEnConstruccion : IEstadoBase
{
    public void Ejecutar(Casona casona)
    {
        Debug.Log("🔨 La casona está en construcción...");
    }

    public void ProducirUnidad(Casona casona, UnidadType tipo)
    {
        Debug.Log("❌ No se pueden producir unidades mientras se construye la casona.");
    }

    public void RecibirDanio(Casona casona, int cantidad)
    {
        casona.ModificarSalud(-cantidad);
    }
}