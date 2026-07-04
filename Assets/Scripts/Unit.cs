using UnityEngine;

public enum UnitType
{
    Infantry,
    Archer,
    Cavalry,
    HorseArcher
}

public enum UnitState
{
    InPosition,
    Move,
    Charge,
    Retreat
}

public class Unit
{
    public string Name;
    public int Hp;
    public float MoveSpeed = 5.0f;
    public float RotateSpeed = 200.0f;
    public Vector3 Position;
    public Vector3 Direction;
    public UnitType Type;
    public UnitState State;

    public Unit(string name, Vector3 position, UnitType type)
    {
        Name = name;
        Position = position;
        Type = type;
        State = UnitState.InPosition;
    }

    public void Info()
    {
        Debug.Log($"({Position.x}, {Position.y}, {Position.z})");
    }

}
