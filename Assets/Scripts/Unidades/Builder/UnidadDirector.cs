using UnityEngine;

public class UnidadDirector
{
    private readonly IUnidadBuilder _builder;
    private readonly UnidadFactory _factory;

    public UnidadDirector(IUnidadBuilder builder, UnidadFactory factory)
    {
        _builder = builder;
        _factory = factory;
    }

    public IUnidad ConstruirUnidad(UnidadType tipo, Vector3 spawn, Quaternion rot)
    {
        _builder.Reset();

        var prefab = _factory.GetPrefab(tipo);
        if (prefab == null)
        {
            Debug.LogError($"UnidadDirector: no hay prefab para {tipo}");
            return null;
        }

        _builder.SetPrefab(prefab);
        _builder.SetSpawn(spawn, rot);

        // Stats por tipo (ejemplo simple; ajusta a tus valores)
        switch (tipo)
        {
            case UnidadType.Campesino: _builder.SetStats(100, 3.5f); break;
            case UnidadType.Esperancita: _builder.SetStats(60, 4.2f); break;
            case UnidadType.Morenito: _builder.SetStats(200, 3.0f); break;
            case UnidadType.Chaman: _builder.SetStats(80, 3.2f); break;
            default: _builder.SetStats(100, 3.5f); break;
        }

        return _builder.Build();
    }
}
