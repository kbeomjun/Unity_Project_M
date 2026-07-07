using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TroopController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    private Troop _troop;
    private FormationType _newFormation = FormationType.LineFormation;
    private int _maxRow = -1;

    private void Start()
    {
        List<UnitController> controllers = GetComponentsInChildren<UnitController>().ToList();
        List<Unit> units = controllers.Select(c => c.Unit).ToList();
        _troop = new Troop(units);
        _troop.SetFormation(_newFormation);
    }

    private void Update()
    {
        if(_newFormation != _troop.FormationType)
        {
            _troop.SetFormation(_newFormation);
        }

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

}
