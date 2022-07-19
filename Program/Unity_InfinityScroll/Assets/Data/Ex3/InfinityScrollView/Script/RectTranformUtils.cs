using UnityEngine;
namespace OneP.InfinityScrollView
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Set the scale to 1,1,1
        /// </summary>
        public static void SetDefaultScale(this RectTransform trans) => trans.localScale = new Vector3(1, 1, 1);

        /// <summary>
        /// 앵커와 피벗이 모두 배치되어야 하는 지점을 설정합니다. 이렇게 하면 위치와 스케일을 매우 쉽게 설정할 수 있지만 자동 크기 조정이 손상됩니다.
        /// </summary>
        public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
        {
            trans.pivot = aVec;
            trans.anchorMin = aVec;
            trans.anchorMax = aVec;
        }

        /// <summary>
        /// RectTransform의 현재 크기를 Vector2로 가져옵니다.
        /// </summary>
        public static Vector2 GetSize(this RectTransform trans) => trans.rect.size;
        public static float GetWidth(this RectTransform trans) => trans.rect.width;
        public static float GetHeight(this RectTransform trans) => trans.rect.height;

        /// <summary>
        /// 부모 좌표 내에서 RectTransform의 위치를 설정합니다. 피벗의 위치에 따라 RectTransform 실제 위치가 달라집니다.
        /// </summary>
        public static void SetLocalPosition(this RectTransform trans, Vector2 newPos) => trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
        public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos) => trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos) => trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos) => trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos) => trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        public static void SetSizeDelta(this RectTransform trans, Vector2 newSize)
        {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }
        public static void SetWidth(this RectTransform trans, float newSize) => SetSizeDelta(trans, new Vector2(newSize, trans.rect.size.y));
        public static void SetHeight(this RectTransform trans, float newSize) => SetSizeDelta(trans, new Vector2(trans.rect.size.x, newSize));

    }
}