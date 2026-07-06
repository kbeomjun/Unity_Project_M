using UnityEngine;

public enum CommandMenu
{
    None,
    Move,
    Formation,
    Direction
}

public enum MoveCommand
{
    Move,
    Follow,
    Charge
}

public class CommandManager : MonoBehaviour
{
    private CommandMenu _commandMenu = CommandMenu.None;

    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        if (_commandMenu == CommandMenu.None)
        {
            SelectMenu();
        }
        else
        {
            ExecuteCommand();
        }
    }

    private void SelectMenu()
    {
        if (_controls.Command.F1.WasPressedThisFrame())
        {
            Debug.Log($"F1 Pressed");
            _commandMenu = CommandMenu.Move;
        }
        else if (_controls.Command.F2.WasPressedThisFrame())
        {
            Debug.Log($"F2 Pressed");
            _commandMenu = CommandMenu.Formation;
        }
        else if (_controls.Command.F3.WasPressedThisFrame())
        {
            Debug.Log($"F3 Pressed");
            _commandMenu = CommandMenu.Direction;
        }
    }

    private void ExecuteCommand()
    {
        switch (_commandMenu)
        {
            case CommandMenu.Move:
                ExecuteMoveCommand();
                break;

            case CommandMenu.Formation:
                break;

            case CommandMenu.Direction:
                break;
        }
    }

    private void ExecuteMoveCommand()
    {
        if (_controls.Command.F1.WasPressedThisFrame())
        {
            // 이동
            Debug.Log($"F1-F1 Pressed");
            ResetMenu();
        }
        else if (_controls.Command.F2.WasPressedThisFrame())
        {
            // 따라오기
            Debug.Log($"F1-F2 Pressed");
            ResetMenu();
        }
        else if (_controls.Command.F3.WasPressedThisFrame())
        {
            // 돌격
            Debug.Log($"F1-F3 Pressed");
            ResetMenu();
        }
        else if (_controls.Command.Esc.WasPressedThisFrame())
        {
            Debug.Log($"F1-Esc Pressed");
            ResetMenu();
        }
    }

    private void ResetMenu()
    {
        _commandMenu = CommandMenu.None;
    }

}
