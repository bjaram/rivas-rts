using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Sistema de colocación")]
    public PlacementSystem placement;
    
    [Header("Botones")]
    public Button buildCasonaCommand;
    public Button buildBaseMilitarCommand;
    public Button buildMinaPalmeraCommand;
    public Button buildCafetalCommand;

    private void Start()
    {
        // Crear comandos concretos
        IBuildCommand buildCasona = new BuildCommand(placement, 0);
        IBuildCommand buildBaseMilitar = new BuildCommand(placement, 1);
        IBuildCommand buildMinaPalmera = new BuildCommand(placement, 2);
        IBuildCommand buildCafetal = new BuildCommand(placement, 3);
        
        // Asignar comandos a los botones
        buildCasonaCommand.onClick.AddListener(() => buildCasona.Execute());
        buildBaseMilitarCommand.onClick.AddListener(() => buildBaseMilitar.Execute());
        buildMinaPalmeraCommand.onClick.AddListener(() => buildMinaPalmera.Execute());
        buildCafetalCommand.onClick.AddListener(() => buildCafetal.Execute());
    }
}
