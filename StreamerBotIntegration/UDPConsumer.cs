#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace StreamerBotIntegration
{
    public class UDPConsumer
    {
        public int UdpPort { get; set; }
        public ConsolePrint ConsolePrintDelegate { get; set; } = Debug.Log;

        private Thread? _receiveThread;
        private UdpClient? _udpClient;
        private CancellationTokenSource? _cancellationTokenSource;
        private Dictionary<string, StreamerBotEvent> _eventHandlers = new();
        private static readonly ConcurrentQueue<UDPEventData>? _eventQueue = new();

        public delegate void StreamerBotEvent(UDPEventData eventData);
        public delegate void ConsolePrint(string output);
        
        public void Init()
        {
            ConsolePrintDelegate($"Initialising UDP Consumer at port {UdpPort}.");
            
            if (_receiveThread == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                CancellationToken token = _cancellationTokenSource.Token;
                _receiveThread = new Thread(() => ReceiveData(token)) // Running continuously in the background
                {
                    IsBackground = true
                };
                _receiveThread.Start();
            }
            else
            {
                ConsolePrintDelegate("UDP Consumer thread already running.");
            }

            _eventHandlers = new Dictionary<string, StreamerBotEvent>();
        }
        
        public void RegisterEvent(string eventType, StreamerBotEvent action)
        {
            if (!_eventHandlers.TryAdd(eventType, action))
            {
                _eventHandlers[eventType] += action;
            }
        }
        
        private void ProcessEvent(UDPEventData eventData)
        {
            if (eventData?.Event == null) return;
            
            if (_eventHandlers.TryGetValue(eventData.Event, out StreamerBotEvent? handler))
            {
                handler?.Invoke(eventData);
            }
            else
            {
                ConsolePrintDelegate($"No action is registered for event: \"{eventData.Event}\"");
            }
        }
        
        public void CloseConnection()
        {
            if (_receiveThread != null)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
            
            _receiveThread = null;
            _udpClient?.Close();
        }
        
        private void ReceiveData(CancellationToken token)
        {
            using (_udpClient = new UdpClient(UdpPort))
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                        
                        byte[] receivedData = _udpClient.Receive(ref anyIP);
                        string encodedData = Encoding.UTF8.GetString(receivedData);
                        
                        UDPEventData? newEvent = UDPEventData.DeserializeJson(encodedData);
                        
                        if (newEvent != null)
                        {
                            _eventQueue?.Enqueue(newEvent);
                        }
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
                    {
                        ConsolePrintDelegate("UDP consumer thread interrupted.");
                    }
                    catch (Exception err)
                    {
                        ConsolePrintDelegate(err.ToString());
                    }
                }
            }

            ConsolePrintDelegate("UDP consumer thread stopped.");
        }
        

        public void ProcessEventQueue()
        {
            if (_eventQueue == null) return;
            
            while (_eventQueue.TryDequeue(out UDPEventData? newEvent))
            {
                if (newEvent != null)
                {
                    ProcessEvent(newEvent);
                }
            }
        }
    }
}