using UnityEngine;

public class SitioConstruccionMilitar : SitioConstruccionBase
{
    protected override void Start()
    {
        tipoRecurso = RecursoType.Cafe;
        base.Start();
    }

    protected override void ConstruirEdificioFinal()
    {
        base.ConstruirEdificioFinal();
        // Aqu� se podr�a activar entrenamiento de peones si se quisiera
    }
}