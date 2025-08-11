using UnityEngine;

public class EntradaBaseTrigger : MonoBehaviour
{
    public BaseMilitar baseMilitar;

    private void Reset()
    {
        if (baseMilitar == null)
            baseMilitar = GetComponentInParent<BaseMilitar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var peon = other.GetComponent<Peon>();
        if (peon != null && peon.DeseaEntrenar && baseMilitar != null)
        {
            baseMilitar.TryEncolar(peon);
        }
    }
}
