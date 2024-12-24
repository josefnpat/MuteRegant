using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _panel;

    [SerializeField]
    private TMP_Text _dialogText;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private DialogScriptableObject _startDialog;

    private TileManager _tileManager;
    private DialogScriptableObject _currentDialog;

    private List<DialogScriptableObject> _shownDialogs;

    void Start()
    {
        _tileManager = FindAnyObjectByType<TileManager>();
        _button.onClick.AddListener(OnButtonClick);
        _shownDialogs = new List<DialogScriptableObject>();
        Show(_startDialog);
    }

    private void OnButtonClick()
    {
        if (_currentDialog.next)
        {
            Show(_currentDialog.next);
        }
        else
        {
            Hide();
        }
    }

    public void Show(DialogScriptableObject dialog)
    {
        if (dialog.onlyShowOnce)
        {
            if (_shownDialogs.Contains(dialog))
            {
                if (dialog.next)
                {
                    Show(dialog.next);
                }
                return;
            }
            else
            {
                _shownDialogs.Add(dialog);
            }
        }

        _currentDialog = dialog;
        _dialogText.text = dialog.text;
        _panel.SetActive(true);
        _tileManager.Hide();
    }

    public void Hide()
    {
        _panel.SetActive(false);
        _tileManager.Show();
    }

    public bool IsActive()
    {
        return _panel.activeSelf;
    }

}
