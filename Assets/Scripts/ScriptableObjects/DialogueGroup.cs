using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Group", menuName = "Dialogue Group", order = 1)]
public class DialogueGroup : ScriptableObject
{
    [Serializable]
    public class DialogueUnit
    {
        public string name;

        [TextArea]
        public List<string> dialogues = new List<string>();
    };

    public List<DialogueUnit> units = new List<DialogueUnit>();

    public List<string> Get(int index)
    {
        if(index < 0 && index >= units.Count)
            return null;
        return units[index].dialogues;
    }

    public List<string> Get(string tag)
    {
        foreach(DialogueUnit unit in units)
            if(unit.name.Equals(tag))
                return unit.dialogues;
        return null;
    }
}
