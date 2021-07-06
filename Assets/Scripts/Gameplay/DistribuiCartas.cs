using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistribuiCartas : MonoBehaviour
{
    public GameObject handPlayer;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < 6; i++)
        {

            GameObject cartaDistribuida = this.transform.GetChild(this.transform.childCount - i).gameObject;
            cartaDistribuida = Instantiate( cartaDistribuida,handPlayer.transform, false);

        }        
        
    }
}
