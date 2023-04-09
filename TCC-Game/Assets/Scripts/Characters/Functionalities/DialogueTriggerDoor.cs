using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerDoor : DialogueTrigger
{
    [field: SerializeField] public Door Door { get; set; }

    public override void Dialogue()
    {
        base.Dialogue();
        if (!_canDialogue)
        {
            Door.Open();
        }
    }

}
