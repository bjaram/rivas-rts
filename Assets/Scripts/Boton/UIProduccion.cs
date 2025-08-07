using UnityEngine;
using UnityEngine.UI;

public class UIProduccion : MonoBehaviour
{
    [Header("Referencia a la casona activa")]
    public Casona casonaActiva;

    [Header("Botón para crear peones")]
    public Button botonCrearPeon;

    public void CrearPeon()
    {
        if (casonaActiva != null)
        {
            casonaActiva.ProducirUnidad(UnidadType.Peon);
        }
        else
        {
            Debug.LogWarning("⚠️ No hay casona activa asignada.");
        }
    }

    private void Update()
    {
        if (botonCrearPeon != null && casonaActiva != null)
        {
            // Habilita el botón solo si la casona está en estado Activa
            botonCrearPeon.interactable = casonaActiva.ObtenerEstado() == EstadoBase.Activa;
        }
    }
}