using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TroopController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private int _troopId;

    private Troop _troop;
    private int _maxRow = -1;

    private void Start()
    {
        List<UnitController> controllers = GetComponentsInChildren<UnitController>().ToList();
        foreach (UnitController controller in controllers)
        {
            controller.Init();
        }
        List<Unit> units = controllers.Select(c => c.Unit).ToList();
        _troop = new Troop(_troopId, units);
        _troop.SetFormation(FormationType.LineFormation);
    }

    private void Update()
    {
        if(_maxRow >= 1 && _maxRow != _troop.MaxRow)
        {
            _troop.SetMaxRow(_maxRow);
        }

        if(_troop.State == TroopState.Follow)
        {
            Vector3 centerPosition = _player.transform.position - _troop.Direction * _troop.FollowDistance;
            _troop.SetCenterPosition(centerPosition);
        }
    }

    public void Move(Vector3 centerPosition)
    {
        _troop.SetCenterPosition(centerPosition);
        _troop.State = TroopState.None;
    }

    public void Follow()
    {
        _troop.State = TroopState.Follow;
    }

    public void ChangeFormation(FormationType formationType)
    {
        _troop.SetFormation(formationType);
    }

    public void Turn(Vector3 point)
    {
        Vector3 direction = point - _troop.CenterPosition;
        direction.y = 0.0f;
        direction.Normalize();
        _troop.SetDirection(direction);
    }

}
