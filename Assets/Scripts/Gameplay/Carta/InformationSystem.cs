using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationSystem : MonoBehaviour
{
    private static InformationSystem current;

    public InformationCardHover informationHover;
    
    private void Start()
    {
        current = this;
        //current.informationHover.gameObject.SetActive(false);
    }

    public static void Show(Sprite content)
    {
        current.informationHover.setCardImage(content);
        //current.informationHover.gameObject.SetActive(true);
        current.informationHover.GetComponent<Image>().enabled = true;
    }


    public static void Hide()
    {
        //current.informationHover.gameObject.SetActive(false);
        current.informationHover.GetComponent<Image>().enabled = false;
    }

}
