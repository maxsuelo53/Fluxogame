using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class InformationCardHover : MonoBehaviour
{
    public Image CardImagem;
    public RectTransform rectTransform;

    private void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();
    }

    public void setCardImage(Sprite content)
    {
        CardImagem.GetComponent<Image>().sprite = content;

    }

    private void Update()
    {
        Vector3 position = Input.mousePosition;


        float pivotX, pivotY;
        if ((Input.mousePosition.x - 311) < 0)
        {
            pivotX = 0;
        }
        else
        {
            pivotX = 1;
        }
        if ((Input.mousePosition.y - 471) < 0)
        {
            pivotY = 0;
        }
        else
        {
            pivotY = 1;
        }


        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = position;
    }
}
