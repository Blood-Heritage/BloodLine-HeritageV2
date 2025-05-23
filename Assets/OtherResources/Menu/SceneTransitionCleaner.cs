using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionCleaner : MonoBehaviour
{
    void Start()
    {
        GameObject transitionImage = GameObject.Find("TransitionImage");
        if (transitionImage != null)
        {
            CanvasGroup cg = transitionImage.GetComponent<CanvasGroup>();
            StartCoroutine(FadeOutAndDestroy(cg, transitionImage));
        }
    }

    private IEnumerator FadeOutAndDestroy(CanvasGroup cg, GameObject go)
    {
        float duration = 1f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            cg.alpha = 1f - t / duration;
            yield return null;
        }
        cg.alpha = 0f;
        Destroy(go);
    }
}
