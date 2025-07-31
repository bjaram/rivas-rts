using UnityEngine;

public class EstadoActiva : IEstadoBase
{
    public void Ejecutar(Casona casona)
    {
        Debug.Log("✅ La casona está activa.");
    }

    public void ProducirUnidad(Casona casona, UnidadType tipo)
    {
        if (tipo == UnidadType.Peon)
            UnidadFactory.Instance.CrearUnidad(tipo, casona.transform.position);
        else
            Debug.LogWarning("⚠️ Solo se puede crear Peon desde la Casona.");
    }

    public void RecibirDanio(Casona casona, int cantidad)
    {
        casona.ModificarSalud(-cantidad);
    }
}