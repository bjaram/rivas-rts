using UnityEngine;

public interface IRecolector
{
    void BuscarRecurso();
    void Recolectar();

    //BuscarRecurso(): determina qué recurso va a buscar según la tarea.

    //Recolectar(): lógica para extraer unidades del recurso y guardarlas temporalmente.
}
