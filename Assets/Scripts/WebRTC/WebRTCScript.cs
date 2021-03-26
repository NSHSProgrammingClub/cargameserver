using System.Collections;
using UnityEngine;
using Unity.WebRTC;
using System.Collections.Generic;

public class WebRTCScript : MonoBehaviour
{
    [SerializeField] private Camera forward;
    [SerializeField] private Camera backward;
    [SerializeField] private Camera right;
    [SerializeField] private Camera left;
    [SerializeField] private Camera up;
    [SerializeField] private Camera down;

    Signaling signaling;
    private RTCPeerConnection connection;
    private RTCDataChannel dataChannel;
    List<RTCRtpSender> senders;

    private RTCOfferOptions offerOptions = new RTCOfferOptions
    {
        iceRestart = false,
        offerToReceiveAudio = true,
        offerToReceiveVideo = true
    };

    private RTCDataChannelInit dataChannelOptions=new RTCDataChannelInit(false){
    };

    private void Awake()
    {
        WebRTC.Initialize(EncoderType.Software);
    }

    /*
    1024x1024
    */
    RTCConfiguration getRTCConfiguration()
    {
        RTCConfiguration config = default;

        config.iceServers = WebRTCSettings.iceServers;

        return config;
    }

    private void Start()
    {
        signaling = gameObject.GetComponent<Signaling>();
        senders = new List<RTCRtpSender>();
        StartCoroutine(WebRTC.Update());
    }

    public void webRTCStartConnection(){
        disconnect();


        RTCConfiguration config=getRTCConfiguration();

        connection = new RTCPeerConnection(ref config);
        connection.OnIceCandidate = onIceCandidate;
        connection.OnIceConnectionChange = onIceConnectionChange;
        connection.OnNegotiationNeeded = () => { StartCoroutine(onNegotiationNeeded());};

        dataChannel = connection.CreateDataChannel("data", ref dataChannelOptions);
        addCamera(forward);
        addCamera(backward);
        addCamera(right);
        addCamera(left);
        addCamera(up);
        addCamera(down);
    }
    void addCamera(Camera cam){
        MediaStream videoStream = cam.CaptureStream(720, 720, 10_000_000);

        Debug.Log("camera added");
        foreach(var track in videoStream.GetTracks())
        {
            senders.Add(connection.AddTrack(track, null));
        }

    }
    void onIceConnectionChange(RTCIceConnectionState state)
    {
        switch (state)
        {
        case RTCIceConnectionState.New:
            Debug.Log("IceConnectionState: New");
            break;
        case RTCIceConnectionState.Checking:
            Debug.Log("IceConnectionState: Checking");
            break;
        case RTCIceConnectionState.Closed:
            Debug.Log("IceConnectionState: Closed");
            break;
        case RTCIceConnectionState.Completed:
            Debug.Log("IceConnectionState: Completed");
            break;
        case RTCIceConnectionState.Connected:
            Debug.Log("IceConnectionState: Connected");
            break;
        case RTCIceConnectionState.Disconnected:
            Debug.Log("IceConnectionState: Disconnected");
            break;
        case RTCIceConnectionState.Failed:
            Debug.Log("IceConnectionState: Failed");
            break;
        case RTCIceConnectionState.Max:
            Debug.Log("IceConnectionState: Max");
            break;
        default:
            break;
        }
    }
    void onIceCandidate(RTCIceCandidate candidate)
    {
        string candidateStr=JsonUtility.ToJson(candidate);
        Debug.Log("Ice candidate "+candidateStr);
        signaling.sendIceCandidate(candidateStr);
    }
    IEnumerator onNegotiationNeeded()
    {
        Debug.Log("onNegotiationNeeded");
        var op = connection.CreateOffer(ref offerOptions);
        yield return op;

        if (!op.IsError)
        {
            RTCSessionDescription desc=op.Desc;
            Debug.Log($"Offer creation sdp: {desc.sdp}");

            var op2 = connection.SetLocalDescription(ref desc);
            yield return op2;

            if (!op2.IsError)
            {
                Debug.Log($"Local description set.");
            }
            else
            {
                Debug.Log($"Offer set local description error: {op2.Error}");
            }

            signaling.sendSDP(desc.sdp);
        }
        else
        {
            Debug.Log($"Offer creation error: {op.Error}");
        }

    }
    public void onIceCandidateReceived(string candidateStr){
        RTCIceCandidate candidate=JsonUtility.FromJson<RTCIceCandidate>(candidateStr);
        Debug.Log("Ice candidate recieved "+candidateStr);
        connection.AddIceCandidate(ref candidate);
    }
    private void OnDestroy()
    {
        connection.Close();
        WebRTC.Dispose();
    }
    void disconnect()
    {
        foreach(var sender in senders)
        {
            connection.RemoveTrack(sender);
        }
        senders.Clear();

        if(connection!=null){

            connection.Close();
            connection = null;
        }
    }
    public IEnumerator onRemoteSDPAnswer(string sdp){
        RTCSessionDescription desc=new RTCSessionDescription {
            sdp=sdp,
            type=RTCSdpType.Answer
        };
        var op = connection.SetRemoteDescription(ref desc);
        yield return op;
        if (!op.IsError)
        {
            Debug.Log("Remote SDP answer set.");
        }
        else
        {
            Debug.Log($"SDP Answer setting error: {op.Error}");
        }
    }
}
