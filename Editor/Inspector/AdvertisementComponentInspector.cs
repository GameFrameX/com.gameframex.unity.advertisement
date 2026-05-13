using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameFrameX.Advertisement.Runtime;
using GameFrameX.Editor;
using GameFrameX.Runtime;
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
        private SerializedProperty m_adUnitIdWebGLKuaiShou = null;
        private SerializedProperty m_debug = null;
        private SerializedProperty m_config = null;

        private GUIContent m_debugGUIContent = new GUIContent("是否是测试模式");
        private GUIContent m_adUnitIdAndroidGUIContent = new GUIContent("Android 广告位ID");
        private GUIContent m_adUnitIdiOSGUIContent = new GUIContent("IOS 广告位ID");
        private GUIContent m_adUnitIdWebGLGUIContent = new GUIContent("WebGL 广告位ID");
        private GUIContent m_adUnitIdWebGLWeChatGUIContent = new GUIContent("WebGL 微信 广告位ID");
        private GUIContent m_adUnitIdWebGLDouYinGUIContent = new GUIContent("WebGL 抖音 广告位ID");
        private GUIContent m_adUnitIdWebGLKuaiShouGUIContent = new GUIContent("WebGL 快手 广告位ID");

        private System.Type _selectedManagerType;
        private System.Type _configType;
        private FieldInfo[] _configFields;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode & Application.isPlaying);
            {
                EditorGUILayout.PropertyField(m_debug, m_debugGUIContent);

                DrawConfigFields();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("广告位ID (旧接口)", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_adUnitIdAndroid, m_adUnitIdAndroidGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdiOS, m_adUnitIdiOSGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdWebGL, m_adUnitIdWebGLGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdWebGLWeChat, m_adUnitIdWebGLWeChatGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdWebGLDouYin, m_adUnitIdWebGLDouYinGUIContent);
                EditorGUILayout.PropertyField(m_adUnitIdWebGLKuaiShou, m_adUnitIdWebGLKuaiShouGUIContent);
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void DrawConfigFields()
        {
            UpdateConfigType();

            if (_configType == null)
                return;

            var config = m_config.managedReferenceValue as AdvertisementConfig;
            if (config == null || config.GetType() != _configType)
            {
                config = (AdvertisementConfig)Activator.CreateInstance(_configType);
                m_config.managedReferenceValue = config;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(_configType.Name), EditorStyles.boldLabel);

            var isDebugProp = serializedObject.FindProperty($"{m_config.propertyPath}.isDebug");
            if (isDebugProp != null)
            {
                EditorGUILayout.PropertyField(isDebugProp, new GUIContent("是否是测试模式 (Config)"));
            }

            if (_configFields == null)
                return;

            foreach (var field in _configFields)
            {
                var prop = serializedObject.FindProperty($"{m_config.propertyPath}.{field.Name}");
                if (prop != null)
                {
                    EditorGUILayout.PropertyField(prop, new GUIContent(ObjectNames.NicifyVariableName(field.Name)));
                }
            }
        }

        private void UpdateConfigType()
        {
            var currentTypeName = ComponentType.stringValue;
            if (string.IsNullOrEmpty(currentTypeName))
            {
                _selectedManagerType = null;
                _configType = null;
                _configFields = null;
                return;
            }

            if (_selectedManagerType != null && _selectedManagerType.FullName == currentTypeName)
            {
                return;
            }

            _selectedManagerType = Utility.Assembly.GetType(currentTypeName);
            if (_selectedManagerType == null)
            {
                _configType = null;
                _configFields = null;
                return;
            }

            var attr = _selectedManagerType.GetCustomAttribute<AdvertisementConfigAttribute>();
            if (attr != null)
            {
                _configType = attr.ConfigType;
                _configFields = _configType.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => f.DeclaringType != typeof(AdvertisementConfig))
                    .ToArray();
            }
            else
            {
                _configType = null;
                _configFields = null;
            }
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
            m_adUnitIdWebGLKuaiShou = serializedObject.FindProperty("m_adUnitIdWebGLKuaiShou");
            m_debug = serializedObject.FindProperty("m_debug");
            m_config = serializedObject.FindProperty("m_config");
        }
    }
}
