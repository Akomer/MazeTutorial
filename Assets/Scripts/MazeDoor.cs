using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MazePassage
{
    public Transform hinge;

    private static Quaternion
    normalDoorRotation = Quaternion.Euler(0f, -90f, 0f),
    mirroredDoorRotation = Quaternion.Euler(0f, 90f, 0f);

    private bool isMirrored;
    private bool isTheDoorOpen;

    public override void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        base.Initialize(cell, otherCell, direction);
        var GFX = transform.GetChild(0);
        if (OtherSideOfDoor != null)
        {
            isMirrored = true;
            GFX.transform.Rotate(0f, 180f, 0f);
        }
        for (var i = 0; i < 4; i++)
        {
            var doorPart = GFX.GetChild(i);
            doorPart.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
        }
    }

    public override void OnPlayerEntered()
    {
        if (!isTheDoorOpen)
        {
            OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredDoorRotation : normalDoorRotation;
            isTheDoorOpen = true;
            OtherSideOfDoor.isTheDoorOpen = true;
            OtherSideOfDoor.cell.room.Show();
        }
        if (isTheDoorOpen && !OtherSideOfDoor.isTheDoorOpen)
        {
            OtherSideOfDoor.hinge.localRotation = isMirrored ? normalDoorRotation : mirroredDoorRotation;
            OtherSideOfDoor.isTheDoorOpen = true;
            OtherSideOfDoor.cell.room.Show();
        }
    }

    public override void OnPlayerExited()
    {
        hinge.localRotation = Quaternion.identity;
        isTheDoorOpen = false;
        OtherSideOfDoor.cell.room.Hide();
    }

    private MazeDoor OtherSideOfDoor
    {
        get
        {
            return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
        }
    }
}
