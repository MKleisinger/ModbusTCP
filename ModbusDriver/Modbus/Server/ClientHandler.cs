using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using Modbus.Factories;
using Modbus.IO;
using Modbus.IO.Interfaces;

namespace Modbus.Server
{
    public class ClientHandler
    {        
        #region Fields

        private TcpClient _socketClient = null;
        private ModbusServer _serverReference = null;
        private Guid _clientID;
        private Queue<IRequest> _pendingRequests = new Queue<IRequest>();
        private Subject<IRequest> _requestStreamSubject = new Subject<IRequest>();

        #endregion

        #region Constructor(s)

        public ClientHandler(TcpClient client, ModbusServer serverRef)
        {
            _serverReference = serverRef;
            _socketClient = client;
            _clientID = Guid.NewGuid();            

            Start();
        }

        #endregion  

        #region Public Properties

        public IObservable<IRequest> RequestStream
        {
            get { return _requestStreamSubject.AsObservable(); }
        }

        #endregion

        #region Client Processing

        /// <summary>
        /// Starts the Read/Response tasks and subscribes to the 
        /// request stream.
        /// </summary>
        private void Start()
        {
            this.RequestStream.Subscribe(r => QueueRequest(r));
            StartReadTask();
            StartResponseTask();
        }

        #region Client Request Processing

        /// <summary>
        /// Starts the task to read client requests
        /// </summary>
        private void StartReadTask()
        {
            Task.Factory.StartNew(() =>
            {                
                using (NetworkStream clientStream = _socketClient.GetStream())
                {
                    while (_socketClient.Connected)
                    {
                        if (clientStream.CanRead)
                        {
                            byte[] mbapBytes = new byte[MBAPHeader.TotalBytes];
                            if (DataReceived(clientStream, mbapBytes))
                            {
                                MBAPHeader mbap = new MBAPHeader(mbapBytes);

                                // Get the rest of the data
                                byte[] requestData = new byte[mbap.Length];
                                if (DataReceived(clientStream, requestData))
                                {
                                    // Create the request
                                    IRequest request = RequestFactory.GetInstance(mbap, requestData);
                                   
                                    // Queue the pending requesT
                                    _requestStreamSubject.OnNext(request);                                    
                                }
                            }
                            
                            clientStream.Flush();
                            Task.Delay(10);
                        }
                    }
                }

                _socketClient.Close();
            });
        }

        #endregion

        #region Server Response Processing

        /// <summary>
        /// Starts the task to report the server response back to the client.
        /// </summary>
        private void StartResponseTask()
        {
            Task.Factory.StartNew(() =>
            {
                using (NetworkStream clientStream = _socketClient.GetStream())
                {
                    while (_socketClient.Connected)
                    {
                        if (_pendingRequests.Count > 0 && clientStream.CanWrite)
                        {
                            // Dequeue the request
                            IRequest request = _pendingRequests.Dequeue();

                            // Get the response bytes
                            byte[] package = _serverReference.HandleRequest(request).GetPackage();

                            // Send response to client
                            clientStream.Write(package, 0, package.Length);
                        }

                        clientStream.Flush();                        
                        Task.Delay(10);
                    }
                }
            });
        }

        #endregion


        /// <summary>
        /// Determines if data has been received from the client network stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static bool DataReceived(NetworkStream stream, byte[] buffer, int offset = 0)
        {
            return stream.Read(buffer, offset, buffer.Length) != 0;
        }

        /// <summary>
        /// Enqueues a pending request.
        /// </summary>
        /// <param name="requestObserver"></param>
        private void QueueRequest(object requestObserver)
        {
            _pendingRequests.Enqueue(requestObserver as IRequest);
        }

        #endregion  
    }
}
