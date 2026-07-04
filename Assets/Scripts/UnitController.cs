using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Unit _unit;
    public Unit Unit => _unit;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _unit = new Unit("Infantry", new Vector3(0.0f, 0.0f, 0.0f), UnitType.Infantry);
    }

    private void Update()
    {
        if (_unit == null)
            return;

        UpdateState();

        switch (_unit.State)
        {
            case UnitState.InPosition:
                UpdateInPosition();
                break;

            case UnitState.Move:
                UpdateMove();
                break;

            case UnitState.Charge:
                break;

            case UnitState.Retreat:
                break;
        }
    }

    private void UpdateState()
    {
        Vector3 offset = _unit.Position - transform.position;
        offset.y = 0.0f;

        if (offset.sqrMagnitude > 0.01f)
            _unit.State = UnitState.Move;
        else
            _unit.State = UnitState.InPosition;
    }

    private void UpdateInPosition()
    {
        Rotate(_unit.Direction);
    }

    private void UpdateMove()
    {
        Vector3 direction = _unit.Position - transform.position;
        direction.y = 0.0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        direction.Normalize();

        Rotate(direction);
        Move();
    }

    private void Rotate(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _unit.RotateSpeed * Time.deltaTime);
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _unit.Position, _unit.MoveSpeed * Time.deltaTime);
        ResolveUnitCollision();
    }

    [SerializeField] private float unitRadius = 0.5f;
    [SerializeField] private LayerMask unitLayer;

    private void ResolveUnitCollision()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, unitRadius, unitLayer);

        foreach (Collider hit in hits)
        {
            if (hit.transform == transform)
                continue;

            Vector3 diff = transform.position - hit.transform.position;
            diff.y = 0.0f;

            float distance = diff.magnitude;

            if (distance < 0.0001f)
                continue;

            float overlap = unitRadius * 2.0f - distance;

            if (overlap > 0.0f)
            {
                Vector3 push = diff.normalized * overlap * 0.5f;
                transform.position += push;
                hit.transform.position -= push;
            }
        }
    }

}
