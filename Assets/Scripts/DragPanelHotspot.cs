﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragPanelHotspot : MonoBehaviour, IPointerDownHandler, IDragHandler {

    private Vector2 pointerOffset;
    private RectTransform canvasRectTransform;
    private RectTransform panelRectTransform;

    private float panelWidth;
    private float panelHeight;

    void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (null != canvas)
        {
            canvasRectTransform = canvas.transform as RectTransform;
            panelRectTransform = transform.parent as RectTransform;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        panelRectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform,
            data.position, data.pressEventCamera, out pointerOffset);

        Vector3[] panelCorners = new Vector3[4];
        panelRectTransform.GetLocalCorners(panelCorners);
        panelWidth = panelCorners[2].x - panelCorners[0].x;
        panelHeight = panelCorners[2].y - panelCorners[0].y;

    }

    public void OnDrag(PointerEventData data)
    {
        if (null == panelRectTransform) return;

        Vector2 pointerPosition = ClampToWindow(data);
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform,
            pointerPosition, data.pressEventCamera, out localPointerPosition))
        {
            panelRectTransform.localPosition = localPointerPosition - pointerOffset;
        }
    }

    Vector2 ClampToWindow(PointerEventData data)
    {
        Vector2 rawPointerPosition = data.position;

        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);

        float clampX = Mathf.Clamp(rawPointerPosition.x,
            canvasCorners[0].x + pointerOffset.x,
            canvasCorners[2].x - panelWidth + pointerOffset.x);
        float clampY = Mathf.Clamp(rawPointerPosition.y,
            canvasCorners[0].y + panelHeight + pointerOffset.y,
            canvasCorners[2].y + pointerOffset.y);

        Vector2 newPointerPosition = new Vector2(clampX, clampY);
        return newPointerPosition;
    }
}
