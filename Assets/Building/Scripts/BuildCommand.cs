using UnityEngine;

public class BuildCommand : IBuildCommand
{
    private PlacementSystem placementSystem;
    private int buildingId;

    public BuildCommand(PlacementSystem placementSystem, int buildingId)
    {
        this.placementSystem = placementSystem;
        this.buildingId = buildingId;
    }

    public void Execute()
    {
        Debug.Log($"Ejecutando comando para construir ID: {buildingId}");
        placementSystem.StartPlacement(buildingId);
    }
}

public interface IBuildCommand
{
    void Execute();
}
