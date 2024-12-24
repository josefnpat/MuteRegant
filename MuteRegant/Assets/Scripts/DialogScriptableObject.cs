using UnityEngine;

[CreateAssetMenu(fileName = "DialogScriptableObject", menuName = "Scriptable Objects/DialogScriptableObject")]
public class DialogScriptableObject : ScriptableObject
{
    [TextArea(15, 20)]
    public string text;
    public DialogScriptableObject next;
    public bool onlyShowOnce;
}
