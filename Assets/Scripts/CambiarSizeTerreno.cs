using UnityEngine;

[ExecuteInEditMode] // Permite ejecutar el código incluso sin Play
public class CambiarSizeTerreno : MonoBehaviour
{
    public Vector3 nuevoTamano = new Vector3(100, 0, 100); // Ancho, Alto, Largo

    void Update()
    {
        Terrain terreno = GetComponent<Terrain>();
        if (terreno != null)
        {
            // Solo actualiza si el tamaño es diferente para evitar sobreescribir
            if (terreno.terrainData.size != nuevoTamano)
            {
                terreno.terrainData.size = nuevoTamano;
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un componente Terrain en este GameObject.");
        }
    }
}

