using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TD
{
    /// <summary>
    /// Infinity Scroll �� ���Ǵ� �������̽� �Դϴ�.
    /// </summary>
    public interface TDI_InfinityScroll
    {
        void OnPostSetupItems();
        void OnUpdateItem(int itemCount, GameObject obj);
    }
}