using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRoom : ScriptableObject {

    public MazeRoomSettingsDTO settings;

    private List<MazeCell> cells = new List<MazeCell>();
    private bool visible = true;

    public void Add(MazeCell cell)
    {
        cell.room = this;
        cells.Add(cell);
    }

    public void Hide()
    {
        if (!visible)
        {
            return;
        }

        visible = false;
        for (var i = 0; i < cells.Count; i++)
        {
            cells[i].Hide();
        }
    }

    public void Show()
    {
        if (visible)
        {
            return;
        }

        visible = true;
        for (var i = 0; i < cells.Count; i++)
        {
            cells[i].Show();
        }
    }

}
