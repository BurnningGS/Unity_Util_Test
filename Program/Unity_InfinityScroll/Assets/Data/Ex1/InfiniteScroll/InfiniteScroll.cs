﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Ex1
{
    public class InfiniteScroll : UIBehaviour
    {
        public enum ScrollDirection
        {
            Vertical,
            Horizontal,
        }

        [SerializeField]
        [Header("[ 스크롤 방향 ]")]
        private ScrollDirection direction;

        [SerializeField, Range(0, 30)]
        [Header("[ 아이템 갯수 ]")]
        int instantateItemCount = 9;


        [SerializeField]
        [Header("[ 아이템 프리팹 ]")]
        private RectTransform itemPrototype;


        [Space(20)]


        public OnItemPositionChange onUpdateItem = new OnItemPositionChange();
        [System.NonSerialized] public LinkedList<RectTransform> itemList = new LinkedList<RectTransform>();


        public float diffPreFramePosition = 0;
        public int currentItemNo = 0;




        // cache component

        private RectTransform _rectTransform;
        protected RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        private float anchoredPosition
        {
            get
            {
                return direction == ScrollDirection.Vertical ? -rectTransform.anchoredPosition.y : rectTransform.anchoredPosition.x;
            }
        }

        private float _itemScale = -1;
        public float itemScale
        {
            get
            {
                if (itemPrototype != null && _itemScale == -1)
                {
                    _itemScale = direction == ScrollDirection.Vertical ? itemPrototype.sizeDelta.y : itemPrototype.sizeDelta.x;
                }
                return _itemScale;
            }
        }

        protected override void Start()
        {
            //-----------------------------------------------------------------------------------------------
            // 1. Find Controllers (콘트롤러 검색)
            //
            var controllers = GetComponents<MonoBehaviour>().Where(item => item is IInfiniteScrollSetup).Select(item => item as IInfiniteScrollSetup).ToList();

            //-----------------------------------------------------------------------------------------------
            // 2. Create items (아이템 생성)
            //

            var scrollRect = GetComponentInParent<ScrollRect>();
            scrollRect.horizontal = (direction == ScrollDirection.Horizontal);
            scrollRect.vertical = (direction == ScrollDirection.Vertical);
            scrollRect.content = rectTransform;

            itemPrototype.gameObject.SetActive(false); // 생성 아이템 원본 꺼줌.

            // 아이템 생성.
            for (int i = 0; i < instantateItemCount; i++)
            {
                var item = Instantiate(itemPrototype) as RectTransform;
                item.SetParent(transform, false); // 부모설정
                item.name = i.ToString(); // 이름 설정
                item.anchoredPosition = (direction == ScrollDirection.Vertical) ? new Vector2(0, -itemScale * i) : new Vector2(itemScale * i, 0);
                itemList.AddLast(item);// 리스트 끝에 추가.

                item.gameObject.SetActive(true);// 생성한 아이템 켜줌

                foreach (var controller in controllers)
                    controller.OnUpdateItem(i, item.gameObject);
            }

            foreach (var controller in controllers)
                controller.OnPostSetupItems();
            
        }

        void Update()
        {
            if (itemList.First == null)
            {
                return;
            }

            while (anchoredPosition - diffPreFramePosition < -itemScale * 2)
            {
                diffPreFramePosition -= itemScale;

                var item = itemList.First.Value;
                itemList.RemoveFirst();
                itemList.AddLast(item);

                var pos = itemScale * instantateItemCount + itemScale * currentItemNo;
                item.anchoredPosition = (direction == ScrollDirection.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);

                onUpdateItem.Invoke(currentItemNo + instantateItemCount, item.gameObject);

                currentItemNo++;
            }

            while (anchoredPosition - diffPreFramePosition > 0)
            {
                diffPreFramePosition += itemScale;

                var item = itemList.Last.Value;
                itemList.RemoveLast();
                itemList.AddFirst(item);

                currentItemNo--;

                var pos = itemScale * currentItemNo;
                item.anchoredPosition = (direction == ScrollDirection.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);
                onUpdateItem.Invoke(currentItemNo, item.gameObject);
            }
        }

        [System.Serializable]
        public class OnItemPositionChange : UnityEngine.Events.UnityEvent<int, GameObject> { }
    }
}