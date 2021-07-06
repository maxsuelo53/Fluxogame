using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    [PunRPC]
    public void RPC_MostrarParaTodos()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
    }
}
