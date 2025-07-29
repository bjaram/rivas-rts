using UnityEngine;

public class AtaqueChancleta : IComportamientoAtaque
{
    public void Atacar(GameObject objetivo)
    {
        Debug.Log("💥 Esperancita manda su Chancletazo como mi madre...");
    }
}
