using UnityEngine;

public class EstadoActiva : IEstadoBase
{
    public void Ejecutar(Casona casona)
    {
        Debug.Log("La casona está activa.");
    }

    public void RecibirDanio(Casona casona, int cantidad)
    {
        casona.ModificarSalud(-cantidad);
    }

    public void ProducirUnidad(Casona casona, UnidadType tipo)
    {
        // ✅ Solo permitir la creación de Peon
        if (tipo != UnidadType.Peon)
        {
            Debug.LogWarning("⚠️ La casona solo puede producir unidades de tipo Peon.");
            return;
        }

        if (UnidadFactory.Instance == null)
        {
            Debug.LogError("❌ UnidadFactory.Instance no está disponible.");
            return;
        }

        Transform puntoSpawn = casona.GetSpawnPoint();
        if (puntoSpawn == null)
        {
            Debug.LogError("❌ Punto de spawn no asignado en la casona.");
            return;
        }

        IUnidad nuevaUnidad = UnidadFactory.Instance.CrearUnidad(tipo, puntoSpawn.position);

        if (nuevaUnidad != null)
        {
            Debug.Log($"✅ Unidad producida correctamente: {tipo}");
        }
    }
}