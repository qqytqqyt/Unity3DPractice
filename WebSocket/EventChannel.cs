using System;
using Assets.Scripts.WebSocket.Models;
using WebSocketSharp;
using Newtonsoft.Json;
using UnityEngine;

namespace WebSocket
{
    public class EventChannel
    {
        public WebSocketSharp.WebSocket WebSocket { get; set; }

        private bool m_isConnected;

        private double m_taskDelay = 5;

        private DateTime _lastTaskTime = DateTime.Now;

        public EventChannel ()
        {
            WebSocket = new WebSocketSharp.WebSocket("ws://a.bphots.com:9601");

            WebSocket.OnOpen += (sender, e) => {
                Console.WriteLine("Connected");
                m_isConnected = true;
            };
            WebSocket.OnMessage += (sender, e) =>
            {
                Console.WriteLine(e.Data);
            };
        }
        ~EventChannel()
        {
            WebSocket.Close();
        }

        public void Connect()
        {
            Console.WriteLine("Connecting...");
            WebSocket.ConnectAsync();
        }

        public void Queue(int id)
        {
            Console.WriteLine("Queueing...");
            if (m_isConnected)
            {
                var message = new SocketMessage();
                message.Action = "playerQueue";
                message.Data = new RoomMessageData()
                {
                    PlayerId = id
                };
            
                WebSocket.SendAsync(JsonConvert.SerializeObject(message), b => Console.WriteLine("Queued"));
            }
        }

        public void Move(int playerId, int roomId, Vector3 position)
        {
            if (m_isConnected && (DateTime.Now - _lastTaskTime).Milliseconds > m_taskDelay)
            {
                _lastTaskTime = DateTime.Now;
                Console.WriteLine("Positioning...");
                if (m_isConnected)
                {
                    var message = new SocketMessage();
                    message.Action = "playerPositioning";
                    message.Data = new PositioningMessageData()
                    {
                        PlayerId = playerId,
                        RoomId = roomId,
                        Position = new PositioningMessageData.PositionInfo(position.x, position.y, position.z)
                    };

                    var json = JsonConvert.SerializeObject(message);
                    WebSocket.SendAsync(json, b => Console.WriteLine("Moved"));
                }
            }
        }
    }
}
