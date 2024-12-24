using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileScriptableObject", menuName = "Scriptable Objects/TileScriptableObject")]
public class TileScriptableObject : ScriptableObject
{
    public string displayName;
    public SECAMColorPalette.Enum color;
    public Sprite art;

    [System.Serializable]
    public class ActionTarget
    {
        public long cost;
        public List<TileScriptableObject> tiles;
    }

    public ActionTarget build;
    public ActionTarget destroy;

    public long generateMoney = 0;
    public int generateMoneyT = 1;

    public List<TriggerScriptableObject> OnStartTriggers;

}
