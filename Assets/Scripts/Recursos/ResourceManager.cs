using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<RecursoType, int> recursos = new();

    public delegate void CambioRecurso(RecursoType tipo, int nuevoValor);
    public event CambioRecurso OnRecursoCambiado;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // <- para que sobreviva entre escenas si es necesario

    }

    public bool Gastar(RecursoType tipo, int cantidad)
    {
        if (recursos[tipo] < cantidad) return false;
        recursos[tipo] -= cantidad;
        OnRecursoCambiado?.Invoke(tipo, recursos[tipo]);
        return true;
    }

    public void Incrementar(RecursoType tipo, int cantidad)
    {
        recursos[tipo] += cantidad;
        OnRecursoCambiado?.Invoke(tipo, recursos[tipo]);
    }

    public int ObtenerValor(RecursoType tipo)
    {
        return recursos[tipo];
    }
}