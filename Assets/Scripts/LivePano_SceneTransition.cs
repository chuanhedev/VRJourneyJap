using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivePano_SceneTransition : MonoBehaviour
{
    public List<Renderer> planesRender;
    private float myTimer = 0f;
    private float fadeOutTime = 0;
    private float fadeInTime = 0;

    //public void SetMaterials(List<Material> materialList){
    //	int index = 0;
    //	foreach (Renderer r in planesRender) {
    //		r.material = materialList [index];
    //		index++;
    //	}
    //}

    void Update()
    {
        if (fadeOutTime > 0)
        {
            myTimer -= Time.deltaTime;
            if (myTimer >= 0)
            {
                foreach (Renderer r in planesRender)
                {
                    float alpha = myTimer / fadeOutTime;
                    r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, alpha);
                }
            }
            else
            {
                fadeOutTime = 0;
                Destroy(this.gameObject);
            }
        }

        if (fadeInTime > 0)
        {
            myTimer -= Time.deltaTime;
            if (myTimer >= 0)
            {
                foreach (Renderer r in planesRender)
                {
                    float alpha = (fadeInTime - myTimer) / fadeInTime;
                    r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, alpha);
                }
            }
            else
            {
                fadeInTime = 0;
                foreach (Renderer r in planesRender)
                {
                    r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, 1);
                }
            }
        }
    }

    public void FadeOut(float timer)
    {
        myTimer = timer;
        fadeOutTime = timer;
    }

    public void FadeIn(float timer)
    {
        myTimer = timer;
        fadeInTime = timer;
        foreach (Renderer r in planesRender)
        {
            r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, 0);
        }
    }
}
