using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public GameObject c_content;
    public TMP_InputField c_inputField;

    PhotonView photonview;
    GameObject c_contentText;
    string userName;


    void Start()
    {
        Screen.SetResolution(960, 600, false);
        PhotonNetwork.ConnectUsingSettings();
        c_contentText = c_content.transform.GetChild(0).gameObject;
        photonview = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && c_inputField.isFocused == false)
        {
            c_inputField.ActivateInputField();
        }
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new();
        options.MaxPlayers = 5;

        int nRandomKey = Random.Range(0, 100);
        userName = "user" + nRandomKey;

        PhotonNetwork.LocalPlayer.NickName = userName;
        PhotonNetwork.JoinOrCreateRoom("Room1", options, null);

    }

    public override void OnJoinedRoom()
    {
        AddChatMessage("connect user : " + PhotonNetwork.LocalPlayer.NickName);

    }
    public void OnEndEditEvent()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnClickSend();
        }
    }
    public void OnClickSend()
    {
        if (c_inputField.text.Length <= 60)
        {
            string strMessage = userName + " : " + c_inputField.text;

            photonview.RPC("RPC_Chat", RpcTarget.All, strMessage);
            c_inputField.text = "";
        }
        else
        {
            //ToastMsg.Instance.showMessage(1.0f);
        }


    }
    void AddChatMessage(string message)
    {
        GameObject goText = Instantiate(c_contentText, c_content.transform);

        goText.GetComponent<TextMeshProUGUI>().text = message;
        c_content.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    [PunRPC]
    void RPC_Chat(string message)
    {
        AddChatMessage(message);
    }
}
