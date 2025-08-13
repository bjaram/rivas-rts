using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConstruccionManager : MonoBehaviour
{
    [Header("UI Recursos")]
    [SerializeField] private TextMeshProUGUI textoPalmeras;

    [Header("Botones de construcción")]
    [SerializeField] private Button botonConstruirCasona;
    [SerializeField] private Button botonConstruirBaseMilitar;
    [SerializeField] private Button botonCrearPeon; // botón de crear peón

    [Header("Costos")]
    [SerializeField] private int costoPeon = 500;
    [SerializeField] private int costoCasona = 1000;
    [SerializeField] private int costoBaseMilitar = 1500;

    [Header("Prefabs a crear")]
    [SerializeField] private GameObject prefabCasona;
    [SerializeField] private GameObject prefabBaseMilitar;

    [Header("Sombras/Lugares de construcción")]
    [SerializeField] private Transform sombraCasona;
    [SerializeField] private Transform sombraBaseMilitar;

    [Header("Spawns")]
    [SerializeField] private Transform peonSpawnFallback;

    // Estado runtime
    private Casona casonaActiva;
    private bool casonaConstruida = false;
    private bool baseMilitarConstruida = false;

    private void Start()
    {
        if (botonConstruirCasona != null) botonConstruirCasona.onClick.AddListener(ConstruirCasona);
        if (botonConstruirBaseMilitar != null) botonConstruirBaseMilitar.onClick.AddListener(ConstruirBaseMilitar);
        if (botonCrearPeon != null) botonCrearPeon.onClick.AddListener(CrearPeon);

        // Inicial: los de construir se controlan por recursos en Update; CrearPeon SIEMPRE oculto al inicio
        SetVisible(botonConstruirCasona, false);
        SetVisible(botonConstruirBaseMilitar, false);
        SetVisible(botonCrearPeon, false); // ⬅️ clave
    }

    private void Update()
    {
        if (ResourceManager.Instance == null) return;

        int palmeras = ResourceManager.Instance.ObtenerValor(RecursoType.Palmeras);
        if (textoPalmeras != null)
            textoPalmeras.text = $"🌴 Palmeras: {palmeras}";

        // Botones de construir (orden correcto)
        SetVisible(botonConstruirCasona, !casonaConstruida && palmeras >= costoCasona);
        SetVisible(botonConstruirBaseMilitar, casonaConstruida && !baseMilitarConstruida && palmeras >= costoBaseMilitar);

        // Botón Crear Peón: SOLO visible si YA existe la Casona
        if (!casonaConstruida)
        {
            SetVisible(botonCrearPeon, false);
        }
        else
        {
            SetVisible(botonCrearPeon, true);
            // Interactuable si hay recursos y la Casona está Activa
            bool casonaActivaYOK = (casonaActiva != null && casonaActiva.ObtenerEstado() == EstadoBase.Activa);
            if (botonCrearPeon != null)
                botonCrearPeon.interactable = casonaActivaYOK && palmeras >= costoPeon;
        }
    }

    private void SetVisible(Button b, bool visible)
    {
        if (b == null) return;
        if (b.gameObject.activeSelf != visible) b.gameObject.SetActive(visible);
        // El interactable se controla aparte donde corresponda
    }

    // ============================
    // Botones
    // ============================

    private void CrearPeon()
    {
        if (casonaActiva == null)
        {
            Debug.Log("⚠️ No hay Casona activa.");
            return;
        }

        if (casonaActiva.ObtenerEstado() != EstadoBase.Activa)
        {
            Debug.Log("ℹ️ La Casona aún no está activa.");
            return;
        }

        if (!ResourceManager.Instance.Gastar(RecursoType.Palmeras, costoPeon))
        {
            Debug.Log("⚠️ Faltan palmeras para crear Peón.");
            return;
        }

        Vector3 spawnPos = peonSpawnFallback ? peonSpawnFallback.position : Vector3.zero;
        if (casonaActiva.GetSpawnPoint() != null)
            spawnPos = casonaActiva.GetSpawnPoint().position;

        if (!UnidadFactory.Instance)
        {
            Debug.LogError("❌ Falta UnidadFactory en escena.");
            return;
        }

        var u = UnidadFactory.Instance.CrearUnidad(UnidadType.Peon, spawnPos);
        if (u != null) Debug.Log("✅ Peón creado.");
    }

    private void ConstruirCasona()
    {
        if (casonaConstruida)
        {
            Debug.Log("ℹ️ La Casona ya existe.");
            return;
        }

        if (!ResourceManager.Instance.Gastar(RecursoType.Palmeras, costoCasona))
        {
            Debug.Log("⚠️ Faltan palmeras para Casona.");
            return;
        }

        if (prefabCasona == null || sombraCasona == null)
        {
            Debug.LogError("❌ Prefab Casona o SombraCasona sin asignar.");
            return;
        }

        GameObject go = Instantiate(prefabCasona, sombraCasona.position, Quaternion.identity);
        casonaActiva = go.GetComponent<Casona>();
        casonaConstruida = true;

        if (sombraCasona.gameObject != null) Destroy(sombraCasona.gameObject);

        // Mostrar el botón "Crear Peón" recién ahora
        SetVisible(botonCrearPeon, true);

        Debug.Log("🏠 Casona construida.");
    }

    private void ConstruirBaseMilitar()
    {
        if (baseMilitarConstruida)
        {
            Debug.Log("ℹ️ La Base Militar ya existe.");
            return;
        }

        if (!ResourceManager.Instance.Gastar(RecursoType.Palmeras, costoBaseMilitar))
        {
            Debug.Log("⚠️ Faltan palmeras para Base Militar.");
            return;
        }

        if (prefabBaseMilitar == null || sombraBaseMilitar == null)
        {
            Debug.LogError("❌ Prefab BaseMilitar o SombraBaseMilitar sin asignar.");
            return;
        }

        GameObject nuevaBase = Instantiate(prefabBaseMilitar, sombraBaseMilitar.position, Quaternion.identity);
        baseMilitarConstruida = true;

        if (sombraBaseMilitar.gameObject != null) Destroy(sombraBaseMilitar.gameObject);

        Debug.Log("🛡️ Base Militar construida.");

        var uiEntrenamiento = Object.FindFirstObjectByType<UIEntrenamiento>();
        if (uiEntrenamiento != null)
        {
            var bm = nuevaBase.GetComponent<BaseMilitar>();
            if (bm != null)
            {
                uiEntrenamiento.SetBaseMilitarDestino(bm);
                Debug.Log("🔗 UIEntrenamiento ahora apunta a la Base Militar recién construida.");
            }
        }
    }
}