using UnityEngine;

public class MazeCell : MonoBehaviour {

    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
    private int initializedEdgeCount;

    public IntVector2 coordinates;
    public MazeRoom room;

    public void Initialize(MazeRoom room)
    {
        room.Add(this);
        transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
    }

    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        var ind = (int)direction;
        if (edges[ind] == null)
        {
            initializedEdgeCount++;
        }
        edges[ind] = edge;
    }

    public bool IsFullyInitialized
    {
        get { return initializedEdgeCount == edges.Length; }
    }

    public MazeDirection RandomUninitializedDirection()
    {
        var skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
        for (var i = 0; i < MazeDirections.Count; i++)
        {
            if (edges[i] == null)
            {
                if (skips == 0)
                {
                    return (MazeDirection)i;
                }
                skips -= 1;
            }
        }
        throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnPlayerEntered()
    {
        room.Show();
        for (var i = 0; i < edges.Length; i++)
        {
            edges[i].OnPlayerEntered();
        }
    }

    public void OnPlayerExited()
    {
        room.Hide();
        for (var i = 0; i < edges.Length; i++)
        {
            edges[i].OnPlayerExited();
        }
    }
}
