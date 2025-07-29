using UnityEngine;

public class AtaqueMachete : IComportamientoAtaque
{
    public void Atacar(GameObject objetivo)
    {
        Debug.Log("💥 Campesino ataca con su machete oxidado.");
    }
}
