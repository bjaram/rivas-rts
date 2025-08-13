using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEntrenamiento : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button btnEnviarAEntrenar;

    [Header("Destinos posibles")]
    [SerializeField] private BaseMilitar baseMilitarDestino;   // flujo BaseMilitar (simplificado)
    [SerializeField] private SimpleTrainer trainer;            // flujo Casona con trainer simple

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
        // Si no hay destino asignado aún (por construcción en runtime), intenta localizarlo
        if (baseMilitarDestino == null)
            baseMilitarDestino = Object.FindFirstObjectByType<BaseMilitar>();
        if (trainer == null)
            trainer = Object.FindFirstObjectByType<SimpleTrainer>();

        bool hayDestino = (baseMilitarDestino != null) || (trainer != null);
        int seleccionCount = (selector != null && selector.Seleccionados != null) ? selector.Seleccionados.Count : 0;

        if (btnEnviarAEntrenar != null)
        {
            if (!btnEnviarAEntrenar.gameObject.activeSelf)
                btnEnviarAEntrenar.gameObject.SetActive(true);

            btnEnviarAEntrenar.interactable = hayDestino && seleccionCount > 0;
        }
    }

    private void EnviarSeleccionados()
    {
        // Resolver destino preferente: si hay BaseMilitar úsala; si no, intenta con trainer
        if (baseMilitarDestino == null && trainer == null)
        {
            // Intento final por si se creó en este frame
            baseMilitarDestino = Object.FindFirstObjectByType<BaseMilitar>();
            trainer = Object.FindFirstObjectByType<SimpleTrainer>();
        }

        if (baseMilitarDestino == null && trainer == null)
        {
            Debug.LogWarning("⚠️ No hay Base Militar ni SimpleTrainer disponibles en escena.");
            return;
        }

        if (selector == null || selector.Seleccionados == null || selector.Seleccionados.Count == 0)
        {
            Debug.Log("ℹ️ Selecciona uno o más peones para entrenar.");
            return;
        }

        var copia = new List<Peon>(selector.Seleccionados);
        int enviados = 0;

        foreach (var peon in copia)
        {
            if (peon == null) continue;

            if (baseMilitarDestino != null)
            {
                // Flujo BaseMilitar simplificado (cola interna)
                baseMilitarDestino.EnviarAPeonEntrenar(peon);
            }
            else
            {
                // Flujo Casona con trainer simple
                trainer.Encolar(peon);
            }
            enviados++;
        }

        string destinoStr = baseMilitarDestino != null ? baseMilitarDestino.name : trainer.name;
        Debug.Log($"🎯 Enviados {enviados} peones a entrenar en {destinoStr}.");
    }

    // === Setters para ambos flujos ===
    public void SetBaseMilitarDestino(BaseMilitar bm)
    {
        baseMilitarDestino = bm;
        // Si eliges BaseMilitar, “deshabilita” el trainer actual para evitar ambigüedad
        // (Puedes comentar esta línea si quieres permitir ambos)
        trainer = null;
    }

    public void SetTrainer(SimpleTrainer t)
    {
        trainer = t;
        // Si eliges trainer en casona, “deshabilita” BaseMilitar para evitar ambigüedad
        baseMilitarDestino = null;
    }
}
