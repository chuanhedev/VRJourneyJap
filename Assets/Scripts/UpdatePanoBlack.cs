using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePanoBlack
{
    FacadeManager facadeManager;
    LivePano_SceneTransition livePano_SceneTransition;
    public string panoPath { get; private set; }

    public UpdatePanoBlack(FacadeManager facadeManager)
    {
        this.facadeManager = facadeManager;
    }

    public void UpdatePano(LivePano_SceneTransition livePano_SceneTransition, string panoPath)
    {
        this.panoPath = panoPath;
        this.livePano_SceneTransition = livePano_SceneTransition;

        facadeManager.StartCoroutine(Updatepano());
    }

    public void UpdatePano(string panoPath, bool showLabel)
    {
        this.panoPath = panoPath;
        facadeManager.LoadPano(panoPath);
        if (showLabel)
            facadeManager.UpdateLabel(panoPath);
    }

    IEnumerator Updatepano()
    {
        livePano_SceneTransition.FadeIn(1);
        yield return new WaitForSeconds(1);
        facadeManager.LoadPano(panoPath);
        facadeManager.UpdateLabel(panoPath);
        livePano_SceneTransition.FadeOut(1);
    }
}
