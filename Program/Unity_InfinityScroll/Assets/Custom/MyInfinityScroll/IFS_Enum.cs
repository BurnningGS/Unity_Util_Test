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
    /// Infinity Scroll Type �Դϴ�.
    /// </summary>
    public enum eIFS_ScrollType
    {
        Infinite = 0,
        Limited,
        Max
    }


    /// <summary>
    /// Infinity Scroll ���� �Դϴ�.
    /// </summary>
    public enum eIFS_ST_Axis
    {
        Horizontal = 0,
        Vertical,
        Max
    }

    /// <summary>
    /// �ڽ� �������� ���� ��ġ �Դϴ�.
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
    /// Infinity Scroll �� �ڽ� �������� ���� ��� �Դϴ�.
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