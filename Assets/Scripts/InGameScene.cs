using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHAPE
{
    PENTAGON = 0,
    HEXAGON,
}

public class InGameScene : MonoBehaviour
{
    /// <summary>
    /// GameObject
    /// </summary>
    public LandMineController m_pentagon = null;
    public LandMineController m_hexagon = null;
    public GameObject m_parent = null;

    public int m_countX = 10;
    public int m_countY = 10;

    private Dictionary<KeyValuePair<int, int>, LandMineController> m_pentagons = new Dictionary<KeyValuePair<int, int>, LandMineController>();
    private Dictionary<KeyValuePair<int, int>, LandMineController> m_hexagons = new Dictionary<KeyValuePair<int, int>, LandMineController>();
    

    private void Start()
    {
        bool isReverse = false;
        bool isIndent = true;

        for (int y = 0; y < m_countY; ++y)
        {
            SHAPE shape = (0 == y % 2) ? SHAPE.HEXAGON : SHAPE.PENTAGON;
            int CountX = (SHAPE.HEXAGON == shape) ? (int)((float)m_countX / 2f) : m_countX;
            bool reverse = isReverse;
            for (int x = 0; x < CountX; ++x)
            {
                Vector3 pos = Vector3.zero;
                bool isMine = (0.5f >= Random.Range(0f, 1f));
                LandMineController controller = null;

                switch (shape)
                {
                    case SHAPE.HEXAGON:
                        {
                            float posX = (true == isIndent) ? x * m_hexagon.GetWitdh() : x * m_hexagon.GetWitdh() + (m_hexagon.GetWitdh() / 2f);
                            float posY = y * m_hexagon.GetHeight() - m_hexagon.GetReverseHeight();

                            pos = new Vector3(posX, posY, 0f);
                            controller = GameObject.Instantiate<LandMineController>(m_hexagon, pos, Quaternion.identity, m_parent.transform);
                            m_hexagons.Add(new KeyValuePair<int, int>(x, y), controller);
                        }
                        break;

                    case SHAPE.PENTAGON:
                        {
                            float posX = x * m_pentagon.GetWitdh();
                            float posY = (true == reverse)
                                ? y * m_pentagon.GetHeight() - m_pentagon.GetReverseHeight()
                                : y * m_pentagon.GetHeight();

                            pos = new Vector3(posX, posY, 0f);
                            controller = GameObject.Instantiate<LandMineController>(m_pentagon, pos, Quaternion.identity, m_parent.transform);
                            if (true == reverse)
                            {
                                controller.transform.Rotate(new Vector3(0, 0, 180f));
                                controller.SetReverse(true);
                            }
                            m_pentagons.Add(new KeyValuePair<int, int>(x, y), controller);
                        }
                        break;
                }

                controller.SetMine(isMine);
                controller.SetX(x);
                controller.SetY(y);

                reverse = !reverse;
            }

            if (1 == y % 2)
            {
                isReverse = !isReverse;
                isIndent = !isIndent;
            }
        }

        foreach (var pair in m_hexagons)
        {
            var hex = pair.Value;
            if (false == hex.IsMine())
            {
                var near = GetNearLandMine(hex);
                int num = 0;

                foreach (var mine in near)
                {
                    if (true == mine.IsMine())
                    {
                        ++num;
                    }
                }

                hex.SetNumber(num);
            }
            else
            {
                hex.SetNumber(0);
            }
        }

        foreach (var pair in m_pentagons)
        {
            var penta = pair.Value;
            if (false == penta.IsMine())
            {
                var near = GetNearLandMine(penta);
                int num = 0;

                foreach (var mine in near)
                {
                    if (true == mine.IsMine())
                    {
                        ++num;
                    }
                }

                penta.SetNumber(num);
            }
            else
            {
                penta.SetNumber(0);
            }
        }
    }

    public List<LandMineController> GetNearLandMine(LandMineController controller)
    {
        var list = new List<LandMineController>();

        if (SHAPE.HEXAGON == controller.Shape)
        {
            int startX = (0 == controller.GetY() % 4) ? controller.GetX() * 2 - 1 : controller.GetX() * 2;
            for (int x = startX; x < startX + 3; ++x)
            {
                if (m_pentagons.ContainsKey(new KeyValuePair<int, int>(x, controller.GetY() + 1)))
                {
                    list.Add(m_pentagons[new KeyValuePair<int, int>(x, controller.GetY() + 1)]);
                }

                if (m_pentagons.ContainsKey(new KeyValuePair<int, int>(x, controller.GetY() - 1)))
                {
                    list.Add(m_pentagons[new KeyValuePair<int, int>(x, controller.GetY() - 1)]);
                }
            }
        }
        else
        {

        }

        return list;
    }
}
