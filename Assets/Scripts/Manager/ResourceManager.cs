using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private readonly Dictionary<RecursoType, int> recursos = new();

    public delegate void CambioRecurso(RecursoType tipo, int nuevoValor);
    public event CambioRecurso OnRecursoCambiado;

    [Header("Inicio")]
    [SerializeField] private int palmerasIniciales = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        recursos[RecursoType.Palmeras] = palmerasIniciales;
    }

    public bool Gastar(RecursoType tipo, int cantidad)
    {
        if (!recursos.ContainsKey(tipo)) return false;
        if (recursos[tipo] < cantidad) return false;

        recursos[tipo] -= cantidad;
        OnRecursoCambiado?.Invoke(tipo, recursos[tipo]);
        return true;
    }

    public void Incrementar(RecursoType tipo, int cantidad)
    {
        if (!recursos.ContainsKey(tipo)) recursos[tipo] = 0;
        recursos[tipo] += cantidad;
        OnRecursoCambiado?.Invoke(tipo, recursos[tipo]);
    }

    public int ObtenerValor(RecursoType tipo)
    {
        return recursos.ContainsKey(tipo) ? recursos[tipo] : 0;
    }
}