using UnityEngine;

public class CellTest : MonoBehaviour, IOnTouchEnded
{
    bool visited = false;
    int val;
    Vector2Int pos;

    public Sprite spriteNormal, spriteVisited, spritePath;


    GridTest gridTest;

    TextMesh textMesh;
    SpriteRenderer spriteRenderer;
    public Vector2Int Pos
    {
        get { return pos; }
        set
        {
            pos = value;
            textMesh.text = string.Format("[{0}, {1}]\n{2}", Pos.x, Pos.y, val);
        }
    }

    public int Value
    {
        get { return val; }
        set
        {
            val = value;
            if (val == -1)
                spriteRenderer.sprite = spritePath;
            textMesh.text = string.Format("[{0}, {1}]\n{2}", Pos.x, Pos.y, val);
        }
    }

    public bool Visited
    {
        set
        {
            visited = value;
            if (val == -1 && !value)
                spriteRenderer.sprite = spritePath;
            else
                spriteRenderer.sprite = (value) ? (spriteVisited) : (spriteNormal);
        }
        get
        {
            return visited;
        }
    }

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMesh>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gridTest = GameObject.FindAnyObjectByType<GridTest>();
    }

    public void OnTouchEnded()
    {
        gridTest.SelectCell(this);
    }
}
