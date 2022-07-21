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
        /// Infinity Scroll Item Event Class(���� Ŭ����) ����
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
            // ����ó��
            //
            if (Data.ItemList.First == null)
                return;



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

            CreateData.TargetScrollRect = GetComponentInParent<ScrollRect>();
            CreateData.TargetScrollRect.horizontal = (CreateData.ScrollDirection == eInfinityScrollDirection.Horizontal);
            CreateData.TargetScrollRect.vertical = (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical);
            CreateData.TargetScrollRect.content = rectTransform;

            CreateData.CreateItemPrefab.gameObject.SetActive(false); // ���� ������ ���� ����.

            // ������ ����.
            for (int i = 0; i < CreateData.InstantateItemCount; i++)
            {
                var item = Instantiate(CreateData.CreateItemPrefab) as RectTransform;
                item.SetParent(transform, false); // �θ���
                item.name = i.ToString(); // �̸� ����
                item.anchoredPosition = (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -itemScale * i) : new Vector2(itemScale * i, 0);
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
                if (anchoredPosition > 0) // ó���϶� 
                {
                    CreateData.TargetScrollRect.decelerationRate = 0;
                    anchoredPosition = 0;
                }
                else if (anchoredPosition <= -(CreateData.LimitTypeMaxItemCount-4) * Data.ChildItemScale) // �������϶�
                {
                    CreateData.TargetScrollRect.decelerationRate = 0;
                    anchoredPosition = -(CreateData.LimitTypeMaxItemCount - 4) * Data.ChildItemScale;
                }
                else // ��
                    CreateData.TargetScrollRect.decelerationRate = 0.3f;
            }



            while (anchoredPosition - Data.DiffPreFramePosition < -itemScale * 2)
            {
                // TODO : Ż�� ����.-> ���ѷ��� �ݺ�����.


                Debug.Log("<color=red>���� ����ø��� -> ȭ���� ��������.</color>");
                Data.DiffPreFramePosition -= itemScale;

                var item = Data.ItemList.First.Value;
                Data.ItemList.RemoveFirst();
                Data.ItemList.AddLast(item);

                // ��ġ ������
                var pos = itemScale * CreateData.InstantateItemCount + itemScale * Data.CurrentItemNumb;
                item.anchoredPosition = (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);

                // ��ġ�� �ٲ� ������ ��ϵ� �̺�Ʈ �Լ� ����. 
                Data.onUpdateItemEvents.Invoke(Data.CurrentItemNumb + CreateData.InstantateItemCount, item.gameObject);

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
                item.anchoredPosition = (CreateData.ScrollDirection == eInfinityScrollDirection.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);
                Data.onUpdateItemEvents.Invoke(Data.CurrentItemNumb, item.gameObject);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------

        public void OnClick_CreateInfinityScroll()
        {
            Debug.Log("�ʱ� ����");
        }
    }
}