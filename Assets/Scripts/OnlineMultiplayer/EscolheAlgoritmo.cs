using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EscolheAlgoritmo : MonoBehaviour, IPointerDownHandler
{

    public bool escolhida = false;
    public GameObject CartaEscolhidaObjetivos;
    public GameObject PanelObj;

    private void Start()
    {
        PanelObj =  GameObject.Find("PanelObjetivos");
        CartaEscolhidaObjetivos = GameObject.Find("CartaEscolhidaObjetivo");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        GameObject cartaObjEscolhida = Instantiate(this.gameObject, CartaEscolhidaObjetivos.transform);
        PanelObj.SetActive(false);
        cartaObjEscolhida.GetComponent<RectTransform>().sizeDelta = new Vector2(170, 235);
        Destroy(CartaEscolhidaObjetivos.transform.GetChild(0).gameObject);
        var componentChose = cartaObjEscolhida.GetComponent<EscolheAlgoritmo>();
        Destroy(componentChose);

    }

}
