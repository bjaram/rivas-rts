using UnityEngine;
using UnityEngine.UI;

public class UIEntrenamiento : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button btnEnviarAEntrenar;

    [Header("Destino (se asigna solo en runtime)")]
    public BaseMilitar baseMilitarDestino;

    private SeleccionSimple selector;

    private void Awake()
    {
        selector = Object.FindFirstObjectByType<SeleccionSimple>();
        if (btnEnviarAEntrenar != null)
            btnEnviarAEntrenar.onClick.AddListener(EnviarSeleccionados);
        else
            Debug.LogWarning("UIEntrenamiento: asigna el botón BtnEntrenarPeones en el inspector.");
    }

    private void Update()
    {
        // Trata de tener referencia, si aún no la hay
        if (baseMilitarDestino == null)
            baseMilitarDestino = Object.FindFirstObjectByType<BaseMilitar>();

        bool hayBase = baseMilitarDestino != null;
        int seleccionCount = (selector != null && selector.Seleccionados != null) ? selector.Seleccionados.Count : 0;

        if (btnEnviarAEntrenar != null)
        {
            // SIEMPRE visible
            if (!btnEnviarAEntrenar.gameObject.activeSelf)
                btnEnviarAEntrenar.gameObject.SetActive(true);

            // Solo se habilita cuando hay base y al menos un peón seleccionado
            btnEnviarAEntrenar.interactable = hayBase && seleccionCount > 0;
        }
    }

    private void EnviarSeleccionados()
    {
        if (baseMilitarDestino == null)
            baseMilitarDestino = Object.FindFirstObjectByType<BaseMilitar>();

        if (baseMilitarDestino == null)
        {
            Debug.LogWarning("⚠️ Aún no has construido la Base Militar.");
            return;
        }

        if (selector == null || selector.Seleccionados == null || selector.Seleccionados.Count == 0)
        {
            Debug.Log("ℹ️ Selecciona uno o más peones para entrenar.");
            return;
        }

        int enviados = 0;
        foreach (var peon in selector.Seleccionados)
        {
            if (peon == null) continue;
            peon.EnviarAEntrenamiento(baseMilitarDestino);
            enviados++;
        }

        Debug.Log($"🎯 Enviados {enviados} peones a entrenar en {baseMilitarDestino.name}.");
    }

    // Llamado por UIConstruccionManager tras construir la base:
    public void SetBaseMilitarDestino(BaseMilitar bm) => baseMilitarDestino = bm;
}