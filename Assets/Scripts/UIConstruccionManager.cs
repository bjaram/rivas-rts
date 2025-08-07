using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConstruccionManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoPalmeras;
    public Button botonConstruirCasona;
    public Button botonConstruirBaseMilitar;

    [Header("Construcción")]
    public GameObject sombraCasona;
    public GameObject prefabCasona;
    public GameObject sombraBaseMilitar;
    public GameObject prefabBaseMilitar;

    private bool casonaConstruida = false;
    private bool baseMilitarConstruida = false;

    private void Start()
    {
        botonConstruirCasona.onClick.AddListener(ConstruirCasona);
        botonConstruirBaseMilitar.onClick.AddListener(ConstruirBaseMilitar);
        botonConstruirCasona.gameObject.SetActive(false);
        botonConstruirBaseMilitar.gameObject.SetActive(false);
    }

    private void Update()
    {
        int cantidad = ResourceManager.Instance.ObtenerValor(RecursoType.Palmeras);
        textoPalmeras.text = $"🌴 Palmeras: {cantidad}";

        if (!casonaConstruida && cantidad >= 1000)
            botonConstruirCasona.gameObject.SetActive(true);

        if (casonaConstruida && !baseMilitarConstruida && cantidad >= 1000)
            botonConstruirBaseMilitar.gameObject.SetActive(true);
    }

    private void ConstruirCasona()
    {
        if (ResourceManager.Instance.Gastar(RecursoType.Palmeras, 1000))
        {
            Instantiate(prefabCasona, sombraCasona.transform.position, Quaternion.identity);
            Destroy(sombraCasona);
            casonaConstruida = true;
            botonConstruirCasona.gameObject.SetActive(false);
        }
    }

    private void ConstruirBaseMilitar()
    {
        if (ResourceManager.Instance.Gastar(RecursoType.Palmeras, 1000))
        {
            Instantiate(prefabBaseMilitar, sombraBaseMilitar.transform.position, Quaternion.identity);
            Destroy(sombraBaseMilitar);
            baseMilitarConstruida = true;
            botonConstruirBaseMilitar.gameObject.SetActive(false);
        }
    }
}