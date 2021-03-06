﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : Singleton<CameraController>
{
    // inspector
    public float m_panSpeed = 20f;

    //
    private Vector3 m_oldPos = Vector3.zero;
    private Vector3 m_panOrigin = Vector3.zero;
    private bool m_isDragging = false;
    private float m_totalMoveDistance = 0.0f;

    public CameraController()
    {
        _instance = this;
    }

    public void Update()
    {
        //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}

        if (CameraExtension.IsPointerOverUIObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_oldPos = transform.position;
            m_panOrigin =  Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - m_panOrigin;
            transform.position = m_oldPos + -1 * pos * m_panSpeed;

            m_totalMoveDistance += Vector3.Distance(transform.position, m_oldPos);

            if (20.0f < m_totalMoveDistance)
            {
                m_isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_totalMoveDistance = 0.0f;
            m_isDragging = false;
        }
    }

    public bool IsDragging()
    {
        return m_isDragging;
    }
}
