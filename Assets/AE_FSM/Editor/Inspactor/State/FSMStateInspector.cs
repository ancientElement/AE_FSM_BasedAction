using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AE_FSM
{
    public class ActionStateReorderableList
    {
        private ReorderableList reoRderableList;
        private RunTimeFSMController m_runTimeFSMController;
        private List<StateActionSO> m_stateActionSOs;
        private string m_title;

        public ActionStateReorderableList(List<StateActionSO> stateActionSOs, RunTimeFSMController runTimeFSMController, string title)
        {
            m_stateActionSOs = stateActionSOs;
            m_title = title;
            m_runTimeFSMController = runTimeFSMController;
            reoRderableList = new ReorderableList(stateActionSOs, typeof(StateActionSO), true, true, true, true);
            reoRderableList.drawHeaderCallback += DrawHeaderCallback;
            reoRderableList.onAddCallback += OnAddCallback;
            reoRderableList.onRemoveCallback += OnRemoveCallback;
            reoRderableList.drawElementCallback += DrawElementCallback;
        }

        public void DoLayoutList()
        {
            reoRderableList.DoLayoutList();
        }

        public void UpdateList(List<StateActionSO> list)
        {
            m_stateActionSOs = list;
            reoRderableList.list = list;
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            float width = rect.width / 10;
            Rect lableRect = new Rect(rect.x, rect.y, width * 2, rect.height);
            Rect objectFiledRect = new Rect(rect.x + width * 2f, rect.y, width * 0.9f, rect.height);
            Rect descriptionRect = new Rect(rect.x + width * 2.9f, rect.y, width * 7.1f, rect.height);

            StateActionSO stateActionSO = m_stateActionSOs[index];
            string name;
            string description;

            if (stateActionSO == null)
            {
                name = "文件丢失!!";
                description = string.Empty;
            }
            else
            {
                name = m_stateActionSOs[index]?.name;
                description = m_stateActionSOs[index]?.name;
            }

            GUI.Label(lableRect, name);
            StateActionSO temp = (StateActionSO)EditorGUI.ObjectField(objectFiledRect, stateActionSO, typeof(StateActionSO), false);
            if (temp != m_stateActionSOs[index])
            {
                m_stateActionSOs[index] = temp;
                Save();
            }
            GUI.Label(descriptionRect, description);
        }

        private void Save()
        {
            EditorUtility.SetDirty(m_runTimeFSMController);
            AssetDatabase.SaveAssetIfDirty(m_runTimeFSMController);
        }

        private void OnRemoveCallback(ReorderableList list)
        {
            m_stateActionSOs.RemoveAt(list.index);
            Save();
        }

        private void OnAddCallback(ReorderableList list)
        {
            m_stateActionSOs.Add(null);
            Save();
        }

        private void DrawHeaderCallback(Rect rect)
        {
            GUI.Label(rect, m_title);
        }
    }

    [CustomEditor(typeof(FSMStateInspectorHelper))]
    public class FSMStateInspector : Editor
    {
        private ReorderableList paramsReorderableList;

        private ActionStateReorderableList enterEventreoRderableList;
        private ActionStateReorderableList exitEventreoRderableList;
        private ActionStateReorderableList onUpdateEventreoRderableList;
        private ActionStateReorderableList fixedUpdateEventreoRderableList;
        private ActionStateReorderableList laterUpdateEventreoRderableList;

        private int clickCount;
        private int preListIndex;

        private void OnEnable()
        {
            FSMStateInspectorHelper helper = target as FSMStateInspectorHelper;
            if (helper == null) return;

            paramsReorderableList = new ReorderableList(helper.stateNodeData.trasitions, typeof(FSMTranslationData), true, true, false, true);
            paramsReorderableList.onMouseUpCallback += MouseDownParamter;
            paramsReorderableList.drawHeaderCallback += DrawHeaderCallbackParamter;
            paramsReorderableList.onRemoveCallback += RemoveParamter;
            paramsReorderableList.drawElementCallback += DrawOneParamter;

            enterEventreoRderableList = new ActionStateReorderableList(helper.stateNodeData.stateActions.EnterActions, helper.contorller, "Enter");
            exitEventreoRderableList = new ActionStateReorderableList(helper.stateNodeData.stateActions.EnterActions, helper.contorller, "Exit");
            onUpdateEventreoRderableList = new ActionStateReorderableList(helper.stateNodeData.stateActions.EnterActions, helper.contorller, "Update");
            fixedUpdateEventreoRderableList = new ActionStateReorderableList(helper.stateNodeData.stateActions.EnterActions, helper.contorller, "FixedUpdate");
            laterUpdateEventreoRderableList = new ActionStateReorderableList(helper.stateNodeData.stateActions.EnterActions, helper.contorller, "LaterUpdate");
        }

        public override void OnInspectorGUI()
        {
            FSMStateInspectorHelper helper = target as FSMStateInspectorHelper;
            if (helper == null) return;
            paramsReorderableList.list = helper.stateNodeData.trasitions;
            paramsReorderableList.DoLayoutList();

            enterEventreoRderableList.UpdateList(helper.stateNodeData.stateActions.EnterActions);
            enterEventreoRderableList.DoLayoutList();

            exitEventreoRderableList.UpdateList(helper.stateNodeData.stateActions.ExitActions);
            exitEventreoRderableList.DoLayoutList();

            onUpdateEventreoRderableList.UpdateList(helper.stateNodeData.stateActions.OnUpdateActions);
            onUpdateEventreoRderableList.DoLayoutList();

            fixedUpdateEventreoRderableList.UpdateList(helper.stateNodeData.stateActions.FixUpdateActions);
            fixedUpdateEventreoRderableList.DoLayoutList();

            laterUpdateEventreoRderableList.UpdateList(helper.stateNodeData.stateActions.LaterUpdateActions);
            laterUpdateEventreoRderableList.DoLayoutList();
        }

        protected override void OnHeaderGUI()
        {
            FSMStateInspectorHelper helper = target as FSMStateInspectorHelper;
            if (helper == null) return;

            bool disable = EditorApplication.isPlaying || helper.stateNodeData.name == FSMConst.enterState || helper.stateNodeData.name == FSMConst.anyState;

            string name = null;
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(EditorGUIUtility.IconContent("icons/processed/unityeditor/animations/animatorstate icon.asset"), GUILayout.Width(30), GUILayout.Height(30));
                EditorGUILayout.LabelField("Name", GUILayout.Width(80));

                EditorGUI.BeginDisabledGroup(disable);
                name = EditorGUILayout.DelayedTextField(helper.stateNodeData.name);

                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                FSMStateNodeFactory.ReNameFSMNode(helper.contorller, helper.stateNodeData, name);
            }

            var rect = EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space();
            Handles.color = Color.black;
            Handles.DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y));
            EditorGUILayout.Space();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 选择过渡
        /// </summary>
        /// <param name="list"></param>
        private void SelectCallback(ReorderableList list)
        {
            FSMStateInspectorHelper helper = target as FSMStateInspectorHelper;
            if (helper == null) return;
            FSMTranslationInspectorHelper.Instance.Inspector(helper.contorller, helper.stateNodeData.trasitions[list.index]);
            FSMEditorWindow.GetWindow<FSMEditorWindow>().Repaint();
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="index"></param>
        /// <param name="isActive"></param>
        /// <param name="isFocused"></param>
        private void DrawOneParamter(Rect rect, int index, bool isActive, bool isFocused)
        {
            FSMStateInspectorHelper helper = target as FSMStateInspectorHelper;
            if (helper == null) return;
            EditorGUI.LabelField(rect, helper.stateNodeData.trasitions[index].fromState + "--->" + helper.stateNodeData.trasitions[index].toState);
            Repaint();
        }

        /// <summary>
        /// 移除过渡
        /// </summary>
        /// <param name="list"></param>
        private void RemoveParamter(ReorderableList list)
        {
            //TODO
            FSMStateInspectorHelper helper = target as FSMStateInspectorHelper;
            if (helper == null) return;
            FSMTranslationFactory.DeleteTransition(helper.contorller, helper.stateNodeData.trasitions[list.index]);
        }

        /// <summary>
        /// 过渡鼠标点击
        /// </summary>
        /// <param name="list"></param>
        private void MouseDownParamter(ReorderableList list)
        {
            if (Event.current.button == 0)
            {
                if (preListIndex == list.index)
                {
                    clickCount++;
                }
                else if (preListIndex != list.index)
                {
                    clickCount = 0;
                }

                if (clickCount == 2)
                {
                    clickCount = 0;
                    // 双击事件处理逻辑
                    SelectCallback(list);
                }
                preListIndex = list.index;
            }
        }

        private void DrawHeaderCallbackParamter(Rect rect)
        {
            GUI.Label(rect, "过渡");
        }
    }
}