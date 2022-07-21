using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    /// <summary>
    /// Rect Transfom Anchor �Ǵ� Pivot Ÿ�� �Դϴ�.
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
    /// Infinity Scroll Type �Դϴ�.
    /// </summary>
    public enum eInfinityScrollControllerType
    {
        Infinite,
        Limited,
        Max
    }

    /// <summary>
    /// Infinity Scroll ���� �Դϴ�.
    /// </summary>
    public enum eInfinityScrollDirection
    {
        Vertical,
        Horizontal,
        Max
    }

    /// <summary>
    /// �ڽ� �������� ���� ��ġ �Դϴ�.
    /// </summary>
    public enum eStartCorner
    {

    }

    /// <summary>
    /// Infinity Scroll �� �ڽ� �������� ���� ��� �Դϴ�.
    /// </summary>
    public enum eChildAlignment
    {
    }
}