using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace OneP.InfinityScrollView
{
    public enum InfinityType
    {
        Vertical,
        Horizontal
    }

    public enum VerticalType
    {
        TopToBottom,
        BottomToTop
    }

    public enum HorizontalType
    {
        LeftToRight,
        RigthToLeft
    }

    public class InfinityGridScrollView : MonoBehaviour
    {
        [Header("Setting Reference object")]
        public GameObject ChildItem; // link object item
        public ScrollRect targetScrollRect;// link to UGUI scrollRect
        public RectTransform targetContentBox;// link to content that contain all item in scrollrect

        [Header("Setting For Custom Scroll View")]
        public InfinityType type = InfinityType.Vertical;// type scrollview
        public VerticalType verticalType = VerticalType.TopToBottom;
        public HorizontalType horizontalType = HorizontalType.LeftToRight;
        public float overrideX = 0;
        public float overrideY = 0;
        public float extraContentLength = 0;

        [Header("Setting For Custom Data")]
        public Vector2 showCellNumber = new Vector2(3, 4);
        public Vector2 cellSize = new Vector2(100, 100);
        public int itemGenerate = 10; // number item generate, note: only need create +2 more item appear, if max item appear in screen is 5 =>itemGenerate =7 is enough
        public int totalNumberItem = 100;// total item of scrollview

        [Header("flat check auto setup references")]
        public bool isOverrideSettingScrollbar = true;
        public bool setupOnAwake = true;

        private List<GameObject> ChildLst = new List<GameObject>();
        private GameObject[] arrayCurrent = null;
        private int cacheOld = -1;
        private bool isInit = false;


        //============================================================================
        //
        // Unity Base
        //

        void Awake()
        {
            // 이벤트 추가
            targetScrollRect = GetComponent<ScrollRect>();
            targetScrollRect.onValueChanged.AddListener(Event_OnScrollChange);
            
            // 셋업(초기화)
            if (setupOnAwake)
                Setup();
        }


        //============================================================================
        //
        // Event
        //

        #region [Evnet] OnScrollChange
        //------------------------------------------------------------------------------------------------------------------------------------------
        public void Event_OnScrollChange(Vector2 vec)
        {
            if (arrayCurrent == null || arrayCurrent.Length < 1)
                return;

            int index = GetCurrentIndex();
            if (cacheOld != index)
            {
                cacheOld = index;
            }
            else
            {
                return;
            }

            if (!FixFastReload(index))
            {
                int num = type == InfinityType.Vertical ? (int)showCellNumber.x : (int)showCellNumber.y;
                int max = num + index * num;
                for (int i = index * num; i < max; i++)
                {
                    ChangeReload(i);
                }
            }
        }
        #endregion


        //============================================================================
        //
        // 초기화
        //

        public void Setup()
        {
            //------------------------------------------------------
            // 예외처리
            //
            if (ChildItem == null)
            {
                Debug.LogWarning("No prefab/Gameobject Item linking");
                return;
            }

            //------------------------------------------------------
            // 총 너비 또는 높이 계산
            //
            if (type == InfinityType.Vertical) // 세로방향일때 총 높이 계산
            {
                int num = Mathf.CeilToInt(totalNumberItem / showCellNumber.x); 
                int totalHeight = (int)(num * cellSize.y);
                targetContentBox.SetHeight(totalHeight);
            }
            else if (type == InfinityType.Horizontal) // 가로 방향일떄 총 너비 계산.
            {
                int num = Mathf.CeilToInt(totalNumberItem / showCellNumber.y);
                int totalWidth = (int)(num * cellSize.x);
                targetContentBox.SetWidth(totalWidth);
            }

            //------------------------------------------------------
            // 초기화
            //

            // 배열 초기화
            arrayCurrent = new GameObject[totalNumberItem];

            // 앵커 % 피봇 초기화.
            int itemX = horizontalType == HorizontalType.LeftToRight ? 0 : 1;
            int itemY = verticalType == VerticalType.TopToBottom ? 1 : 0;
            targetContentBox.anchorMin = new Vector2(itemX, itemY);
            targetContentBox.anchorMax = new Vector2(itemX, itemY);
            targetContentBox.pivot = new Vector2(itemX, itemY);


            // 아이템 생성. 
            for (int i = 0; i < itemGenerate; i++)
            {
                GameObject obj;
                if (!isInit)
                {
                    if (i < totalNumberItem)
                    {
                        obj = Instantiate(ChildItem);
                        obj.name = "item_" + (i); // 이름 설정
                        obj.transform.SetParent(targetContentBox.transform, false); // 부모 설정. 
                        obj.transform.localScale = Vector3.one;
                        ChildLst.Add(obj);
                        RectTransform rect = obj.GetComponent<RectTransform>();
                        if (rect != null)
                        {
                            rect.anchorMin = new Vector2(itemX, itemY);
                            rect.anchorMax = new Vector2(itemX, itemY);
                            rect.pivot = new Vector2(itemX, itemY);
                        }
                        Reload(obj, i);
                        arrayCurrent[i] = obj;
                    }
                }
                else
                {
                    if (i < totalNumberItem)
                    {
                        obj = ChildLst[i];
                        obj.SetActive(true);
                        Reload(obj, i);
                        arrayCurrent[i] = obj;
                    }
                    else
                    {
                        obj = ChildLst[i];
                        obj.SetActive(false);
                    }
                }
            }
            isInit = true;
        }

        #region [파악중]
        //------------------------------------------------------------------------------------------------------------------------------------------
        private int GetCurrentIndex()
        {
            int index = -1;

            if (type == InfinityType.Vertical)
            {
                if (verticalType == VerticalType.TopToBottom)
                    index = (int)(targetContentBox.anchoredPosition.y / cellSize.y);
                else
                    index = (int)(-targetContentBox.anchoredPosition.y / cellSize.y);
            }

            else if (type == InfinityType.Horizontal)
            {
                if (horizontalType == HorizontalType.LeftToRight)
                    index = (int)(-targetContentBox.anchoredPosition.x / cellSize.x);
                else
                    index = (int)(targetContentBox.anchoredPosition.x / cellSize.x);
            }

            if (index < 0)
                index = 0;
            
            int maxNum = type == InfinityType.Vertical ? (int)showCellNumber.x : (int)showCellNumber.y;
            
            int ceil = Mathf.CeilToInt(totalNumberItem / maxNum);
            
            if (index > ceil - 1)
                index = ceil - 1;
            

            return index;
        }
        public void InternalReload()
        {

            int index = GetCurrentIndex();
            FixFastReload(index);
        }
        private void ChangeReload(int index)
        {
            GameObject objIndex = arrayCurrent[index];
            if (objIndex == null)
            {// truot len
                int next = index + itemGenerate;
                if (next > totalNumberItem - 1)
                {
                    return;
                }
                else
                {
                    GameObject objNow = arrayCurrent[next];
                    if (objNow != null)
                    {//swap
                        arrayCurrent[next] = objIndex;
                        arrayCurrent[index] = objNow;
                        Reload(arrayCurrent[index], index);
                    }
                }
            }
            else
            {// truot xuong
                int num = type == InfinityType.Vertical ? (int)showCellNumber.x : (int)showCellNumber.y;
                if (index > num - 1)
                {
                    GameObject obj = arrayCurrent[index - num];
                    if (obj == null)
                    {
                        return;
                    }
                    int next = index - num + itemGenerate;
                    if (next > totalNumberItem - 1)
                    {
                        return;
                    }
                    else
                    {
                        GameObject objNow = arrayCurrent[next];
                        if (objNow == null)
                        {//swap
                            arrayCurrent[next] = obj;
                            arrayCurrent[index - num] = objNow;
                            Reload(arrayCurrent[next], next);
                        }
                    }
                }
            }
        }

        public bool FixFastReload(int index)
        {
            bool isNeedFix = false;

            int deletNum = type == InfinityType.Vertical ? (int)showCellNumber.x : (int)showCellNumber.y;
            int add = (index + 1) * deletNum;

            for (int i = add; i < add + itemGenerate - deletNum * 2; i++)
            {
                if (i < totalNumberItem)
                {
                    GameObject obj = arrayCurrent[i];
                    if (obj == null)
                    {
                        isNeedFix = true;
                        break;
                    }
                    else if (!obj.name.Equals("item_" + i))
                    {
                        isNeedFix = true;
                        break;
                    }
                }
            }

            if (isNeedFix)
            {
                for (int i = 0; i < totalNumberItem; i++)
                {
                    arrayCurrent[i] = null;
                }

                int maxNum = type == InfinityType.Vertical ? (int)showCellNumber.x : (int)showCellNumber.y;
                int start = index * maxNum;
                if (start + itemGenerate > totalNumberItem)
                {
                    int ceil = Mathf.CeilToInt((totalNumberItem - itemGenerate) / maxNum);
                    start = ceil - 1;
                }
                //Debug.LogError ("Fix Fast reload:"+start+","+index);
                for (int i = 0; i < itemGenerate; i++)
                {
                    arrayCurrent[start + i] = ChildLst[i];
                    Reload(arrayCurrent[start + i], start + i);
                }
                return true;
            }
            return false;
        }

        protected virtual void Reload(GameObject obj, int indexReload)
        {
            obj.transform.name = "item_" + indexReload;
            Vector3 vec = Vector3.zero;
            vec = new Vector2(overrideX, overrideY);
            vec = GetLocationAppear(vec, indexReload);
            obj.transform.localPosition = vec;
         
            InfinityBaseItem baseItem = obj.GetComponent<InfinityBaseItem>();
            if (baseItem != null)
                baseItem.Reload(indexReload);
        }

        // Content 내 Local 위치 지정. 
        private Vector3 GetLocationAppear(Vector2 offset, int location)
        {
            Vector3 vec = offset;

            if (type == InfinityType.Vertical) // 세로형 타입일때.
            {
                if (verticalType == VerticalType.TopToBottom)
                    vec = new Vector3(vec.x + cellSize.x * (location % showCellNumber.x), -cellSize.y * (location / (int)showCellNumber.x), 0);
             
                else
                    vec = new Vector3(vec.x + cellSize.x * (location % showCellNumber.x), cellSize.y * (location / (int)showCellNumber.x), 0);
            }

            else if (type == InfinityType.Horizontal) // 세로형 타입일때.
            {
                float x;
                float y;

                if (horizontalType == HorizontalType.LeftToRight)
                    x = cellSize.x * (location / (int)showCellNumber.y);
                
                else
                    x = -cellSize.x * (location / (int)showCellNumber.y);


                if (verticalType == VerticalType.TopToBottom)
                    y = -(vec.y + cellSize.y * (location % showCellNumber.y));
                
                else
                    y = vec.y + cellSize.y * (location % showCellNumber.y);
                
                vec = new Vector3(x, y, 0);
            }

            return vec;
        }

        #endregion


    }
}
