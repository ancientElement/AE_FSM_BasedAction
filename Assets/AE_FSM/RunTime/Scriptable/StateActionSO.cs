using AE_FSM;
using UnityEngine;

public class StateActionSO : ScriptableObject, IFSMState
{
    [TextArea]
    public string Description;

    public virtual void Enter(FSMController controller)
    {
    }

    public virtual void Exit(FSMController controller)
    {
    }

    public virtual void FixUpdate(FSMController controller)
    {
    }

    public virtual void OnUpdate(FSMController controller)
    {
    }

    public virtual void LaterUpdate(FSMController controller)
    {
    }
}
