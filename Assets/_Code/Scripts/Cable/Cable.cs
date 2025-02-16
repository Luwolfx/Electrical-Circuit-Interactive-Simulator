using UnityEngine;

public class Cable : MonoBehaviour
{
    [field:Header("Cable References")]
    [field:SerializeField] public CableRenderer StartPoint { get; private set; }
    [field:SerializeField] public Transform EndPoint { get; private set; }

    [Header("Cable Settings")]
    [SerializeField] private Material _cableMaterial;
    [SerializeField] private Material _cableEnergyMaterial;


    public void SetStartPoint(Vector3 position)
    {
        transform.position = position;
    }

    public void UpdateEndPointPosition(Vector3 position)
    {
        EndPoint.position = position;
    }

    public void ToogleEnergy(bool toggle)
    {
        if(!toggle)
            StartPoint.SetMaterial(_cableMaterial);
        else
            StartPoint.SetMaterial(_cableEnergyMaterial);

    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
