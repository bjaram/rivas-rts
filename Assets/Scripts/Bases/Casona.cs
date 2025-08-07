using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casona : MonoBehaviour, IBase
{
    [SerializeField] private int saludMaxima = 1000;
    [SerializeField] private int saludActual;
    [SerializeField] private int costePalmeras = 200;
    [SerializeField] private float tiempoConstruccion = 5f;
    [SerializeField] private Transform puntoSpawn;

    private EstadoBase estado = EstadoBase.EnConstruccion;
    private IEstadoBase estadoActual;

    // 🔗 Cubos de progreso asociados
    private List<GameObject> cubosAsociados = new();

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

        CambiarEstado(new EstadoEnConstruccion());
        StartCoroutine(ConstruccionProgresiva());
    }

    private IEnumerator ConstruccionProgresiva()
    {
        float tiempo = 0f;
        while (tiempo < tiempoConstruccion)
        {
            tiempo += Time.deltaTime;
            yield return null;
        }

        CambiarEstado(new EstadoActiva());
        estado = EstadoBase.Activa;
        Debug.Log("Casona construida y activa.");
    }

    public void RecibirDanio(int cantidad)
    {
        estadoActual?.RecibirDanio(this, cantidad);
    }

    public void ModificarSalud(int delta)
    {
        saludActual += delta;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);

        if (saludActual <= 0 && !(estadoActual is EstadoDestruida))
        {
            CambiarEstado(new EstadoDestruida());
            estado = EstadoBase.Destruida;
            Debug.Log("¡La Casona ha sido destruida!");
        }
    }

    public void ProducirUnidad(UnidadType tipo)
    {
        if (estado != EstadoBase.Activa)
        {
            Debug.LogWarning("⚠️ La Casona aún no está construida. No se pueden crear unidades.");
            return;
        }

        estadoActual?.ProducirUnidad(this, tipo);
    }

    public EstadoBase ObtenerEstado()
    {
        return estado;
    }

    public void CambiarEstado(IEstadoBase nuevoEstado)
    {
        estadoActual = nuevoEstado;
        estadoActual.Ejecutar(this);
    }

    public Transform GetSpawnPoint()
    {
        return puntoSpawn;
    }

    // 🔗 Método nuevo: recibe los cubos del sitio de construcción
    public void AsignarCubosProgreso(List<GameObject> cubos)
    {
        cubosAsociados = cubos;

        foreach (var cubo in cubosAsociados)
        {
            cubo.transform.SetParent(this.transform); // opcional
        }

        Debug.Log("🎯 Cubos asociados a la Casona final.");
    }
}