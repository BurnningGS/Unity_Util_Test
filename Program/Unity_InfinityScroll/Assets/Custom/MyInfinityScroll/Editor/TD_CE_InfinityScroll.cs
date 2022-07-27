using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TD
{
#if UNITY_EDITOR

    [CustomEditor(typeof(TD_InfinityScroll_Test))]
    [ExecuteInEditMode]
    public class TD_CE_InfinityScroll : Editor
    {
        private TD_InfinityScroll_Test _target;

        private void OnEnable()
        {
            _target = (TD_InfinityScroll_Test)target;
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
            GUILayout.Label(" [ Infinity Scroll Create & Setting ] ");

            GUILayout.Space(20);
            if (GUILayout.Button(" 1) Reset ( 초기화 ) ")) 
                _target.Editor_ResetInfinityScroll();

            GUILayout.Space(20);
            if (GUILayout.Button(" 2) Create ( 생성 ) "))
                _target.Editor_CreateInfinityScroll();

            GUILayout.Space(20);
            if (GUILayout.Button(" 3) Set ( 셋팅 ) "))
                _target.Editor_SetInfinityScroll();

        }



    }



#endif
}