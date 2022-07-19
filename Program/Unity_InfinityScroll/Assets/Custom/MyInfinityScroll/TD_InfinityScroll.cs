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
        public enum eInfinityScrollType
        {
            Infinite,
            Limited,
        }
        public enum eInfinityScrollDirection
        {
            Vertical,
            Horizontal,
        }

        //============================================================================================
        // Inner Class


        /// <summary>
        /// Infinity Scroll Item Event Class(���� Ŭ����) ����
        /// </summary>
        [Serializable]
        public class OnItemPositionChangeEvent : UnityEngine.Events.UnityEvent<int, GameObject> { }

        [Serializable]
        public class NScrollSetting
        {
            [Range(0, 30)]
            public int InstantateItemCount = 9;
            public eInfinityScrollType InfiniteScrollType;
            public eInfinityScrollDirection ScrollDirection;
            public RectTransform CreateItemPrefab;
        }
        [Serializable]
        public class NScrollData
        {
            [NonSerialized]
            public LinkedList<RectTransform> ItemList = new LinkedList<RectTransform>();
            public OnItemPositionChangeEvent onUpdateItemEvents = new OnItemPositionChangeEvent();
            public ScrollRect TargetScrollRect;

            public float DiffPreFramePosition = 0;
            public int CurrentItemNumb = 0;

            public RectTransform _rectTransform;
            public float ChildItemScale = -1;
        }

        [Header("TD_InfinityScroll")]
        [Space(20)]

        public NScrollSetting Setting = new NScrollSetting();
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
            get => Setting.ScrollDirection == eInfinityScrollDirection.Vertical ? -rectTransform.anchoredPosition.y : rectTransform.anchoredPosition.x;
            set
            {
                if (Setting.ScrollDirection == eInfinityScrollDirection.Vertical)
                    rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -value);
                else
                    rectTransform.anchoredPosition = new Vector2(-value, rectTransform.anchoredPosition.y);
            }
        }

       
        public float itemScale
        {
            get
            {
                if (Setting.CreateItemPrefab != null && Data.ChildItemScale == -1)
                {
                    Data.ChildItemScale = Setting.ScrollDirection == eInfinityScrollDirection.Vertical ? Setting.CreateItemPrefab.sizeDelta.y : Setting.CreateItemPrefab.sizeDelta.x;
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
            // ����ó��
            //
            if (Data.ItemList.First == null)
                return;

            if (anchoredPosition > 0)
            {
                Data.TargetScrollRect.decelerationRate = 0;
                anchoredPosition = 0;
            }
            else
                Data.TargetScrollRect.decelerationRate = 0.3f;

            //------------------------------------------------------------------------
            // ��ġ�̵� ������Ʈ 
            //
            updatePosition();
            
        }

        //------------------------------------------------------------------------------------------------------------------------------
        protected void _initialize_InfinityScroll()
        {
            //-----------------------------------------------------------------------------------------------
            // 1. Find Controllers (��Ʈ�ѷ� �˻�)
            //
            var controllers = GetComponents<MonoBehaviour>().Where(item => item is TDI_InfinityScroll).Select(item => item as TDI_InfinityScroll).ToList();

            //-----------------------------------------------------------------------------------------------
            // 2. Create items (������ ����)
            //

            Data.TargetScrollRect = GetComponentInParent<ScrollRect>();
            Data.TargetScrollRect.horizontal = (Setting.ScrollDirection == eInfinityScrollDirection.Horizontal);
            Data.TargetScrollRect.vertical = (Setting.ScrollDirection == eInfinityScrollDirection.Vertical);
            Data.TargetScrollRect.content = rectTransform;

            Setting.CreateItemPrefab.gameObject.SetActive(false); // ���� ������ ���� ����.

            // ������ ����.
            for (int i = 0; i < Setting.InstantateItemCount; i++)
            {
                var item = Instantiate(Setting.CreateItemPrefab) as RectTransform;
                item.SetParent(transform, false); // �θ���
                item.name = i.ToString(); // �̸� ����
                item.anchoredPosition = (Setting.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -itemScale * i) : new Vector2(itemScale * i, 0);
                Data.ItemList.AddLast(item);// ����Ʈ ���� �߰�.

                item.gameObject.SetActive(true);// ������ ������ ����
                // ������Ʈ �ѹ� ����.
                foreach (var controller in controllers)
                    controller.OnUpdateItem(i, item.gameObject);
            }
            // ������Ʈ �ѹ� ����.
            foreach (var controller in controllers)
                controller.OnPostSetupItems();
        }
        protected virtual void updatePosition()
        {
            while (anchoredPosition - Data.DiffPreFramePosition < -itemScale * 2)
            {
                // TODO : Ż�� ����.-> ���ѷ��� �ݺ�����.


                Debug.Log("<color=red>���� ����ø��� -> ȭ���� ��������.</color>");
                Data.DiffPreFramePosition -= itemScale;

                var item = Data.ItemList.First.Value;
                Data.ItemList.RemoveFirst();
                Data.ItemList.AddLast(item);

                // ��ġ ������
                var pos = itemScale * Setting.InstantateItemCount + itemScale * Data.CurrentItemNumb;
                item.anchoredPosition = (Setting.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);

                // ��ġ�� �ٲ� ������ ��ϵ� �̺�Ʈ �Լ� ����. 
                Data.onUpdateItemEvents.Invoke(Data.CurrentItemNumb + Setting.InstantateItemCount, item.gameObject);

                Data.CurrentItemNumb++;
            }

            while (anchoredPosition - Data.DiffPreFramePosition > 0)
            {
                // TODO : Ż�� ����. -> ���ѷ��� �ݺ�����.


                Debug.Log("<color=blue>�Ʒ��� ���� ������ -> ȭ���� ���� �ö󰣴�.</color>");
                Data.DiffPreFramePosition += itemScale;

                var item = Data.ItemList.Last.Value;
                Data.ItemList.RemoveLast();
                Data.ItemList.AddFirst(item);

                Data.CurrentItemNumb--;

                var pos = itemScale * Data.CurrentItemNumb;
                item.anchoredPosition = (Setting.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);
                Data.onUpdateItemEvents.Invoke(Data.CurrentItemNumb, item.gameObject);
            }
        }
        

    }
}