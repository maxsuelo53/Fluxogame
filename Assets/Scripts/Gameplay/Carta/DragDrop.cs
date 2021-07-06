using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using System;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform handReturn = null;
    public Transform placeHolderParent = null;

    GameObject placeHolder = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        handReturn = this.transform.parent;
        placeHolderParent = handReturn;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;

        if (placeHolder.transform.parent != placeHolderParent)
            placeHolder.transform.SetParent(placeHolderParent);

        int newSiblingIndex = placeHolderParent.childCount;

        for (int i = 0; i < placeHolderParent.childCount; i++){
            if (this.transform.position.x < placeHolderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeHolder.transform.GetSiblingIndex()< newSiblingIndex)
                    newSiblingIndex--;
                
                break;
            }
        }

        placeHolder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(handReturn);
        this.transform.SetSiblingIndex( placeHolder.transform.GetSiblingIndex() );
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent.name == "Lixeira")
        {
            JogandoCartaNoLixo();
        }
        Destroy(placeHolder);
    }

    private void JogandoCartaNoLixo()
    {
        
        GetComponent<PhotonView>().RPC("RPC_AtivaPhotonTransformView", RpcTarget.All);
        GetComponent<PhotonView>().RPC("RPC_SetCardLocal", RpcTarget.All, "Lixeira");
        GetComponent<PhotonView>().RPC("RPC_SetFlipCardForAll", RpcTarget.All);
        transform.position = GameObject.Find("Lixeira").transform.position;        
        //GameObject.Find("Lixeira").GetComponent<DropZone>().enabled = false;
        GameObject buttonEndTurn = GameObject.Find("ButtonEndTurn");
        GameObject ControllerGame = GameObject.Find("GameManager");

        ControllerGame.GetComponent<GameManager>().VerificaNumCartaHandPlayer();

        //buttonEndTurn.GetComponent<Button>().onClick.RemoveAllListeners();
        //buttonEndTurn.GetComponent<Button>().onClick.AddListener(ControllerGame.GetComponent<GameManager>().buttonChangeTurn );
        GetComponent<DragDrop>().enabled = false;

    }
}
