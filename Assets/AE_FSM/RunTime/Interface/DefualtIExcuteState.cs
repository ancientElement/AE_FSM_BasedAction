namespace AE_FSM
{
    public class DefualtIExcuteState : IExcuteState
    {
        public void Enter(FSMStateNode node)
        {
            if (node.stateNodeData.stateActions == null) return;
            for (int i = 0; i < node.stateNodeData.stateActions.EnterActions.Count; i++)
            {
                node.stateNodeData.stateActions.EnterActions[i]?.Enter(node.controller);
            }
        }
        public void Update(FSMStateNode node)
        {
            if (node.stateNodeData.stateActions == null) return;
            for (int i = 0; i < node.stateNodeData.stateActions.OnUpdateActions.Count; i++)
            {
                node.stateNodeData.stateActions.EnterActions[i]?.OnUpdate(node.controller);
            }
        }
        public void LaterUpdate(FSMStateNode node)
        {
            if (node.stateNodeData.stateActions == null) return;
            for (int i = 0; i < node.stateNodeData.stateActions.OnUpdateActions.Count; i++)
            {
                node.stateNodeData.stateActions.EnterActions[i]?.LaterUpdate(node.controller);
            }

        }
        public void FixUpdate(FSMStateNode node)
        {
            if (node.stateNodeData.stateActions == null) return;
            for (int i = 0; i < node.stateNodeData.stateActions.OnUpdateActions.Count; i++)
            {
                node.stateNodeData.stateActions.EnterActions[i]?.FixUpdate(node.controller);
            }
        }
        public void Exit(FSMStateNode node)
        {
            if (node.stateNodeData.stateActions == null) return;
            for (int i = 0; i < node.stateNodeData.stateActions.OnUpdateActions.Count; i++)
            {
                node.stateNodeData.stateActions.EnterActions[i]?.Exit(node.controller);
            }
        }
    }
}