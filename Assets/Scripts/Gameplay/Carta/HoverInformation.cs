using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite content;
    private static LTDescr delay;

    private void Start()
    {
        content = this.gameObject.GetComponent<Image>().sprite;     
        InformationSystem.Hide();
        
    }

    private void Update()
    {
        if(GameObject.Find("LocalCartaBaralhoVirada").transform.childCount > 0)
        if (this.gameObject.name == "ButtonComprarCartaViradaBaralho" && 
                GameObject.Find("LocalCartaBaralhoVirada").transform.GetChild(0).GetComponent<Carta>().cartaVirada == true)
        {
            content = GameObject.Find("LocalCartaBaralhoVirada").transform.GetChild(0).GetComponent<Image>().sprite;
        }
    }

    private void Teste()
    {
        if (this.gameObject.name == "ButtonComprarCartaViradaBaralho")
        {
            content = GameObject.Find("LocalCartaBaralhoVirada").transform.GetChild(0).GetComponent<Image>().sprite;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Teste();
        delay = LeanTween.delayedCall(0.5f, () =>
        {
            InformationSystem.Show(content);
        });        

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        InformationSystem.Hide();
    }

}
