using UnityEngine;

public class CommandUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _commandMenuUI;
    [SerializeField] private GameObject _moveCommandUI;
    [SerializeField] private GameObject _formationCommandUI;
    [SerializeField] private GameObject _directionCommandUI;

    private void Awake()
    {
        HideAll();
    }

    public void Show(CommandMenu menu)
    {
        HideAll();

        switch (menu)
        {
            case CommandMenu.Root:
                _commandMenuUI.SetActive(true);
                break;

            case CommandMenu.Move:
                _moveCommandUI.SetActive(true);
                break;

            case CommandMenu.Formation:
                _formationCommandUI.SetActive(true);
                break;

            case CommandMenu.Direction:
                _directionCommandUI.SetActive(true);
                break;
        }
    }

    public void HideAll()
    {
        _commandMenuUI.SetActive(false);
        _moveCommandUI.SetActive(false);
        _formationCommandUI.SetActive(false);
        _directionCommandUI.SetActive(false);
    }

}
