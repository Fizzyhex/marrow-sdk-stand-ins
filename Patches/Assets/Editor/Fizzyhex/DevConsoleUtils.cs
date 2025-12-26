using System;
using UnityEditor;
using UnityEngine;

namespace Fizzyhex.MarrowSDK.StandIns
{
    public static class DevConsoleUtils
    {
        private static bool TryOpenWebSocket(out WebSocketSharp.WebSocket webSocket)
        {
            try
            {
                webSocket = new WebSocketSharp.WebSocket("ws://127.0.0.1:50152/console");
                webSocket.OnMessage += (sender, e) => Debug.Log(e.Data);
                webSocket.Connect();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                webSocket = null;
                return false;
            }
        }
        
        [MenuItem("GameObject/Stand Ins/Take Me Here")]
        private static void TakeMeHere(MenuCommand menuCommand)
        {
            var gameObject = (GameObject)menuCommand.context;

            if (!TryOpenWebSocket(out var webSocket)) return;
            var position = gameObject.transform.position;
            webSocket.Send($"teleport {position.x} {position.y} {position.z}");
            webSocket.Close();
        }
        
        // [MenuItem("GameObject/Stand Ins/Take This To Me")]
        // private static void TakeMeHere(MenuCommand menuCommand)
        // {
        //     var gameObject = (GameObject)menuCommand.context;
        //
        //     if (!TryOpenWebSocket(out var webSocket)) return;
        //     var position = gameObject.transform.position;
        //
        //     void OnWebSocketOnOnMessage(object sender, MessageEventArgs e)
        //     {
        //         webSocket.OnMessage -= OnWebSocketOnOnMessage;
        //         
        //         var split = e.Data.Split(" ");
        //         var coords = new List<float>();
        //         split.ToList().ForEach(x => coords.Add(float.Parse(x)));
        //
        //         var oldPosition = new Vector3(coords[0], coords[1], coords[2]);
        //     }
        //
        //     webSocket.OnMessage += OnWebSocketOnOnMessage;
        //     webSocket.Send($"whereami");
        // }
    }
}