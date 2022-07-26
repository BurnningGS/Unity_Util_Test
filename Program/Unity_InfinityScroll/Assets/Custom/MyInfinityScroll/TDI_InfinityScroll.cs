using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TD
{
    /// <summary>
    /// Infinity Scroll 에 사용되는 인터페이스 입니다.
    /// </summary>
    public interface TDI_InfinityScroll
    {
        void OnPostSetupItems();
        void OnUpdateItem(int itemCount, GameObject obj);
    }
}