using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrainer : MonoBehaviour
{
    [Header("Entrenamiento")]
    [SerializeField] private int capacidadSlots = 2;          // Peones simultáneos
    [SerializeField] private float tiempoEntrenamiento = 10f; // en segundos

    [Header("Puntos (hijos de la Casona)")]
    [SerializeField] private Transform puntoHold;   // dónde se “guardan” los peones mientras entrenan (puede estar dentro)
    [SerializeField] private Transform puntoSpawn;  // dónde aparece la unidad resultante (sobre NavMesh)

    private readonly Queue<Peon> cola = new();
    private int slotsOcupados = 0;

    public Transform GetSpawnPoint() => puntoSpawn;

    /// <summary>Encola un peón para entrenar; si hay slots libres, comienza de una vez.</summary>
    public void Encolar(Peon peon)
    {
        if (peon == null) return;

        // el peón deja de recolectar y se “aparta”
        peon.PrepararseParaEntrenarSimple(puntoHold != null ? puntoHold.position : transform.position);

        cola.Enqueue(peon);
        ProcesarCola();
    }

    private void ProcesarCola()
    {
        while (slotsOcupados < capacidadSlots && cola.Count > 0)
        {
            var p = cola.Dequeue();
            StartCoroutine(CorEntrenar(p));
            slotsOcupados++;
        }
    }

    private IEnumerator CorEntrenar(Peon peon)
    {
        if (peon == null)
        {
            slotsOcupados--;
            yield break;
        }

        // Espera el tiempo de entrenamiento
        yield return new WaitForSeconds(tiempoEntrenamiento);

        // Elegir tipo al azar
        UnidadType tipo = ElegirTipoAleatorio();

        // Construir con Builder + Factory
        if (UnidadFactory.Instance == null)
        {
            Debug.LogError("❌ UnidadFactory.Instance no disponible.");
        }
        else
        {
            var builder = new UnidadBuilder();
            var director = new UnidadDirector(builder, UnidadFactory.Instance);

            Vector3 spawn = puntoSpawn != null ? puntoSpawn.position : transform.position + Vector3.forward * 1.5f;
            var unidad = director.ConstruirUnidad(tipo, spawn, Quaternion.identity);

            if (unidad != null)
                Debug.Log($"✅ {peon.name} convertido en {tipo} (Builder).");
            else
                Debug.LogError("❌ Falló la construcción de la unidad con Builder.");
        }

        // Destruir peón original
        if (peon != null) Destroy(peon.gameObject);

        slotsOcupados--;
        ProcesarCola(); // sigue con la cola
    }

    private UnidadType ElegirTipoAleatorio()
    {
        UnidadType[] pool = {
            UnidadType.Campesino,
            UnidadType.Esperancita,
            UnidadType.Morenito,
            UnidadType.Chaman
        };
        return pool[Random.Range(0, pool.Length)];
    }
}
