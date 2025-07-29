using UnityEngine;

public class AtaquePunno : IComportamientoAtaque
{
    public void Atacar(GameObject objetivo)
    {
        Debug.Log("💥 Morenito ataca con super Vergazo a lo tico.");
    }
}
