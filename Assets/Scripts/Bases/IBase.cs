using UnityEngine;

public interface IBase
{
    void Construir();
    void RecibirDanio(int cantidad);
    void ProducirUnidad(UnidadType tipo);
    EstadoBase ObtenerEstado();
}
