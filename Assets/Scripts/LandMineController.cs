using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineController : MonoBehaviour
{
    // inspector
    public SHAPE Shape = SHAPE.PENTAGON;
    public GameObject LandMineImage = null;
    public GameObject Curver = null;
    public TextMesh Number = null;
    [SerializeField] private float m_width = 1f;
    [SerializeField] private float m_height = 1f;
    [SerializeField] private float m_reverseHeight = 1f;

    //
    public float Width { get { return m_width; } }
    public float Height { get { return m_height; } }
    public float ReverseHeight { get { return m_reverseHeight; } }
    public bool IsMine { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsReverse { get; private set; }

    public LandMineController()
    {
        IsMine = false;
        IsReverse = false;
    }

    public void Initialize(bool isMine, int x, int y, bool isReverse)
    {
        // set mine
        LandMineImage.SetActive(isMine);
        IsMine = isMine;

        // set pos
        X = x;
        Y = y;

        // set reverse
        IsReverse = isReverse;
    }

    private void OnMouseDown()
    {
        DisableCurver();
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

    public void DisableCurver()
    {
        Curver.SetActive(false);
    }

    public bool IsCurvered()
    {
        return Curver.activeInHierarchy;
    }
}
