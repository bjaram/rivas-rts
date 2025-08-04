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
        // Se podría activar un panel UI o animación especial
    }
}
