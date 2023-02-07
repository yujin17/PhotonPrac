using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    public TMP_Text TitleText;
    public TMP_InputField ID_Input;
    public GameObject IDInputUI ,BeforeCon, Coning, AfterCon;


    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    public void OnClickStart()
    {
        Invoke("ConnectServer", 1.0f);
    }

    private IEnumerator ConnectServer()
    {
        yield return PhotonNetwork.ConnectUsingSettings();
        if (PhotonNetwork.IsConnected)
        {
            IDInputUI.SetActive(true);
            Debug.Log("연결됨");
        }
        else
        {
            Application.Quit();
        }
    }

    public void OnClickEnter()
    {
        if (ID_Input.text.Length > 1 && ID_Input.text.Length < 6)
        {
            BeforeCon.SetActive(false);
            AfterCon.SetActive(true);
            PhotonNetwork.NickName = ID_Input.text;
            PhotonNetwork.JoinLobby();
        }
        else
        {
            TitleText.text = "닉네임은 2글자 이상 6글자 이하";
        }
    }
    public override void OnJoinedLobby()
    {
        RoomOptions options = new();
        options.MaxPlayers = 5;
        PhotonNetwork.JoinOrCreateRoom("Room" + (PhotonNetwork.CountOfRooms + 1).ToString(), options, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", Vector2.zero, Quaternion.identity);
        base.OnJoinedRoom();
    }

}
