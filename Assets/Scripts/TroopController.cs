using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TroopController : MonoBehaviour
{
    [SerializeField] private FormationType _newFormation = FormationType.LineFormation;
    [SerializeField] private int _maxRow = -1;

    private Troop _troop;

    void Start()
    {
        List<UnitController> controllers = GetComponentsInChildren<UnitController>().ToList();
        List<Unit> units = controllers.Select(c => c.Unit).ToList();
        _troop = new Troop(units);
        _troop.SetFormation(_newFormation);
    }

    void Update()
    {
        if(_newFormation != _troop.FormationType)
        {
            _troop.SetFormation(_newFormation);
        }

        if(_maxRow >= 1 && _maxRow != _troop.MaxRow)
        {
            _troop.MaxRow = _maxRow;
        }
    }

}
