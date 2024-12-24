using UnityEngine;

[CreateAssetMenu(fileName = "DialogTriggerScriptableObject", menuName = "Scriptable Objects/DialogTriggerScriptableObject")]
public class DialogTriggerScriptableObject : TriggerScriptableObject
{
    public DialogScriptableObject dialog;
    public override void Run(DialogManager dialogManager)
    {
        dialogManager.Show(dialog);
    }
}
