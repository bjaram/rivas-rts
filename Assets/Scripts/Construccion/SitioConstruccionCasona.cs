using UnityEngine;

public class SitioConstruccionCasona : SitioConstruccionBase
{
    protected override void Start()
    {
        tipoRecurso = RecursoType.Palmeras;
        base.Start();
    }

    protected override void ConstruirEdificioFinal()
    {
        base.ConstruirEdificioFinal();
        // Se podr�a activar un panel UI o animaci�n especial
    }
}
