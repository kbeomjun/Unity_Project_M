using System;
using System.Collections.Generic;
using UnityEngine;

public enum FormationType
{
    LineFormation,
    SquareFormation
}

public class Troop
{
    private int Count = 0;

    private int _id;
    private List<Unit> _units = new List<Unit>();
    private Formation _formation;
    private Vector3 _centerPosition;
    private Vector3 _direction;
    private int _maxRow;
    private float _spacing;

    private FormationType _formationType;

    public int MaxRow
    {
        get => _maxRow;

        set 
        {
            if (value < 1) return;
            
            Debug.Log($"Troop{_id} - SetMaxLength:{value}");
            _maxRow = value;
            UpdateFormation();
        }
    }

    public FormationType FormationType
    {
        get => _formationType;
    }

    public Troop(List<Unit> units)
    {
        _id = ++Count;
        _units = units;
        _formation = new LineFormation();
        _centerPosition = new Vector3(0.0f, 0.0f, 0.0f);
        _direction = Vector3.Normalize(new Vector3(0.0f, 0.0f, 1.0f));
        _maxRow = 5;
        _spacing = 2.0f;
        _formationType = FormationType.LineFormation;
    }

    public void SetFormation(FormationType formationType)
    {
        Debug.Log($"Troop{_id} - SetFormation:{formationType.ToString()}");

        _formationType = formationType;

        switch (formationType)
        {
            case FormationType.LineFormation:
                _formation = new LineFormation();
                break;

            case FormationType.SquareFormation:
                _formation = new SquareFormation();
                break;
        }

        UpdateFormation();
    }

    public void UpdateFormation()
    {
        if (_formation == null) return;

        List<Vector3> positions = _formation.CalculatePositions(_units.Count, _centerPosition, _direction, _maxRow, _spacing)[0];
        List<Vector3> directions = _formation.CalculatePositions(_units.Count, _centerPosition, _direction, _maxRow, _spacing)[1];

        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].Position = positions[i];
            _units[i].Direction = directions[i];
            _units[i].State = UnitState.Move;
        }
    }

}
