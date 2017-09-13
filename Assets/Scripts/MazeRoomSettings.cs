using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRoomSettings : MonoBehaviour {

    private int actualIndex = 0;

    public MazeRoomSettingsDTO[] settings;

    public MazeRoomSettingsDTO ActualSettings()
    {
        return settings[actualIndex];
    }

    public MazeRoomSettingsDTO NextSettings()
    {
        actualIndex = (actualIndex + 1) % settings.Length;
        return settings[actualIndex];
    }

    public MazeRoomSettingsDTO RandomSettings()
    {
        return settings[Random.Range(0, settings.Length - 1)];
    }
}
