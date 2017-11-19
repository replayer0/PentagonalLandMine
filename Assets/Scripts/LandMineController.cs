using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineController : MonoBehaviour
{
    public SHAPE Shape = SHAPE.PENTAGON;
    public GameObject LandMineImage = null;
    public GameObject Curver = null;
    public TextMesh Number = null;
    public float m_width = 1f;
    public float m_height = 1f;
    public float m_reverseHeight = 1f;
    public bool m_isMine = false;

    public int m_x, m_y = 0;
    public bool m_isReverse = false;

    private void OnMouseDown()
    {
        Curver.SetActive(false);
    }

    public float GetWitdh()
    {
        return m_width;
    }

    public float GetHeight()
    {
        return m_height;
    }

    public float GetReverseHeight()
    {
        return m_reverseHeight;
    }

    public void SetMine(bool isMine)
    {
        LandMineImage.SetActive(isMine);
        m_isMine = isMine;
    }

    public bool IsMine()
    {
        return m_isMine;
    }

    public void SetNumber(int num)
    {
        Number.text = num.ToString();

        Number.gameObject.SetActive(0 != num);
    }

    public int GetNumber()
    {
        return Int32.Parse(Number.text);
    }

    public void SetX(int x)
    {
        m_x = x;
    }

    public int GetX()
    {
        return m_x;
    }

    public void SetY(int y)
    {
        m_y = y;
    }

    public int GetY()
    {
        return m_y;
    }

    public void SetReverse(bool isReverse)
    {
        m_isReverse = isReverse;
    }

    public bool IsReverse()
    {
        return m_isReverse;
    }

    public void DisableCurver()
    {
        Curver.SetActive(false);
    }

    public bool IsCurvered()
    {
        return Curver.activeInHierarchy;
    }
}
