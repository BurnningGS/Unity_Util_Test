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
    public class TD_InfinityScroll : UIBehaviour
    {
        
        //============================================================================================
        // Inner Class


        /// <summary>
        /// Infinity Scroll Item Event Class(내부 클래스) 형식
        /// </summary>
        [Serializable]
        public class OnItemPositionChangeEvent : UnityEngine.Events.UnityEvent<int, GameObject> { }

        [Serializable]
        public class NCreateData
        {
            public TD_IFS_ItemController ItemController;
            public RectTransform CreateItemPrefab;

            public eInfinityScrollControllerType InfiniteScrollType = eInfinityScrollControllerType.Limited;
            public eInfinityScrollDirection ScrollDirection;

            [Range(0, 30)]
            public int InstantateItemCount = 9;
            [SerializeField, Range(1, 999)]
            public int LimitTypeMaxItemCount = 30;

            [Header("[  TODO List ]")]
            public eStartCorner StartCorner;
            public eChildAlignment ChildAlignment;

            public Vector2 BGSizeDelta = new Vector2(300f, 300f);
            public Vector4 ScrollRectDiffSize = new Vector4(10f, 10f, 10f, 10f);
            public float ItemRootSize;
            public Vector2 StandardItemSize = new Vector2();
            public Vector2 SpacingSize = new Vector2();
            public Vector2 CellMaxNumb = new Vector2();


            public string TargetScrollRectName = "ScrollRect";
            public ScrollRect TargetScrollRect;

            public string ItemRootName = "ItemRoot";
            public RectTransform ItemRootRt;
        }

        [Serializable]
        public class NScrollData
        {
            [NonSerialized]
            public LinkedList<RectTransform> ItemList = new LinkedList<RectTransform>();
            public OnItemPositionChangeEvent onUpdateItemEvents = new OnItemPositionChangeEvent();
            

            public float DiffPreFramePosition = 0;
            public int CurrentItemNumb = 0;

            public RectTransform _rectTransform;
            public float ChildItemScale = -1;

            public Vector2 ContentViewSize;
        }

        //------------------------------------------------------------------------------------------------------------------------------


        [Header("  ==========  0. TD_InfinityScroll - Create Data  ==========  ")]
        [Space(20)]
        public NCreateData CreateData= new NCreateData();

        [Header("  ==========  1. TD_InfinityScroll - Data  ==========  ")]
        [Space(20)]
        public NScrollData Data = new NScrollData();

        
        

        //------------------------------------------------------------------------------------------------------------------------------
        protected RectTransform rectTransform
        {
            get
            {
                if (Data._rectTransform == null) Data._rectTransform = GetComponent<RectTransform>();
                return Data._rectTransform;
            }
        }

        private float anchoredPosition
        {
            get => CreateData.ScrollDirection == eInfinityScrollDirection.Vertical ? -rectTransform.anchoredPosition.y : rectTransform.anchoredPosition.x;
            set
            {
                if (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical)
                    rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -value);
                else
                    rectTransform.anchoredPosition = new Vector2(-value, rectTransform.anchoredPosition.y);
            }
        }


        public float itemScale
        {
            get
            {
                if (CreateData.CreateItemPrefab != null && Data.ChildItemScale == -1)
                {
                    Data.ChildItemScale = CreateData.ScrollDirection == eInfinityScrollDirection.Vertical ? CreateData.CreateItemPrefab.sizeDelta.y : CreateData.CreateItemPrefab.sizeDelta.x;
                }
                return Data.ChildItemScale;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------
        protected override void Start()
        {
            _initialize_InfinityScroll();
        }
        protected virtual void Update()
        {
            //------------------------------------------------------------------------
            // 예외처리
            //
            if (Data.ItemList.First == null)
                return;



            //------------------------------------------------------------------------
            // 위치이동 업데이트 
            //
            updatePosition();

        }

        //------------------------------------------------------------------------------------------------------------------------------
        protected void _initialize_InfinityScroll()
        {
            //-----------------------------------------------------------------------------------------------
            // 1. Find Controllers (콘트롤러 검색)
            //
            var controllers = GetComponents<MonoBehaviour>().Where(item => item is TDI_InfinityScroll).Select(item => item as TDI_InfinityScroll).ToList();

            //-----------------------------------------------------------------------------------------------
            // 2. Create items (아이템 생성)
            //

            CreateData.TargetScrollRect = GetComponentInParent<ScrollRect>();
            CreateData.TargetScrollRect.horizontal = (CreateData.ScrollDirection == eInfinityScrollDirection.Horizontal);
            CreateData.TargetScrollRect.vertical = (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical);
            CreateData.TargetScrollRect.content = rectTransform;

            CreateData.CreateItemPrefab.gameObject.SetActive(false); // 생성 아이템 원본 꺼줌.

            // 아이템 생성.
            for (int i = 0; i < CreateData.InstantateItemCount; i++)
            {
                var item = Instantiate(CreateData.CreateItemPrefab) as RectTransform;
                item.SetParent(transform, false); // 부모설정
                item.name = i.ToString(); // 이름 설정
                item.anchoredPosition = (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -itemScale * i) : new Vector2(itemScale * i, 0);
                Data.ItemList.AddLast(item);// 리스트 끝에 추가.

                item.gameObject.SetActive(true);// 생성한 아이템 켜줌
                // 업데이트 한번 해줌.
                foreach (var controller in controllers)
                    controller.OnUpdateItem(i, item.gameObject);
            }
            // 업데이트 한번 해줌.
            foreach (var controller in controllers)
                controller.OnPostSetupItems();
        }
        protected virtual void updatePosition()
        {
            if (CreateData.InfiniteScrollType == eInfinityScrollControllerType.Infinite)
            {

                if (anchoredPosition > 0)
                {
                    CreateData.TargetScrollRect.decelerationRate = 0;
                    anchoredPosition = 0;
                }
                else
                    CreateData.TargetScrollRect.decelerationRate = 0.3f;
            }
            else if (CreateData.InfiniteScrollType == eInfinityScrollControllerType.Limited)
            {
                if (anchoredPosition > 0) // 처음일때 
                {
                    CreateData.TargetScrollRect.decelerationRate = 0;
                    anchoredPosition = 0;
                }
                else if (anchoredPosition <= -(CreateData.LimitTypeMaxItemCount-4) * Data.ChildItemScale) // 마지막일때
                {
                    CreateData.TargetScrollRect.decelerationRate = 0;
                    anchoredPosition = -(CreateData.LimitTypeMaxItemCount - 4) * Data.ChildItemScale;
                }
                else // 끝
                    CreateData.TargetScrollRect.decelerationRate = 0.3f;
            }



            while (anchoredPosition - Data.DiffPreFramePosition < -itemScale * 2)
            {
                // TODO : 탈출 구문.-> 무한루프 반복금지.


                Debug.Log("<color=red>위로 끌어올릴떄 -> 화면은 내려간다.</color>");
                Data.DiffPreFramePosition -= itemScale;

                var item = Data.ItemList.First.Value;
                Data.ItemList.RemoveFirst();
                Data.ItemList.AddLast(item);

                // 위치 재조정
                var pos = itemScale * CreateData.InstantateItemCount + itemScale * Data.CurrentItemNumb;
                item.anchoredPosition = (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);

                // 위치가 바뀐 아이템 등록된 이벤트 함수 실행. 
                Data.onUpdateItemEvents.Invoke(Data.CurrentItemNumb + CreateData.InstantateItemCount, item.gameObject);

                Data.CurrentItemNumb++;
            }

            while (anchoredPosition - Data.DiffPreFramePosition > 0)
            {
                // TODO : 탈출 구문. -> 무한루프 반복금지.


                Debug.Log("<color=blue>아래로 끌어 내릴때 -> 화면은 위로 올라간다.</color>");
                Data.DiffPreFramePosition += itemScale;

                var item = Data.ItemList.Last.Value;
                Data.ItemList.RemoveLast();
                Data.ItemList.AddFirst(item);

                Data.CurrentItemNumb--;

                var pos = itemScale * Data.CurrentItemNumb;
                item.anchoredPosition = (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);
                Data.onUpdateItemEvents.Invoke(Data.CurrentItemNumb, item.gameObject);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------

        public void OnClick_CreateInfinityScroll()
        {
            Debug.Log("초기 생성");
        }
    }
}