using UnityEngine;

public enum CommandMenu
{
    Root,
    Move,
    Formation,
    Direction,
    None
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
    [SerializeField] private GameObject _camera;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _flag;
    [SerializeField] private CommandUIManager _uiManager;

    private CommandMenu _commandMenu = CommandMenu.None;

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
        switch (_commandMenu)
        {
            case CommandMenu.None:
                UpdateNone();
                break;

            case CommandMenu.Root:
                SelectMenu();
                break;

            default:
                ExecuteCommand();
                break;
        }

        UpdateFlag();
    }

    private void UpdateNone()
    {
        if (_controls.Command.Tab.WasPressedThisFrame())
        {
            Debug.Log($"Tab Pressed");
            _commandMenu = CommandMenu.Root;
            _uiManager.Show(CommandMenu.Root);
        }
    }

    private void SelectMenu()
    {
        if (_controls.Command.F1.WasPressedThisFrame())
        {
            Debug.Log($"F1 Pressed");
            _commandMenu = CommandMenu.Move;
            _flag.SetActive(true);
            _uiManager.Show(_commandMenu);
        }
        else if (_controls.Command.F2.WasPressedThisFrame())
        {
            Debug.Log($"F2 Pressed");
            _commandMenu = CommandMenu.Formation;
            _flag.SetActive(true);
            _uiManager.Show(_commandMenu);
        }
        else if (_controls.Command.F3.WasPressedThisFrame())
        {
            Debug.Log($"F3 Pressed");
            _commandMenu = CommandMenu.Direction;
            _flag.SetActive(true);
            _uiManager.Show(_commandMenu);
        }
        else if (_controls.Command.Esc.WasPressedThisFrame())
        {
            Debug.Log($"Esc Pressed");
            ResetMenu();
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
                ExecuteFormationCommand();
                break;

            case CommandMenu.Direction:
                //ExecuteDirectionCommand();
                break;
        }
    }

    private void ExecuteMoveCommand()
    {
        if (_controls.Command.F1.WasPressedThisFrame())
        {
            // 이동
            Debug.Log($"F1-F1 Pressed");
            if (TryGetLookPoint(out Vector3 point))
            {
                point.y += 1.0f;
                _troopControllers[0].Move(point);
            }
            ResetMenu();
        }
        else if (_controls.Command.F2.WasPressedThisFrame())
        {
            // 따라오기
            Debug.Log($"F1-F2 Pressed");
            _troopControllers[0].Follow();
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
            BackToRoot();
        }
    }

    private void ExecuteFormationCommand()
    {
        if (_controls.Command.F1.WasPressedThisFrame())
        {
            // 선형
            Debug.Log($"F2-F1 Pressed");
            _troopControllers[0].ChangeFormation(FormationType.LineFormation);
            ResetMenu();
        }
        else if (_controls.Command.F2.WasPressedThisFrame())
        {
            // 방패벽
            Debug.Log($"F2-F2 Pressed");
            ResetMenu();
        }
        else if (_controls.Command.F3.WasPressedThisFrame())
        {
            // 산개
            Debug.Log($"F2-F3 Pressed");
            ResetMenu();
        }
        else if (_controls.Command.F4.WasPressedThisFrame())
        {
            // 사각
            Debug.Log($"F2-F4 Pressed");
            _troopControllers[0].ChangeFormation(FormationType.SquareFormation);
            ResetMenu();
        }
        else if (_controls.Command.Esc.WasPressedThisFrame())
        {
            Debug.Log($"F2-Esc Pressed");
            BackToRoot();
        }
    }

    private void ResetMenu()
    {
        _commandMenu = CommandMenu.None;
        _flag.SetActive(false);
        _uiManager.HideAll();
    }

    private void BackToRoot()
    {
        _commandMenu = CommandMenu.Root;
        _flag.SetActive(false);
        _uiManager.Show(CommandMenu.Root);
    }

    private bool TryGetLookPoint(out Vector3 point)
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, _groundLayer))
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
