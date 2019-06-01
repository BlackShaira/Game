using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    public string MapName;

    public void LoadMap()
    {
        SceneManager.LoadScene(MapName);
    }
}
