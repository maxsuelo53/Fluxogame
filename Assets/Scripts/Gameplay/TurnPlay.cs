using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPlay : MonoBehaviour
{
    
    public int turnoAtual = 0;
    public Text TextTurnoAtual;
    float destroyTime = 1.33f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    public void OneCurrentTurnPlayer()
    {
        turnoAtual = 1;
        TextTurnoAtual.text = turnoAtual.ToString() + "/2";
    }

    public void TwoCurrentTurnPlayer()
    {
        turnoAtual = 2;
        TextTurnoAtual.text = turnoAtual.ToString() + "/2";
    }

}
