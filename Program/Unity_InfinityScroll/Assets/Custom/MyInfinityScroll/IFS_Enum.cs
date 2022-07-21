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

    /// <summary>
    /// Infinity Scroll Type 입니다.
    /// </summary>
    public enum eInfinityScrollControllerType
    {
        Infinite,
        Limited,
        Max
    }

    /// <summary>
    /// Infinity Scroll 방향 입니다.
    /// </summary>
    public enum eInfinityScrollDirection
    {
        Vertical,
        Horizontal,
        Max
    }

    /// <summary>
    /// 자식 아이템의 시작 위치 입니다.
    /// </summary>
    public enum eStartCorner
    {

    }

    /// <summary>
    /// Infinity Scroll 의 자식 아이템의 정렬 방식 입니다.
    /// </summary>
    public enum eChildAlignment
    {
    }
}