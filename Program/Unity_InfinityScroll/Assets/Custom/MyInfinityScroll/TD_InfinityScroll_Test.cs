using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace TD
{
    


    [Serializable]
    public class TD_InfinityScroll_Test : UIBehaviour
    {
        //============================================================================================
        // Inner Class


        /// <summary>
        /// Infinity Scroll Item Event Class(내부 클래스) 형식
        /// </summary>
        [Serializable]
        public class OnItemPositionChangeEvent : UnityEngine.Events.UnityEvent<int, GameObject> { }



        //============================================================================================
        // 변수


        [Header("======================= Infinity Scroll ==============================")]
        [Space(30)]
        
        [Header("1. Scroll Setting")]
        [Space(15)]
        public eIFS_ScrollType ScrollType = eIFS_ScrollType.Max;
        public eIFS_ST_Axis StandardAxis = eIFS_ST_Axis.Max;
        public eIFS_StartCorner StartCorner = eIFS_StartCorner.Max;
        public eIFS_ChildAligent ChildAligent = eIFS_ChildAligent.Max;
        [Space(15)]
        public Vector2 CellSize = Vector2.one;
        public Vector2 Spacing = Vector2.zero;
        public Vector4 Padding = Vector2.zero;
        [Space(15)]
        public Vector2 ST_ItemCount = Vector2.zero;
        public Vector2 ExtraItemCount = Vector2.one;

        [Space(30)]
        [Header("2. Calculate Data")]
        [Space(15)]
        public int CurrentIndex;
        public int MaxItemCount;
        public int TotalItemCount;


        [Space(30)]
        [Header("3. Target Component")]
        [Space(15)]

        public RectTransform Target_BG;
        public RectTransform Target_ScrollRectRt;
        public ScrollRect Target_ScrollRect;
        public RectTransform Content;


        [Space(30)]
        [Header("4. Inner Data")]
        [Space(15)]
        public LinkedList<RectTransform> ItemList = new LinkedList<RectTransform>();


        [Space(30)]
        [Header("5. Inner Event")]
        [Space(15)]
        public OnItemPositionChangeEvent onUpdateItemEvents = new OnItemPositionChangeEvent();



        //============================================================================================
        // 함수

        public void OnClick_CreateInfinityScroll()
        {

            Target_BG = gameObject.GetComponent<RectTransform>();
            var crBG = Target_BG.gameObject.AddComponent<CanvasRenderer>();
            crBG.cullTransparentMesh = true;


            var sr = Instantiate(new GameObject(), Target_BG);
            sr.name = "ScrollRect";
            Target_ScrollRect = sr.AddComponent<ScrollRect>();
            Target_ScrollRectRt = sr.GetComponent<RectTransform>();



            var ct = Instantiate(new GameObject(), Target_ScrollRectRt);
            Content = ct.GetComponent<RectTransform>();



        }

        public void OnClick_ResetInfinityScroll()
        {
            
        }
    }
}