using System;
using System.Collections.Generic;
using UnityEngine;

public enum FormationType
{
    LineFormation,
    SquareFormation
}

public enum TroopState
{
    None,
    Follow,
    Charge,
    Retreat
}

public class Troop
{
    public static int Count = 0;

    public int Id;
    public List<Unit> Units = new List<Unit>();
    public Formation Formation { get; private set; }
    public Vector3 CenterPosition { get; private set; }
    public Vector3 Direction;
    public int MaxRow { get; private set; }
    public float Spacing;
    public float FollowDistance;
    public FormationType FormationType;
    public TroopState State;

    public Troop(List<Unit> units)
    {
        Id = ++Count;
        Units = units;
        Formation = new LineFormation();
        CenterPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Direction = Vector3.Normalize(new Vector3(0.0f, 0.0f, 1.0f));
        MaxRow = 5;
        Spacing = 2.0f;
        FollowDistance = 5.0f;
        FormationType = FormationType.LineFormation;
        State = TroopState.None;
    }

    public void SetFormation(FormationType formationType)
    {
        FormationType = formationType;

        switch (formationType)
        {
            case FormationType.LineFormation:
                Formation = new LineFormation();
                break;

            case FormationType.SquareFormation:
                Formation = new SquareFormation();
                break;
        }

        UpdateFormation();
    }

    public void SetCenterPosition(Vector3 centerPosition)
    {
        CenterPosition = centerPosition;
        UpdateFormation();
    }

    public void SetMaxRow(int maxRow)
    {
        if (maxRow < 1) return;
        
        MaxRow = maxRow;
        UpdateFormation();
    }

    public void UpdateFormation()
    {
        if (Formation == null) return;
        
        List<List<Vector3>> result = Formation.CalculatePositions(Units.Count, CenterPosition, Direction, MaxRow, Spacing);
        List<Vector3> positions = result[0];
        List<Vector3> directions = result[1];

        for (int i = 0; i < Units.Count; i++)
        {
            Units[i].Position = positions[i];
            Units[i].Direction = directions[i];
            Units[i].State = UnitState.Move;
        }
    }

}
