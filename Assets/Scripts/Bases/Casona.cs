using System.Resources;
using UnityEngine;
using System.Collections;

public class Casona : MonoBehaviour, IBase
{
    [SerializeField] private int saludMaxima = 1000;
    [SerializeField] private int saludActual;
    [SerializeField] private int costePalmeras = 200;
    [SerializeField] private float tiempoConstruccion = 5f;

    private EstadoBase estado = EstadoBase.EnConstruccion;

    private void Start()
    {
        saludActual = saludMaxima;
        Construir();
    }

    public void Construir()
    {
        if (!ResourceManager.Instance.Gastar(RecursoType.Palmeras, costePalmeras))
        {
            Debug.LogWarning("No hay suficientes palmeras para construir la Casona.");
            return;
        }

        // Simular construcción con espera
        StartCoroutine(ConstruccionProgresiva());
    }

    private IEnumerator ConstruccionProgresiva()
    {
        float tiempo = 0f;
        while (tiempo < tiempoConstruccion)
        {
            tiempo += Time.deltaTime;
            // Aquí puedes actualizar una barra de progreso
            yield return null;
        }

        estado = EstadoBase.Activa;
        Debug.Log("Casona construida y activa.");
    }

    public void RecibirDanio(int cantidad)
    {
        saludActual -= cantidad;
        if (saludActual <= 0)
        {
            saludActual = 0;
            estado = EstadoBase.Destruida;
            // Trigger visual y sonido
            Debug.Log("¡La Casona ha sido destruida!");
        }
    }
    //Crear las unidades básicas de tipo Peon
    public void ProducirUnidad(UnidadType tipo)
    {
        if (tipo == UnidadType.Peon)
        {
            UnidadFactory.Instance.ProducirUnidad(tipo, transform.position);
        }
        else
        {
            Debug.LogWarning("La Casona solo puede crear unidades tipo Peon.");
        }
    }

    public EstadoBase ObtenerEstado()
    {
        return estado;
    }
}
