using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD
{
    public class TD_IFS_ItemController : MonoBehaviour,TDI_InfinityScroll
    {
        private TD_InfinityScroll _myScroll= null;

        public void OnPostSetupItems()
        {
            if (_myScroll == null)
                _myScroll = GetComponent<TD_InfinityScroll>();

            _myScroll.Data.onUpdateItemEvents.AddListener(OnUpdateItem);

            if (_myScroll.CreateData.InfiniteScrollType == eInfinityScrollControllerType.Infinite)
            { 
                _myScroll.CreateData.TargetScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            }
            else if (_myScroll.CreateData.InfiniteScrollType == eInfinityScrollControllerType.Limited)
            {
                GetComponentInParent<ScrollRect>().movementType = ScrollRect.MovementType.Elastic;
                var rt = GetComponent<RectTransform>();
                var delta = rt.sizeDelta;
                delta.y = _myScroll.itemScale * _myScroll.CreateData.LimitTypeMaxItemCount;
                rt.sizeDelta = delta;
            }
        }

        public void OnUpdateItem(int itemCount, GameObject targetItem)
        {
            if (_myScroll == null)
                _myScroll = GetComponent<TD_InfinityScroll>();

            if (_myScroll.CreateData.InfiniteScrollType == eInfinityScrollControllerType.Limited)
            {

                if (itemCount < 0 || itemCount >= _myScroll.CreateData.LimitTypeMaxItemCount)
                    targetItem.SetActive(false);
                else
                    targetItem.SetActive(true);
            }


            var item = targetItem.GetComponentInChildren<TD_IFS_Item>();
            item.UpdateItem(itemCount);
        }
    }
}