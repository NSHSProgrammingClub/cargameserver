using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using NativeWebSocket;

/*
Kinda dirty bc while WebRTCScript uses Coroutines the websocket library uses await/async.
Try to use Coroutines when possible.
*/

public class Signaling : MonoBehaviour
{
    WebRTCScript webRTCScript;

    WebSocket websocket;
    /*
    Classes for deserializing signal messages
    */
    [Serializable]
    public class GenericMessage{
        public string type;
    }
    [Serializable]
    public class OpenServerMessage{
        public string type="openServer";
    }
    [Serializable]
    public class OpenServerInfoMessage{
        public string type="openServerInfo";
        public int id;
    }
    [Serializable]
    public class OpenServerAssignedMessage{
        public string type="openServerAssigned";
        public int assignedClient;
    }
    [Serializable]
    public class ServerOfferSDPMessage{
        public string type="serverOfferSDP";
        public string sdp;
    }

    [Serializable]
    public class ClientAnswerSDPMessage{
        public string type="clientAnswerSDP";
        public string sdp;
    }

    [Serializable]
    public class ServerIceCandidateMessage{
        public string type="serverIceCandidate";
        public string candidate;
    }

    [Serializable]
    public class ClientIceCandidateMessage{
        public string type="clientIceCandidate";
        public string candidate;
    }

    int serverId=-1;
    int clientId=-1;
    // Start is called before the first frame update
    async void Start()
    {
        webRTCScript = gameObject.GetComponent<WebRTCScript>();

        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("Signal websocket connection open");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Signal websocket Error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Signal websocket connection closed");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            GenericMessage genericMessage=JsonUtility.FromJson<GenericMessage>(message);
            string type=genericMessage.type;
            if(type=="openServerInfo"){
                OpenServerInfoMessage openServerInfoMessage=JsonUtility.FromJson<OpenServerInfoMessage>(message);

                serverId=openServerInfoMessage.id;

            }else if(type=="openServerAssigned"){
                OpenServerAssignedMessage openServerAssignedMessage=JsonUtility.FromJson<OpenServerAssignedMessage>(message);
                clientId=openServerAssignedMessage.assignedClient;

                webRTCScript.webRTCStartConnection();
            }else if(type=="clientAnswerSDP"){
                ClientAnswerSDPMessage clientAnswerSDPMessage=JsonUtility.FromJson<ClientAnswerSDPMessage>(message);
                StartCoroutine(webRTCScript.onRemoteSDPAnswer(clientAnswerSDPMessage.sdp));

            }else if(type=="clientIceCandidate"){
                ClientIceCandidateMessage clientIceCandidateMessage=JsonUtility.FromJson<ClientIceCandidateMessage>(message);
                webRTCScript.onIceCandidateReceived(clientIceCandidateMessage.candidate);
            }
        };
        connectToIntermediateServer();

        await websocket.Connect();

    }
    public async void sendSDP(string sdp){
        await websocket.SendText(JsonUtility.ToJson(new ServerOfferSDPMessage{sdp=sdp}));
    }
    public async void sendIceCandidate(string candidate){
        await websocket.SendText(JsonUtility.ToJson(new ServerIceCandidateMessage{candidate=candidate}));
    }
    void Update()
    {
        websocket.DispatchMessageQueue();
    }

    async void connectToIntermediateServer()
    {
        while (websocket.State != WebSocketState.Open) {
            await Task.Delay(25);
        }
        Debug.Log(JsonUtility.ToJson(new OpenServerMessage()));
        await websocket.SendText(JsonUtility.ToJson(new OpenServerMessage()));
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
