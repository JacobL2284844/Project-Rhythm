//jacob lueke: script defines class for the npc state machine
using UnityEngine;
public abstract class NPCBaseState
{
    public abstract void EnterState(NPCStateManager npcContext);
    public abstract void UpdateState(NPCStateManager npcContext);
}
