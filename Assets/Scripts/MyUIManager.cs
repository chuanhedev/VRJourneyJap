using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUIManager
{
    public void CheckPicoMenu(GameObject menu)
    {
        if (menu.activeSelf) menu.SetActive(false);
        else menu.SetActive(true);
    }
}
