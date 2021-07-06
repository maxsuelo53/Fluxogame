using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AlertMessage : MonoBehaviour
{
    public Text MessageText;
    public UnityEvent onCompleteCallBack;

    public void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.1f);
        StartCoroutine(OnClose());

    }

    public IEnumerator OnClose()
    {
        yield return new WaitForSeconds(2);
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.3f).setOnComplete(DestroyMe);
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
