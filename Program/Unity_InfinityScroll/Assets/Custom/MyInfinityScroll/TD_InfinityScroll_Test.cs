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
        /// <summary>
        /// 스크롤 타입 입니다.
        /// </summary>
        public eIFS_ScrollType      ScrollType = eIFS_ScrollType.Max;
        /// <summary>
        /// 스크롤의 기준 축 입니다.
        /// </summary>
        public eIFS_ST_Axis         StandardAxis = eIFS_ST_Axis.Max;
        /// <summary>
        /// 스크롤의 아이템의 시작 위치 입니다.
        /// </summary>
        public eIFS_StartCorner     StartCorner = eIFS_StartCorner.Max;
        /// <summary>
        /// 스크롤의 자식 정렬타입 입니다.
        /// </summary>
        public eIFS_ChildAligent    ChildAligent = eIFS_ChildAligent.Max;
        
        [Space(15)]
        /// <summary>
        /// 아이템의 크기 입니다.
        /// </summary>
        public Vector2 CellSize = Vector2.one;
        /// <summary>
        /// 아이템 사이의 간격 입니다.
        /// </summary>
        public Vector2 Spacing = Vector2.zero;
        /// <summary>
        /// 여백 입니다.
        /// </summary>
        public Vector4 Padding = Vector2.zero;
        
        [Space(15)]
        /// <summary>
        /// 아이템의 갯수 입니다.
        /// </summary>
        public Vector2 ST_ItemCount = Vector2.zero;
        /// <summary>
        /// 여분 아이템의 갯수입니다.
        /// </summary>
        public Vector2 Extra_ItemCount = Vector2.one;
        
        [Space(15)]
        /// <summary>
        /// 자식 아이템 프리팹 입니다.
        /// </summary>
        public GameObject ChildItemPrefab = null;


        [Space(30)]
        [Header("==============================")]
        [Header("  [ 2. Calculate Data ]  ")]
        [Space(15)]
        /// <summary>
        /// 현재 아이템 인덱스 입니다. (기준축 기준)
        /// </summary>
        public int CurrentIndex;
        /// <summary>
        /// 최대 아이템 갯수 입니다. (가로 x 세로)
        /// </summary>
        public int MaxItemCount;
        /// <summary>
        /// 모든 아이템 갯수입니다. ( (가로 x 세로) + (여분아이템 가로 x여분아이템 세로) )
        /// </summary>
        public int TotalItemCount;


        [Space(30)]
        [Header("==============================")]
        [Header("  [ 3. Target Component ]  ")]
        [Space(15)]
        /// <summary>
        /// 인피니티 스크롤의 RectTransfrom 입니다.
        /// </summary>
        public RectTransform Target_BG;
        /// <summary>
        /// Scroll Rect 의 RectTransform 입니다.
        /// </summary>
        public RectTransform Target_ScrollRectRt;
        /// <summary>
        /// Scroll Rect 입니다.
        /// </summary>
        public ScrollRect Target_ScrollRect;
        /// <summary>
        /// 자식 아이템의 Root 오브젝트 RectTransform 입니다. 
        /// </summary>
        public RectTransform Content;


        [Space(30)]
        [Header("==============================")]
        [Header("  [ 4. Inner Data ]  ")]
        [Space(15)]
        /// <summary>
        /// 
        /// </summary>
        public LinkedList<RectTransform> ItemList = new LinkedList<RectTransform>();


        [Space(30)]
        [Header("==============================")]
        [Header("  [ 5. Inner Event ]  ")]
        [Space(15)]
        public OnItemPositionChangeEvent onUpdateItemEvents = new OnItemPositionChangeEvent();




        //============================================================================================
        // 함수

        protected override void Start()
        {
            _initialize_Scroll();
           
        }

        private void _initialize_Scroll()
        {
            //-----------------------------------------------------------------------------------------------
            // 0. Calculate
            //
            MaxItemCount = (int)(ST_ItemCount.x * ST_ItemCount.y);
            TotalItemCount = MaxItemCount + (int)(Extra_ItemCount.x * Extra_ItemCount.y);

            //-----------------------------------------------------------------------------------------------
            // 1. Find Controllers (콘트롤러 검색)
            //
            var controllers = GetComponents<MonoBehaviour>().Where(item => item is TDI_InfinityScroll).Select(item => item as TDI_InfinityScroll).ToList();

            //-----------------------------------------------------------------------------------------------
            // 2. Create items (아이템 생성)
            //
            Target_ScrollRect.horizontal = (StandardAxis == eIFS_ST_Axis.Horizontal);
            Target_ScrollRect.vertical = (StandardAxis == eIFS_ST_Axis.Vertical);
            Target_ScrollRect.content = Content;

            ChildItemPrefab.gameObject.SetActive(false); // 생성 아이템 원본 꺼줌.

            // 아이템 생성.

            for (int Y = 0; Y < (ST_ItemCount.x + Extra_ItemCount.x); ++Y)
            {
                for (int X = 0; X < (ST_ItemCount.y + Extra_ItemCount.y); ++X)
                {
                    var item = Instantiate(ChildItemPrefab) as GameObject;
                    var rt = item.GetComponent<RectTransform>();
                    item.name = $"{Y}_{X}"; // 이름 설정
                    rt.SetParent(Content, false); // 부모설정
                    rt.sizeDelta = CellSize; // 크기 지정

                    // 아이템 위치 지정. 
                    var pos = new Vector2(CellSize.x,CellSize.y);
                    
                    //rt.anchoredPosition = (StandardAxis == eIFS_ST_Axis.Vertical) ? new Vector2(0, -CellSize * i) : new Vector2(itemScale * i, 0);
                }
            }


            //for (int i = 0; i < CreateData.InstantateItemCount; i++)
            //{
            //    var item = Instantiate(CreateData.CreateItemPrefab) as RectTransform;
            //    item.SetParent(transform, false); // 부모설정
            //    item.name = i.ToString(); // 이름 설정
            //    item.anchoredPosition = (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -itemScale * i) : new Vector2(itemScale * i, 0);
            //    Data.ItemList.AddLast(item);// 리스트 끝에 추가.

            //    item.gameObject.SetActive(true);// 생성한 아이템 켜줌
            //    // 업데이트 한번 해줌.
            //    foreach (var controller in controllers)
            //        controller.OnUpdateItem(i, item.gameObject);
            //}
            //// 업데이트 한번 해줌.
            //foreach (var controller in controllers)
            //    controller.OnPostSetupItems();
        }

        //============================================================================================
        // Editor
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

            //-------------------------------------------------------------------------------

            var sr = new GameObject("ScrollRect");
            sr.transform.SetParent(Target_BG);
            var img = sr.AddComponent<Image>();
            img.color = new Color(1f, 1f, 1f, (60f / 255f));
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

            //-------------------------------------------------------------------------------

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

            var contentSizeY = Target_BG.sizeDelta.y - 20f;
            Content.sizeDelta = new Vector2(0f, contentSizeY);
            Content.anchoredPosition = Vector2.zero;
            
        }

        public void Editor_SetInfinityScroll()
        {

        }
    }
}