using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePanoBlack
{
    FacadeManager facadeManager;
    float alpha;
    string panoPath;
    Image black;

    public UpdatePanoBlack(FacadeManager facadeManager)
    {
        this.facadeManager = facadeManager;
    }

    public void UpdatePano(Image black, string panoPath)
    {
        this.black = black;
        this.panoPath = panoPath;
        alpha = 0;
        facadeManager.StartCoroutine(IBlackShow());
    }

    IEnumerator IBlackShow()
    {
        yield return null;
        alpha += Time.deltaTime * 3f;
        if (alpha > 1) alpha = 1;
        black.color = new Color(black.color.r, black.color.b, black.color.g, alpha);

        if (alpha == 1)
        {
            facadeManager.StopCoroutine(IBlackShow());
            facadeManager.LoadPano(panoPath);
            facadeManager.StartCoroutine(IBlackHide());
        }
        else
        {
            facadeManager.StartCoroutine(IBlackShow());
        }
    }

    IEnumerator IBlackHide()
    {
        yield return null;
        alpha -= Time.deltaTime * 0.7f;
        if (alpha < 0) alpha = 0;
        black.color = new Color(black.color.r, black.color.b, black.color.g, alpha);
        if (alpha == 0)
        {
            facadeManager.StopCoroutine(IBlackHide());
        }
        else
        {
            facadeManager.StartCoroutine(IBlackHide());
        }
    }
}
