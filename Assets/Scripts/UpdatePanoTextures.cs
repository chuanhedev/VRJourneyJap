using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePanoTextures
{
    FacadeManager facadeManager;
    public UpdatePanoTextures(FacadeManager facadeManager)
    {
        this.facadeManager = facadeManager;
    }

    public void UpdatePano(string panoPath)
    {
        LoadResource(panoPath);
    }

    void LoadResource(string panoPath)
    {
        Texture[] textures = Resources.LoadAll<Texture>(panoPath);

        for (int i = 0; i < textures.Length; i++)
        {
            facadeManager.panoMats[i].mainTexture = textures[i];
        }
    }
}
