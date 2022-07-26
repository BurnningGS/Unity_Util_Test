using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD

{
    public static class RectTransform_Extention
    {
        /// <summary>
        ///  ��Ŀ �������� Ÿ���� ��ȯ�մϴ�. �ش� �Լ��� �Ǻ������� ���� ��ȯ�մϴ�. 
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="achorType"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void SetAnchor(this RectTransform rt, eAnchorOrPivotType achorType, float offsetX = 0f, float offsetY = 0f)
        {
            // Position Offset
            rt.anchoredPosition = new Vector3(offsetX, offsetY, 0f);

            // Change Achor Preset
            switch (achorType)
            {
                case (eAnchorOrPivotType.TopLeft):
                    {
                        rt.anchorMin = new Vector2(0, 1);
                        rt.anchorMax = new Vector2(0, 1);
                        rt.pivot = new Vector2(0,1);
                        break;
                    }
                case (eAnchorOrPivotType.TopCenter):
                    {
                        rt.anchorMin = new Vector2(0.5f, 1);
                        rt.anchorMax = new Vector2(0.5f, 1);
                        rt.pivot = new Vector2(0.5f, 1);
                        break;
                    }
                case (eAnchorOrPivotType.TopRight):
                    {
                        rt.anchorMin = new Vector2(1, 1);
                        rt.anchorMax = new Vector2(1, 1);
                        rt.pivot = new Vector2(1, 1);
                        break;
                    }
                //-------------------------------------------------------
                case (eAnchorOrPivotType.MiddleLeft):
                    {
                        rt.anchorMin = new Vector2(0, 0.5f);
                        rt.anchorMax = new Vector2(0, 0.5f);
                        rt.pivot = new Vector2(0, 0.5f);
                        break;
                    }
                case (eAnchorOrPivotType.MiddleCenter):
                    {
                        rt.anchorMin = new Vector2(0.5f, 0.5f);
                        rt.anchorMax = new Vector2(0.5f, 0.5f);
                        rt.pivot = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (eAnchorOrPivotType.MiddleRight):
                    {
                        rt.anchorMin = new Vector2(1, 0.5f);
                        rt.anchorMax = new Vector2(1, 0.5f);
                        rt.pivot = new Vector2(1, 0.5f);
                        break;
                    }
                //-------------------------------------------------------
                case (eAnchorOrPivotType.BottomLeft):
                    {
                        rt.anchorMin = new Vector2(0, 0);
                        rt.anchorMax = new Vector2(0, 0);
                        rt.pivot = new Vector2(0, 0);
                        break;
                    }
                case (eAnchorOrPivotType.BottonCenter):
                    {
                        rt.anchorMin = new Vector2(0.5f, 0);
                        rt.anchorMax = new Vector2(0.5f, 0);
                        rt.pivot = new Vector2(0.5f, 0);
                        break;
                    }
                case (eAnchorOrPivotType.BottomRight):
                    {
                        rt.anchorMin = new Vector2(1, 0);
                        rt.anchorMax = new Vector2(1, 0);
                        rt.pivot = new Vector2(0, 1);
                        break;
                    }
                //-------------------------------------------------------
                case (eAnchorOrPivotType.HorStretchTop):
                    {
                        rt.anchorMin = new Vector2(0, 1);
                        rt.anchorMax = new Vector2(1, 1);
                        rt.pivot = new Vector2(0.5f, 1);
                        break;
                    }
                case (eAnchorOrPivotType.HorStretchMiddle):
                    {
                        rt.anchorMin = new Vector2(0, 0.5f);
                        rt.anchorMax = new Vector2(1, 0.5f);
                        rt.pivot = new Vector2(0.5f, 1);
                        break;
                    }
                case (eAnchorOrPivotType.HorStretchBottom):
                    {
                        rt.anchorMin = new Vector2(0, 0);
                        rt.anchorMax = new Vector2(1, 0);
                        rt.pivot = new Vector2(0.5f, 0);
                        break;
                    }
                //-------------------------------------------------------
                case (eAnchorOrPivotType.VertStretchLeft):
                    {
                        rt.anchorMin = new Vector2(0, 0);
                        rt.anchorMax = new Vector2(0, 1);
                        rt.pivot = new Vector2(0, 0.5f);
                        break;
                    }
                case (eAnchorOrPivotType.VertStretchCenter):
                    {
                        rt.anchorMin = new Vector2(0.5f, 0);
                        rt.anchorMax = new Vector2(0.5f, 1);
                        rt.pivot = new Vector2(0, 0.5f);
                        break;
                    }
                case (eAnchorOrPivotType.VertStretchRight):
                    {
                        rt.anchorMin = new Vector2(1, 0);
                        rt.anchorMax = new Vector2(1, 1);
                        rt.pivot = new Vector2(0, 0.5f);
                        break;
                    }
                //-------------------------------------------------------
                case (eAnchorOrPivotType.StretchAll):
                    {
                        rt.anchorMin = new Vector2(0, 0);
                        rt.anchorMax = new Vector2(1, 1);
                        rt.pivot = new Vector2(0.5f, 0.5f);
                        break;
                    }
            }
        }


        /// <summary>
        /// RectTransform �� Pivot�� �����մϴ�.
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="pivotType"></param>
        public static void SetPivot(this RectTransform rt, eAnchorOrPivotType pivotType)
        {
            switch (pivotType)
            {
                case (eAnchorOrPivotType.TopLeft):                rt.pivot = new Vector2(0, 1);       break;
                case (eAnchorOrPivotType.TopCenter):              rt.pivot = new Vector2(0.5f, 1);    break;
                case (eAnchorOrPivotType.TopRight):               rt.pivot = new Vector2(1, 1);       break;
                //---------------------------------------------
                case (eAnchorOrPivotType.MiddleLeft):             rt.pivot = new Vector2(0, 0.5f);    break;
                case (eAnchorOrPivotType.MiddleCenter):           rt.pivot = new Vector2(0.5f, 0.5f); break;
                case (eAnchorOrPivotType.MiddleRight):            rt.pivot = new Vector2(1, 0.5f);    break;
                //---------------------------------------------
                case (eAnchorOrPivotType.BottomLeft):             rt.pivot = new Vector2(0, 0);       break;
                case (eAnchorOrPivotType.BottonCenter):           rt.pivot = new Vector2(0.5f, 0);    break;
                case (eAnchorOrPivotType.BottomRight):            rt.pivot = new Vector2(0, 1);       break;
                //---------------------------------------------
                case (eAnchorOrPivotType.HorStretchTop):          rt.pivot = new Vector2(0.5f, 1);    break;
                case (eAnchorOrPivotType.HorStretchMiddle):       rt.pivot = new Vector2(0.5f, 1);    break;
                case (eAnchorOrPivotType.HorStretchBottom):       rt.pivot = new Vector2(0.5f, 0);    break;
                //---------------------------------------------
                case (eAnchorOrPivotType.VertStretchLeft):        rt.pivot = new Vector2(0, 0.5f);    break;
                case (eAnchorOrPivotType.VertStretchCenter):      rt.pivot = new Vector2(0, 0.5f);    break;
                case (eAnchorOrPivotType.VertStretchRight):       rt.pivot = new Vector2(0, 0.5f);    break;
                //---------------------------------------------
                case (eAnchorOrPivotType.StretchAll):             rt.pivot = new Vector2(0.5f, 0.5f); break;
            }
        }


        /// <summary>
        ///  �θ� ������Ʈ�� ũ�⿡ �°� �̹����� Ȯ�� �Ǵ� ��� �� �ݴϴ�.
        /// </summary>
        /// <param name="target">Target �� �Ǵ� �̹��� �Դϴ�.</param>
        /// <param name="parentRt">�θ� ������Ʈ�� RectTransform �Դϴ�.</param>
        public static void SetImageAutoSizeParentSize(ref Image target, ref RectTransform parentRt)
        {
            target.SetNativeSize();
            Vector2 sizeOri = target.gameObject.GetComponent<RectTransform>().sizeDelta;
            Vector2 dstSize = parentRt.sizeDelta;

            if (sizeOri != dstSize)
            {
                float rate;

                if (sizeOri.x >= sizeOri.y)
                    rate = parentRt.sizeDelta.x / sizeOri.x;
                else
                    rate = parentRt.sizeDelta.y / sizeOri.y;

                target.rectTransform.sizeDelta = target.rectTransform.sizeDelta * rate;
            }
            else
                target.rectTransform.sizeDelta = dstSize;
        }

    }



    public static class UtilTest
    {


        
    }

}