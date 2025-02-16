using UnityEngine;

[CreateAssetMenu(fileName ="ElectricalComponentInfo", menuName ="Electrical Component Info")]
public class ElectricalComponentInfo : ScriptableObject
{
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public string Description { get; private set; }
    [field:SerializeField] public GameObject Prefab { get; private set; }
}
