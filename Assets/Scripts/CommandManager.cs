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
    [SerializeField] private TroopController[] _troopControllers;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _flag;
    [SerializeField] private CommandUIManager _uiManager;

    private CommandMenu _commandMenu = CommandMenu.None;
    private int _selectedTroop = 0;

    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();
        _flag.SetActive(false);
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
        // 숫자는 항상 최우선
        if (HandleTroopSelection())
            return;

        switch (_commandMenu)
        {
            case CommandMenu.None:
                HandleRootInput();
                break;

            case CommandMenu.Move:
                ExecuteMoveCommand();
                break;

            case CommandMenu.Formation:
                ExecuteFormationCommand();
                break;

            case CommandMenu.Direction:
                ExecuteDirectionCommand();
                break;
        }

        if (_flag.activeSelf)
            UpdateFlag();
    }

    private bool HandleTroopSelection()
    {
        int index = -1;

        if (_controls.Command.One.WasPressedThisFrame())
            index = 0;
        else if (_controls.Command.Two.WasPressedThisFrame())
            index = 1;
        else if (_controls.Command.Three.WasPressedThisFrame())
            index = 2;
        else if (_controls.Command.Four.WasPressedThisFrame())
            index = 3;

        if (index == -1)
            return false;

        if (index >= _troopControllers.Length)
            return true;

        _selectedTroop = index;
        _commandMenu = CommandMenu.None;
        _flag.SetActive(false);
        _uiManager.Show(CommandMenu.None);

        Debug.Log($"Troop {index + 1} Selected");
        return true;
    }

    private void HandleRootInput()
    {
        if (_controls.Command.F1.WasPressedThisFrame())
        {
            _commandMenu = CommandMenu.Move;
            _flag.SetActive(true);
            _uiManager.Show(CommandMenu.Move);
        }
        else if (_controls.Command.F2.WasPressedThisFrame())
        {
            _commandMenu = CommandMenu.Formation;
            _flag.SetActive(true);
            _uiManager.Show(CommandMenu.Formation);
        }
        else if (_controls.Command.F3.WasPressedThisFrame())
        {
            _commandMenu = CommandMenu.Direction;
            _flag.SetActive(true);
            _uiManager.Show(CommandMenu.Direction);
        }
        else if (_controls.Command.Esc.WasPressedThisFrame())
        {
            _uiManager.HideAll();
        }
    }

    private void ExecuteMoveCommand()
    {
        if (_controls.Command.F1.WasPressedThisFrame())
        {
            if (TryGetLookPoint(out Vector3 point))
            {
                point.y += 1.0f;
                _troopControllers[_selectedTroop].Move(point);
            }

            FinishCommand();
        }
        else if (_controls.Command.F2.WasPressedThisFrame())
        {
            _troopControllers[_selectedTroop].Follow();
            FinishCommand();
        }
        else if (_controls.Command.F3.WasPressedThisFrame())
        {
            Debug.Log("Charge");
            FinishCommand();
        }
        else if (_controls.Command.Esc.WasPressedThisFrame())
        {
            BackToRoot();
        }
    }

    private void ExecuteFormationCommand()
    {
        if (_controls.Command.F1.WasPressedThisFrame())
        {
            _troopControllers[_selectedTroop].ChangeFormation(FormationType.LineFormation);
            FinishCommand();
        }
        else if (_controls.Command.F2.WasPressedThisFrame())
        {
            FinishCommand();
        }
        else if (_controls.Command.F3.WasPressedThisFrame())
        {
            FinishCommand();
        }
        else if (_controls.Command.F4.WasPressedThisFrame())
        {
            _troopControllers[_selectedTroop].ChangeFormation(FormationType.SquareFormation);
            FinishCommand();
        }
        else if (_controls.Command.Esc.WasPressedThisFrame())
        {
            BackToRoot();
        }
    }

    private void ExecuteDirectionCommand()
    {
        if (_controls.Command.F1.WasPressedThisFrame())
        {
            if (TryGetLookPoint(out Vector3 point))
            {
                _troopControllers[_selectedTroop].Turn(point);
            }

            FinishCommand();
        }
        else if (_controls.Command.Esc.WasPressedThisFrame())
        {
            BackToRoot();
        }
    }

    private void FinishCommand()
    {
        _commandMenu = CommandMenu.None;
        _flag.SetActive(false);
        _uiManager.HideAll();
    }

    private void BackToRoot()
    {
        _commandMenu = CommandMenu.None;
        _flag.SetActive(false);
        _uiManager.Show(CommandMenu.None);
    }

    private bool TryGetLookPoint(out Vector3 point)
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundLayer))
        {
            point = hit.point;
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    private void UpdateFlag()
    {
        if (TryGetLookPoint(out Vector3 point))
        {
            point.y += 1.0f;
            _flag.transform.position = point;
        }
    }

}
