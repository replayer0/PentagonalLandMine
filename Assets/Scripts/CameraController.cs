using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    // inspector
    public float m_panSpeed = 15f;

    //
    public bool IsDragging
    {
        get
        {
            if (false == m_isDragging)
            {
                Update();
            }
            return m_isDragging;
        }
    }

    //
    private bool m_isDragging = false;
    private Vector3 m_oldPos = Vector3.zero;
    private Vector3 m_panOrigin = Vector3.zero;

    public CameraController()
    {
        _instance = this;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Camera UPdate");
            m_isDragging = true;
            m_oldPos = transform.position;
            m_panOrigin =  Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && IsDragging)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - m_panOrigin;
            transform.position = m_oldPos + -1 * pos * m_panSpeed;
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_isDragging = false;
        }
    }
}
