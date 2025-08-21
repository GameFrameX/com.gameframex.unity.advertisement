//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFrameX.Advertisement.Runtime;
using GameFrameX.Editor;
using UnityEditor;
using UnityEngine;

namespace GameFrameX.Advertisement.Editor
{
    [CustomEditor(typeof(AdvertisementComponent))]
    internal sealed class AdvertisementComponentInspector : ComponentTypeComponentInspector
    {
        private SerializedProperty m_adUnitIdAndroid = null;
        private SerializedProperty m_adUnitIdiOS = null;
        private SerializedProperty m_adUnitIdWebGL = null;
        private SerializedProperty m_adUnitIdWebGLWeChat = null;
        private SerializedProperty m_adUnitIdWebGLDouYin = null;
        private SerializedProperty m_debug = null;
        private GUIContent m_debugGUIContent = new GUIContent("是否是测试模式");
        private GUIContent m_adUnitIdAndroidGUIContent = new GUIContent("Android 广告位ID");
        private GUIContent m_adUnitIdiOSGUIContent = new GUIContent("IOS 广告位ID");
        private GUIContent m_adUnitIdWebGLGUIContent = new GUIContent("WebGL 广告位ID");
        private GUIContent m_adUnitIdWebGLWeChatGUIContent = new GUIContent("WebGL 微信 广告位ID");
        private GUIContent m_adUnitIdWebGLDouYinGUIContent = new GUIContent("WebGL 抖音 广告位ID");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode & Application.isPlaying);
            {
                EditorGUILayout.PropertyField(m_debug, m_debugGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdAndroid, m_adUnitIdAndroidGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdiOS, m_adUnitIdiOSGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdWebGL, m_adUnitIdWebGLGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdWebGLWeChat, m_adUnitIdWebGLWeChatGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdWebGLDouYin, m_adUnitIdWebGLDouYinGUIContent);
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void RefreshTypeNames()
        {
            RefreshComponentTypeNames(typeof(IAdvertisementManager));
        }

        protected override void Enable()
        {
            m_adUnitIdAndroid = serializedObject.FindProperty("m_adUnitIdAndroid");
            m_adUnitIdiOS = serializedObject.FindProperty("m_adUnitIdiOS");
            m_adUnitIdWebGL = serializedObject.FindProperty("m_adUnitIdWebGL");
            m_adUnitIdWebGLWeChat = serializedObject.FindProperty("m_adUnitIdWebGLWeChat");
            m_adUnitIdWebGLDouYin = serializedObject.FindProperty("m_adUnitIdWebGLDouYin");
            m_debug = serializedObject.FindProperty("m_debug");
        }
    }
}