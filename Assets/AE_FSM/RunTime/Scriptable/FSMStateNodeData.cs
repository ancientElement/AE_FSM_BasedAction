using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AE_FSM
{
    [Serializable]
    public class StateActionSOS
    {
        public List<StateActionSO> EnterActions = new List<StateActionSO>();
        public List<StateActionSO> ExitActions = new List<StateActionSO>();
        public List<StateActionSO> OnUpdateActions = new List<StateActionSO>();
        public List<StateActionSO> FixUpdateActions = new List<StateActionSO>();
        public List<StateActionSO> LaterUpdateActions = new List<StateActionSO>();
    }

    [Serializable]
    public class FSMStateNodeData
    {
#if UNITY_EDITOR
        public Rect rect;
#endif

        /// <summary>
        /// 是否是默认
        /// </summary>
        public bool defualtState;

        /// <summary>
        /// 状态名
        /// </summary>
        public string name;

        ///// <summary>
        ///// 脚本的名称
        ///// </summary>
        //public string scriptName;

        //public MonoScript script;

        public List<FSMTranslationData> trasitions = new List<FSMTranslationData>();

        public StateActionSOS stateActions = new StateActionSOS();
    }
}
