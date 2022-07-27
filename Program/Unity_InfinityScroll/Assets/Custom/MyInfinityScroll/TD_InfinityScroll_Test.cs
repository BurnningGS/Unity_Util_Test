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


        [Space(20)]
        [Header("  [ 0. CustomEditor ]")]
        [Space(15)]
        public bool EditorUpdateMode = false;

        [Space(30)]
        [Header("==============================")]
        [Header("  [ 1. Scroll Setting ]  ")]
        [Space(15)]
        public eIFS_ScrollType      ScrollType = eIFS_ScrollType.Max;
        public eIFS_ST_Axis         StandardAxis = eIFS_ST_Axis.Max;
        public eIFS_StartCorner     StartCorner = eIFS_StartCorner.Max;
        public eIFS_ChildAligent    ChildAligent = eIFS_ChildAligent.Max;
        [Space(15)]
        public Vector2 CellSize = Vector2.one;
        public Vector2 Spacing = Vector2.zero;
        public Vector4 Padding = Vector2.zero;
        [Space(15)]
        public Vector2 ST_ItemCount = Vector2.zero;
        public Vector2 ExtraItemCount = Vector2.one;

        [Space(30)]
        [Header("==============================")]
        [Header("  [ 2. Calculate Data ]  ")]
        [Space(15)]
        public int CurrentIndex;
        public int MaxItemCount;
        public int TotalItemCount;


        [Space(30)]
        [Header("==============================")]
        [Header("  [ 3. Target Component ]  ")]
        [Space(15)]

        public RectTransform Target_BG;
        public RectTransform Target_ScrollRectRt;
        public ScrollRect Target_ScrollRect;
        public RectTransform Content;


        [Space(30)]
        [Header("==============================")]
        [Header("  [ 4. Inner Data ]  ")]
        [Space(15)]
        public LinkedList<RectTransform> ItemList = new LinkedList<RectTransform>();


        [Space(30)]
        [Header("==============================")]
        [Header("  [ 5. Inner Event ]  ")]
        [Space(15)]
        public OnItemPositionChangeEvent onUpdateItemEvents = new OnItemPositionChangeEvent();



        //============================================================================================
        // 함수

        public void Editor_ResetInfinityScroll()
        {
            if (gameObject.GetComponent<RectTransform>().childCount <= 0)
                return;

            var sr = gameObject.GetComponent<RectTransform>().GetChild(0).gameObject;
            DestroyImmediate(sr.GetComponent<ScrollRect>());
            DestroyImmediate(sr.GetComponent<Mask>());
            if (null != sr.GetComponent<Image>())
                DestroyImmediate(sr.GetComponent<Image>());
            DestroyImmediate(sr.GetComponent<CanvasRenderer>());
            DestroyImmediate(sr);


            onUpdateItemEvents = new OnItemPositionChangeEvent();
            ItemList.Clear();

            Target_BG = null;
            Target_ScrollRectRt = null;
            Target_ScrollRect = null;
            Content = null;

        }

        public void Editor_CreateInfinityScroll()
        {
            Target_BG = gameObject.GetComponent<RectTransform>();
            Target_BG.localScale = Vector3.one;

            if (null == Target_BG.GetComponent<CanvasRenderer>())
            {
                var crBG = Target_BG.gameObject.AddComponent<CanvasRenderer>();
                crBG.cullTransparentMesh = true;
            }


            var sr = new GameObject("ScrollRect");
            sr.transform.SetParent(Target_BG);
            Target_ScrollRect = sr.AddComponent<ScrollRect>();
            Target_ScrollRectRt = sr.GetComponent<RectTransform>();
            Target_ScrollRectRt.localPosition = Vector3.zero;
            Target_ScrollRectRt.localScale = Vector3.one;

            if (null == sr.GetComponent<CanvasRenderer>())
            {
                var crSr = sr.AddComponent<CanvasRenderer>();
                crSr.cullTransparentMesh = true;
            }

            if (null == sr.GetComponent<Mask>())
            {
                var mask = sr.AddComponent<Mask>();
                mask.showMaskGraphic = true;
            }

            Target_ScrollRectRt.SetAnchor(eAnchorOrPivotType.StretchAll);
            Target_ScrollRectRt.sizeDelta = new Vector2(-20f, -20f);

            var ct = new GameObject("Content");
            ct.transform.SetParent(Target_ScrollRectRt);

            if (null == ct.GetComponent<CanvasRenderer>())
            {
                var crCt = ct.AddComponent<CanvasRenderer>();
                crCt.cullTransparentMesh = true;
            }

            Content = ct.AddComponent<RectTransform>();
            Content.localPosition = Vector3.zero;
            Content.localScale = Vector3.one;
            Content.SetAnchor(eAnchorOrPivotType.HorStretchTop);
            Content.sizeDelta = new Vector2(0f,500f);
            Content.anchoredPosition = Vector2.zero;
            
        }

        public void Editor_SetInfinityScroll()
        {

        }
    }
}