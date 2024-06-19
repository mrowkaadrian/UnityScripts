using UnityEngine;
using Debug = UnityEngine.Debug;

namespace StreamerBotIntegration
{
    public class UDPEventManager : MonoBehaviour
    {
        public int port = 5501;

        private readonly UDPConsumer _udpConsumer = new UDPConsumer();

        public void OnEnable()
        {
            _udpConsumer.UdpPort = port;
            _udpConsumer.ConsolePrintDelegate = Debug.Log;
            _udpConsumer.Init();

            RegisterEvents();
        }
        
        public void Update()
        {
            _udpConsumer.ProcessEventQueue();
        }
        
        private void OnDisable()
        {
            _udpConsumer.CloseConnection();
        }

        private void OnApplicationQuit()
        {
            _udpConsumer.CloseConnection();
        }
        
        private void RegisterEvents()
        {
            _udpConsumer.RegisterEvent("AddPlayer", SpawnPlayer);
        }

        private void SpawnPlayer(UDPEventData requestData)
        {
            GetComponent<Spawner>().SpawnPlayer(requestData);
        }
    }
}