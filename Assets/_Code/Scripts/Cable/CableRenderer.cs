using UnityEngine;

public class CableRenderer : CableProceduralCurve
{
    [field: Header("Cable Draw Settings")]
    [field: SerializeField] public float MinDistanceToCurve {get; private set;} = 0.3f;

    private Vector3 _lastEndPosition;

    private void Update()
    {
        if(EndPointTransform.position != _lastEndPosition)
        {
            DrawCable();
        }
    }

    private void DrawCable()
    {
        _lastEndPosition = EndPointTransform.position;

        if(Vector3.Distance(transform.position, _lastEndPosition) < MinDistanceToCurve)
        {
            LineRenderer line = GetLineRenderer();
            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, _lastEndPosition);
        }
        else
        {
            CalculateAndDraw();
        }
    }

    public void SetMaterial(Material newMaterial)
    {
        GetLineRenderer().material = newMaterial;
    }
}
