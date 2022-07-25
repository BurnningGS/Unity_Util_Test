using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TD
{
#if UNITY_EDITOR

    [CustomEditor(typeof(TD_InfinityScroll))]
    [ExecuteInEditMode]
    public class TD_CE_InfinityScroll : Editor
    {
        private TD_InfinityScroll _target;

        private void OnEnable()
        {
            _target = (TD_InfinityScroll)target;
        }

        public override void OnInspectorGUI()
        {
            DrawButton();

            GUILayout.Space(30);
            GUILayout.Label("=======================================");
            GUILayout.Space(30);

            base.OnInspectorGUI();
        }

        public void DrawButton()
        {
            GUILayout.Label("=======================================");
            GUILayout.Label(" [ Infinity Scroll Create �� Setting ] ");
            GUILayout.Space(20);
            if (GUILayout.Button(" Create (����) ")) _target.OnClick_CreateInfinityScroll();
            if (GUILayout.Button(" Reset (�ʱ�ȭ) ")) _target.OnClick_CreateInfinityScroll();
        }
    }
#endif
}