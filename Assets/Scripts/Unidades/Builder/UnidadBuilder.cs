using UnityEngine;

public class UnidadBuilder : IUnidadBuilder
{
    private GameObject _prefab;
    private Vector3 _spawnPos;
    private Quaternion _spawnRot = Quaternion.identity;
    private int _vida = 100;
    private float _vel = 3f;
    private Material _mat;

    public void Reset()
    {
        _prefab = null;
        _spawnPos = Vector3.zero;
        _spawnRot = Quaternion.identity;
        _vida = 100;
        _vel = 3f;
        _mat = null;
    }

    public void SetPrefab(GameObject basePrefab) => _prefab = basePrefab;

    public void SetSpawn(Vector3 position, Quaternion rotation)
    {
        _spawnPos = position;
        _spawnRot = rotation;
    }

    public void SetStats(int vida, float velocidad)
    {
        _vida = vida;
        _vel = velocidad;
    }

    public void SetApariencia(Material materialOpcional = null)
    {
        _mat = materialOpcional;
    }

    public IUnidad Build()
    {
        if (_prefab == null)
        {
            Debug.LogError("UnidadBuilder: prefab no asignado.");
            return null;
        }

        var go = Object.Instantiate(_prefab, _spawnPos, _spawnRot);

        // Debe implementar IUnidad (y opcionalmente IMovible / IDaniable)
        if (!go.TryGetComponent<IUnidad>(out var unidad))
        {
            Debug.LogError($"UnidadBuilder: el prefab {_prefab.name} no implementa IUnidad.");
            Object.Destroy(go);
            return null;
        }

        // Stats básicos si los expone el componente concreto
        // Intentamos setearlos si existen campos públicos comunes (ej. Peon/Campesino implementan IDaniable/IMovible)
        if (go.TryGetComponent<IDaniable>(out var daniable))
        {
            // Si tu IDaniable no expone setter de vida, puedes crear un método público “ConfigurarVida(int)”
            // o exponer un método Init en las clases concretas. Aquí solo un ejemplo simple:
            // (si no tienes API, omite esta parte)
            // daniable.SetVidaBase(_vida); // <- si lo tienes
        }

        if (go.TryGetComponent<IMovible>(out var mov))
        {
            // mov.SetVelocidad(_vel); // <- si tienes un setter en tus unidades
        }

        if (_mat != null)
        {
            var rend = go.GetComponentInChildren<Renderer>();
            if (rend != null) rend.material = _mat;
        }

        return unidad;
    }
}
