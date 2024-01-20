using AE_FSM;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LogAction), menuName = "AE_FSM/StateActions/" + nameof(LogAction))]
public class LogAction : StateActionSO
{
    [TextArea]
    public string logMessage;

    public override void Enter(FSMController controller)
    {
        Debug.Log(controller.currentState.stateNodeData.name + "Enter" + logMessage);
    }
    public override void OnUpdate(FSMController controller)
    {
        Debug.Log("OnUpdate" + logMessage);
    }
    public override void Exit(FSMController controller)
    {
        Debug.Log("Exit" + logMessage);
    }
}
