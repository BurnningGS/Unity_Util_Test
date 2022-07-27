using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    /// <summary>
    /// Rect Transfom Anchor 또는 Pivot 타입 입니다.
    /// </summary>
    public enum eAnchorOrPivotType
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottonCenter,
        BottomRight,
        BottomStretch,

        VertStretchLeft,
        VertStretchRight,
        VertStretchCenter,

        HorStretchTop,
        HorStretchMiddle,
        HorStretchBottom,

        StretchAll
    }

    
    public enum eInfinityScrollControllerType
    {
        Infinite,
        Limited,
        Max
    }

    
    public enum eInfinityScrollDirection
    {
        Vertical,
        Horizontal,
        Max
    }

   
    public enum eStartCorner
    {

    }


    public enum eChildAlignment
    {
    }


    //-----------------------------------------------------------------------------------



    /// <summary>
    /// Infinity Scroll Type 입니다.
    /// </summary>
    public enum eIFS_ScrollType
    {
        Infinite = 0,
        Limited,
        Max
    }


    /// <summary>
    /// Infinity Scroll 방향 입니다.
    /// </summary>
    public enum eIFS_ST_Axis
    {
        Horizontal = 0,
        Vertical,
        Max
    }

    /// <summary>
    /// 자식 아이템의 시작 위치 입니다.
    /// </summary>
    public enum eIFS_StartCorner
    {
        Top_Left,
        Top_Middle,
        Top_Right,
        Middle_Left,
        Middle_Middle,
        Middle_Right,
        Bottom_Left,
        Bottom_Middle,
        Bottom_Right,
        Max
    }

    /// <summary>
    /// Infinity Scroll 의 자식 아이템의 정렬 방식 입니다.
    /// </summary>
    public enum eIFS_ChildAligent
    {
        Top_Left,
        Top_Middle,
        Top_Right,
        Middle_Left,
        Middle_Middle,
        Middle_Right,
        Bottom_Left,
        Bottom_Middle,
        Bottom_Right,
        Max
    }

}