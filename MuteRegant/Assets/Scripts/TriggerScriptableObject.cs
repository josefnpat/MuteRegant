using UnityEngine;

public abstract class TriggerScriptableObject : ScriptableObject
{
    public virtual void Run(DialogManager dialogManager)
    {
        Debug.Log("TriggerScriptableObject");
    }
}
