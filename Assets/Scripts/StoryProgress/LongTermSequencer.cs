using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongTermSequencer : Sequencer
{
    protected override IEnumerator Sequence()
    {
        yield break;
    }

    public override void ManagedUpdate()
    {
        base.ManagedUpdate();
    }
}
