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
    public class OnItemPositionChange : UnityEngine.Events.UnityEvent<int, GameObject> { }

    public class TD_InfinityScroll : MonoBehaviour
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
        public GameObject LstChildItem;
        private RectTransform _childItemRt;


        //--------------------------------------------------------------------------------------------------------
        [Space(20)]

        [NonSerialized]
        protected LinkedList<RectTransform> itemList = new LinkedList<RectTransform>();
        protected OnItemPositionChange onUpdateItem = new OnItemPositionChange();
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
                if (_childItemRt != null && _itemScale == -1)
                {
                    _itemScale = direction == ScrollDirection.Vertical ? _childItemRt.sizeDelta.y : _childItemRt.sizeDelta.x;
                }
                return _itemScale;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void Start()
        {
            initializeScroll();
        }

        private void Update()
        {
            
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void initializeScroll()
        {
        //    var controllers = GetComponents<MonoBehaviour>()
        //        .Where(item => item is IInfiniteScrollSetup)
        //        .Select(item => item as IInfiniteScrollSetup)
        //        .ToList();

        //    // create items

        //    var scrollRect = GetComponentInParent<ScrollRect>();
        //    scrollRect.horizontal = direction == ScrollDirection.Horizontal;
        //    scrollRect.vertical = direction == ScrollDirection.Vertical;
        //    scrollRect.content = rectTransform;

        //    LstChildItem.gameObject.SetActive(false);

        //    for (int i = 0; i < instantateItemCount; i++)
        //    {
        //        var item = Instantiate(LstChildItem).GetComponent<RectTransform>();
        //        item.SetParent(transform, false);
        //        item.name = i.ToString();
        //        item.anchoredPosition = direction == ScrollDirection.Vertical ? new Vector2(0, -itemScale * i) : new Vector2(itemScale * i, 0);
        //        itemList.AddLast(item);

        //        item.gameObject.SetActive(true);

        //        foreach (var controller in controllers)
        //        {
        //            controller.OnUpdateItem(i, item.gameObject);
        //        }
        //    }

        //    foreach (var controller in controllers)
        //    {
        //        controller.OnPostSetupItems();
        //    }
        }


    }
}