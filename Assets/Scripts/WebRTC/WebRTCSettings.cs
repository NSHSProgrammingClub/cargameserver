//#define USE_PROD_SERVERS

using Unity.WebRTC;

public class WebRTCSettings {
    #if (USE_PROD_SERVERS)
    public static RTCIceServer[] iceServers= new RTCIceServer[] {
        new RTCIceServer {
            urls = new string[] { "turn:priv.larrys.tech:3478" },
            username = "test",
            credential = "test",
        },
    };
    public static string websocketURL="wss://priv.larrys.tech:8080";
    #else
    public static RTCIceServer[] iceServers = new RTCIceServer[] {
        new RTCIceServer {
            urls = new string[] { "stun:stun.l.google.com:19302" }
        },
    };
    public static string websocketURL="ws://localhost:8080";
    #endif
}
