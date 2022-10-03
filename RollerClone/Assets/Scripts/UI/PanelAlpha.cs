using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAlpha : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    public void PanelOpen()
    {
        StartCoroutine(OpenPanel());
    }
    

    public void PanelClose()
    {
        StartCoroutine(ClosePanel());
    }


    IEnumerator OpenPanel()
    {
        yield return new WaitForEndOfFrame();

        if (_canvasGroup.alpha!=1)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1, 0.5f * Time.deltaTime);
            StartCoroutine(OpenPanel());
        }
        else
        {
            _canvasGroup.interactable = true;
            StopCoroutine(OpenPanel());
        }


    }


    IEnumerator ClosePanel()
    {

        yield return new WaitForEndOfFrame();

        if (_canvasGroup.alpha != 0)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0, 1 * Time.deltaTime);
            StartCoroutine(ClosePanel());
        }
        else
        {
            _canvasGroup.interactable = false;
            StopCoroutine(ClosePanel());
        }
    }
}
