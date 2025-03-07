using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastMsg : MonoBehaviour
{
    private Image img;
    private float fadeInOutTime = 0.3f;
    private static ToastMsg instance = null;

    public static ToastMsg Instance
    {
        get
        {
            if (null == instance) instance = FindObjectOfType<ToastMsg>();
            return instance;
        }
    }
    void Awake()
    {
        if (null == instance) instance = this;
    }
    void Start()
    {
        img = this.gameObject.GetComponent<Image>();
        img.enabled = false;
    }

    private IEnumerator fadeInOut(Image target,float durationTime,bool inOut)
    {
        float start, end;
        if (inOut)
        {
            start = 0.0f;
            end = 1.0f;
        }
        else
        {
            start = 1.0f;
            end = 0.0f;
        }
        Color current = Color.clear;
        float elapsedTime = 0.0f;

        while (elapsedTime < durationTime)
        {
            float alpha = Mathf.Lerp(start, end, elapsedTime / durationTime);
            target.color = new Color(current.r, current.g, current.b, alpha);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
    private IEnumerator showMessageCoroutine(float durationTime)
    {
        Color origin = img.color;
        img.enabled = true;

        yield return fadeInOut(img, fadeInOutTime, true);

        float elapsedTime = 0.0f;
        while (elapsedTime < durationTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return fadeInOut(img, fadeInOutTime, false);

        img.enabled = false;
        img.color = origin;
    }

    public void showMessage(float durationTime)
    {
        StartCoroutine(showMessageCoroutine(durationTime));
    }


}
