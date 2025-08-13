using UnityEngine;

public class Filibustero : MonoBehaviour
{
    public int health;

    public void ReceiveDamage(int damageToInflict)
    {
        health -= damageToInflict;
    }
}
