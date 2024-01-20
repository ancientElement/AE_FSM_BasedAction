using AE_FSM;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SwitchAction), menuName = "AE_FSM/StateActions/" + nameof(SwitchAction))]
public class SwitchAction : StateActionSO
{
    public string StateName;

    public override void Enter(FSMController controller)
    {
        controller.SwitchState(StateName);
    }
}
