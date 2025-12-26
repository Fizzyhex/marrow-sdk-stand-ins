using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

namespace Fizzyhex.MarrowSDK.StandIns
{
    public static class DevConsoleUtils
    {
        private static readonly ConcurrentQueue<Action> MainThreadQueue = new();

        private static void OpenMainThreadQueue()
        {
            EditorApplication.update += OpenMainThreadQueueUpdate;
        }
        
        private static void OpenMainThreadQueueUpdate()
        {
            var hasItems = false;
            
            while (MainThreadQueue.TryDequeue(out var action))
            {
                hasItems = true;
                try
                {
                    Debug.Log("invoking on main thread");
                    action.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            if (hasItems)
                EditorApplication.update -= OpenMainThreadQueueUpdate;
        }
        
        private static bool TryOpenWebSocket(out WebSocketSharp.WebSocket webSocket)
        {
            try
            {
                webSocket = new WebSocket("ws://127.0.0.1:50152/console");
                //webSocket.OnMessage += (sender, e) => Debug.Log(e.Data);
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
        
        [MenuItem("CONTEXT/Transform/Stand Ins/Take This To Me")]
        private static void TakeThisToMe(MenuCommand menuCommand)
        {
            var transform = (Transform)menuCommand.context;
            if (!TryOpenWebSocket(out var webSocket)) return;

            OpenMainThreadQueue();
            webSocket.OnMessage += OnWebSocketOnOnMessage;
            webSocket.Send("whereami");
            Debug.Log("waiting for response");
            
            return;

            // [!] HEY!!
            // by running as a subscriber of OnMessage, we exit the main thread.
            // this means we lose access to unity apis, accessing them will throw silent exceptions.
            // in other words: we can't update our game object's transform inside of this method.
            //
            // so instead we enqueue the action to be executed on the main thread. i decided
            // to subscribe/unsubscribe from EditorApplication.update when unneeded, since
            // it feels plain wrong to take up any frame performance with such a tiny utility that
            // probably won't see much use.
            // - fizzy
            void OnWebSocketOnOnMessage(object sender, MessageEventArgs e)
            {
                Debug.Log("on msg");
                
                if (!e.Data.StartsWith("whereami: teleport"))
                    return;

                webSocket.OnMessage -= OnWebSocketOnOnMessage;
                var stringCoords = e.Data.Replace("whereami: teleport ", "");
                Debug.Log("hey");
                
                var split = stringCoords.Split(" ");
                var coords = new List<float>();
                split.ToList().ForEach(x => coords.Add(float.Parse(x)));
 
                Debug.Log($"received msg {string.Join(" & ", coords)}");
                
                MainThreadQueue.Enqueue(() =>
                {
                    var playerPosition = new Vector3(coords[0], coords[1], coords[2]);
                    Undo.RecordObject(transform, "Take This To Me");
                    transform.position = playerPosition;
                    Debug.Log($"taken {transform} to {playerPosition}");
                });
                
                webSocket.Close();
            }
        }
    }
}