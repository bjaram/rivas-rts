public interface IEstadoBase
{
    void Ejecutar(Casona casona);
    void ProducirUnidad(Casona casona, UnidadType tipo);
    void RecibirDanio(Casona casona, int cantidad);
}