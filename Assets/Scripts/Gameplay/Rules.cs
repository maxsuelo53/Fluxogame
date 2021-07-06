using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
    public GameObject ImpedebuttonComprar;
    public GameObject ImpedeCartaMesa;
    public GameObject handPlayer;
    bool turnPlayer;
    // Start is called before the first frame update
    void Start()
    {
        turnPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (turnPlayer == true )
        {
            if (handPlayer.transform.childCount <= 6)
            {
                ImpedeCartaMesa.SetActive(false);
                ImpedebuttonComprar.SetActive(false);
            }
            else if (handPlayer.transform.childCount > 6){
                ImpedebuttonComprar.SetActive(true);
            }          
            

        }
        else
        {
            ImpedeCartaMesa.SetActive(true);
            ImpedebuttonComprar.SetActive(true);
        }

    }

    public void trocaTurno( )
    {
        turnPlayer = !turnPlayer;
    }
}
