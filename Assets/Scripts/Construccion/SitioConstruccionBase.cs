using System.Collections.Generic;
using UnityEngine;

public abstract class SitioConstruccionBase : MonoBehaviour
{
    [Header("Configuración")]
    public int recursosNecesarios = 1000;
    protected int recursosAcumulados = 0;

    [Header("Cubos de progreso (en orden)")]
    public List<GameObject> cubosProgreso;

    [Header("Prefab del edificio a construir")]
    public GameObject prefabEdificioFinal;
    public Vector3 offsetSpawnEdificio;

    [Header("Recurso requerido")]
    public RecursoType tipoRecurso = RecursoType.Palmeras;

    [Header("Opciones")]
    public bool usarResourceManager = true;

    protected virtual void Start()
    {
        foreach (GameObject cubo in cubosProgreso)
        {
            cubo.SetActive(false);
        }
    }

    public virtual void RecibirRecursos(Peon peon, int cantidad)
    {
        recursosAcumulados += cantidad;
        Debug.Log($"Recursos acumulados: {recursosAcumulados}/{recursosNecesarios}"); // ← Añade esta línea

        if (usarResourceManager)
        {
            ResourceManager.Instance.Incrementar(tipoRecurso, cantidad);
        }

        ActualizarProgresoVisual();

        if (recursosAcumulados >= recursosNecesarios)
        {
            ConstruirEdificioFinal();
        }
    }

    protected void ActualizarProgresoVisual()
    {
        float porcentaje = (float)recursosAcumulados / recursosNecesarios;

        for (int i = 0; i < cubosProgreso.Count; i++)
        {
            float umbral = (i + 1) * 0.25f;
            cubosProgreso[i].SetActive(porcentaje >= umbral);
        }
    }

    protected virtual void ConstruirEdificioFinal()
    {
        if (prefabEdificioFinal != null)
        {
            Instantiate(prefabEdificioFinal, transform.position + offsetSpawnEdificio, Quaternion.identity);
            Debug.Log("✅ Edificio construido: " + prefabEdificioFinal.name);
        }
        else
        {
            Debug.LogWarning("⚠️ Prefab del edificio final no asignado.");
        }

        Destroy(gameObject); // eliminar sombra de construcción
    }

    public bool EstaCompleto()
    {
        return recursosAcumulados >= recursosNecesarios;
    }
}