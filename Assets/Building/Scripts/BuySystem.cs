using UnityEngine;
using UnityEngine.UI;

public class BuySystem : MonoBehaviour
{
    
    public GameObject buildingsPanel;
    public GameObject unitsPanel;
    
    public Button buildingsButton;
    public Button unitsButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unitsButton.onClick.AddListener(UnitsCategorySelected);
        buildingsButton.onClick.AddListener(BuildingsCategorySelected);
        
        unitsPanel.SetActive(false);
        buildingsPanel.SetActive(true);
    }

    private void BuildingsCategorySelected()
    {
        unitsPanel.SetActive(false);
        buildingsPanel.SetActive(true);
        Debug.Log("Buildings category selected");
    }

    private void UnitsCategorySelected()
    {
        buildingsPanel.SetActive(false);
        unitsPanel.SetActive(true);
        Debug.Log("Units category selected");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
