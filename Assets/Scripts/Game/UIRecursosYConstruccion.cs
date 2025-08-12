using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecursosYConstruccion : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textoPalmeras;
    [SerializeField] private Button btnCrearPeon;
    [SerializeField] private Button btnConstruirCasona;
    [SerializeField] private Button btnConstruirBaseMilitar;

    [Header("Costos")]
    [SerializeField] private int costoPeon = 500;
    [SerializeField] private int costoCasona = 1000;
    [SerializeField] private int costoBaseMilitar = 1500;

    [Header("Prefabs")]
    [SerializeField] private GameObject prefabCasona;
    [SerializeField] private GameObject prefabBaseMilitar;

    [Header("Sombras/Lugares")]
    [SerializeField] private Transform sombraCasona;
    [SerializeField] private Transform sombraBaseMilitar;

    [Header("Spawns")]
    [SerializeField] private Transform peonSpawnFallback; // por si no existe casona aún

    private Casona casonaActiva;
    private bool casonaConstruida = false;
    private bool baseMilitarConstruida = false;

    private void Awake()
    {
        if (btnCrearPeon) btnCrearPeon.onClick.AddListener(OnClickCrearPeon);
        if (btnConstruirCasona) btnConstruirCasona.onClick.AddListener(OnClickConstruirCasona);
        if (btnConstruirBaseMilitar) btnConstruirBaseMilitar.onClick.AddListener(OnClickConstruirBaseMilitar);

        SetBotonActivo(btnCrearPeon, false);
        SetBotonActivo(btnConstruirCasona, false);
        SetBotonActivo(btnConstruirBaseMilitar, false);
    }

    private void Update()
    {
        int palmeras = ResourceManager.Instance.ObtenerValor(RecursoType.Palmeras);
        if (textoPalmeras) textoPalmeras.text = $"🌴 Palmeras: {palmeras}";

        SetBotonActivo(btnCrearPeon, palmeras >= costoPeon);
        SetBotonActivo(btnConstruirCasona, !casonaConstruida && palmeras >= costoCasona);
        SetBotonActivo(btnConstruirBaseMilitar, !baseMilitarConstruida && palmeras >= costoBaseMilitar);
    }

    private void SetBotonActivo(Button b, bool activo)
    {
        if (!b) return;
        if (b.gameObject.activeSelf != activo) b.gameObject.SetActive(activo);
        b.interactable = activo;
    }

    private void OnClickCrearPeon()
    {
        if (!ResourceManager.Instance.Gastar(RecursoType.Palmeras, costoPeon)) { Debug.Log("⚠️ Faltan palmeras para Peón."); return; }

        Vector3 spawnPos = peonSpawnFallback ? peonSpawnFallback.position : Vector3.zero;
        if (casonaActiva && casonaActiva.GetSpawnPoint()) spawnPos = casonaActiva.GetSpawnPoint().position;

        if (!UnidadFactory.Instance) { Debug.LogError("❌ Falta UnidadFactory en escena."); return; }
        var u = UnidadFactory.Instance.CrearUnidad(UnidadType.Peon, spawnPos);
        if (u != null) Debug.Log("✅ Peón creado.");
    }

    private void OnClickConstruirCasona()
    {
        if (casonaConstruida) { Debug.Log("ℹ️ Casona ya construida."); return; }
        if (!ResourceManager.Instance.Gastar(RecursoType.Palmeras, costoCasona)) { Debug.Log("⚠️ Faltan palmeras para Casona."); return; }
        if (!prefabCasona || !sombraCasona) { Debug.LogError("❌ Prefab Casona o SombraCasona sin asignar."); return; }

        var go = Instantiate(prefabCasona, sombraCasona.position, Quaternion.identity);
        casonaActiva = go.GetComponent<Casona>();
        casonaConstruida = true;

        if (sombraCasona.gameObject) Destroy(sombraCasona.gameObject);

        Debug.Log("🏠 Casona construida.");
    }

    private void OnClickConstruirBaseMilitar()
    {
        if (baseMilitarConstruida) { Debug.Log("ℹ️ Base Militar ya construida."); return; }
        if (!ResourceManager.Instance.Gastar(RecursoType.Palmeras, costoBaseMilitar)) { Debug.Log("⚠️ Faltan palmeras para Base Militar."); return; }
        if (!prefabBaseMilitar || !sombraBaseMilitar) { Debug.LogError("❌ Prefab BaseMilitar o SombraBaseMilitar sin asignar."); return; }

        Instantiate(prefabBaseMilitar, sombraBaseMilitar.position, Quaternion.identity);
        baseMilitarConstruida = true;

        if (sombraBaseMilitar.gameObject) Destroy(sombraBaseMilitar.gameObject);

        Debug.Log("🛡️ Base Militar construida.");
    }
}
