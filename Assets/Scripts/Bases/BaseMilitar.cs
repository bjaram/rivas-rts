using UnityEngine;

public class BaseMilitar : MonoBehaviour, IBase
{
    public void Construir() { }
    public void RecibirDanio(int cantidad) { }

    public void ProducirUnidad(UnidadType tipo)
    {
        if (tipo != UnidadType.Peon)
        {
            UnidadFactory.Instance.CrearUnidad(tipo, transform.position);
        }
        else
        {
            Debug.LogWarning("La Base Militar no produce Peones. Usa la Casona.");
        }
    }

    public EstadoBase ObtenerEstado() => EstadoBase.Activa;
}