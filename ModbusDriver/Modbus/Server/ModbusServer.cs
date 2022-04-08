using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Modbus.IO.Interfaces;
using Modbus.IO;
using Modbus.Tables;
using Modbus.Enums;
using System.Configuration;

namespace Modbus.Server {
    public class ModbusServer {
        #region Fields        

        private ushort _port = Convert.ToUInt16(ConfigurationManager.AppSettings["Port"]);
        private bool _isListening = false;
        private TcpListener _listener = null;
        private Task _listenTask = null;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private CancellationToken _cancelToken;

        private static ManualResetEvent _clientConnectedEvent = new ManualResetEvent(false);

        #region Modbus Tables

        private CoilsTable _coils = new CoilsTable();
        private DiscreteInputsTable _discreteInputs = new DiscreteInputsTable();
        private HoldingRegistersTable _holdingRegisters = new HoldingRegistersTable();
        private InputRegistersTable _inputRegisters = new InputRegistersTable();

        #endregion

        #endregion

        #region Constructor(s)

        public ModbusServer(ushort port, bool startOnInit = true) {
            _cancelToken = _tokenSource.Token;

            _port = port;

            if (startOnInit) {
                this.StartListening();
            }

            _coils.NotificationStream.Subscribe(args => NotifyChanged(args));
            _discreteInputs.NotificationStream.Subscribe(args => NotifyChanged(args));
            _holdingRegisters.NotificationStream.Subscribe(args => NotifyChanged(args));
            _inputRegisters.NotificationStream.Subscribe(args => NotifyChanged(args));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Specifies the port that the server will listen on.
        /// Default port is 502 for Modbus.
        /// </summary>
        public ushort Port {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// Indicates whether or not the server is currently running.
        /// </summary>
        public bool IsListening {
            get { return _isListening; }
        }

        #endregion        

        #region Notification Stream

        private Subject<object> _notificationSubject = new Subject<object>();
        public IObservable<object> NotificationStream {
            get { return _notificationSubject.AsObservable(); }
        }

        #endregion  

        #region TCP/IP

        /// <summary>
        /// Starts listening for client connections on the specified port.
        /// </summary>
        public void StartListening() {
            _listener = new TcpListener(IPAddress.Any, this.Port);

            _listenTask = Task.Factory.StartNew(() => {
                _listener.Start();
                _isListening = true;

                while (_listener != null) {
                    if (_cancelToken.IsCancellationRequested) {
                        _cancelToken.ThrowIfCancellationRequested();
                    }

                    _listener.BeginAcceptTcpClient(AcceptClientCallback, _listener);

                    _clientConnectedEvent.WaitOne();
                    _clientConnectedEvent.Reset();

                    Task.Delay(10);
                }
            }, _tokenSource.Token);
        }

        /// <summary>
        /// Stops listening for client connections.
        /// </summary>
        public void StopListening() {
            if (_listener != null) {
                _tokenSource.Cancel();

                _listener.Stop();
            }

            _listener = null;
            _isListening = false;
        }

        /// <summary>
        /// Create a ClientHandler for inbound clients to handle the communcation layer.
        /// </summary>
        /// <param name="result"></param>
        public void AcceptClientCallback(IAsyncResult result) {
            try {
                TcpListener listener = (TcpListener)result.AsyncState;
                TcpClient client = listener.EndAcceptTcpClient(result);

                ClientHandler handler = new ClientHandler(client, this);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            finally {
                _clientConnectedEvent.Set();
            }
        }

        #endregion

        #region Processing

        /// <summary>
        /// Passes the request to the appropriate table for processing.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal IResponse HandleRequest(IRequest request) {
            IResponse serverResponse = null;
            switch ((FunctionCode)request.FunctionCode) {
                case FunctionCode.ReadCoils:
                case FunctionCode.WriteSingleCoil:
                case FunctionCode.WriteMultipleCoils:
                    serverResponse = _coils.ProcessRequest(request);
                    break;
                case FunctionCode.ReadDiscreteInputs:
                    serverResponse = _discreteInputs.ProcessRequest(request);
                    break;
                case FunctionCode.ReadHoldingRegisters:
                case FunctionCode.WriteSingleRegister:
                case FunctionCode.WriteMultipleRegisters:
                    serverResponse = _holdingRegisters.ProcessRequest(request);
                    break;
                case FunctionCode.ReadInputRegisters:
                    serverResponse = _inputRegisters.ProcessRequest(request);
                    break;
                default:
                    _notificationSubject.OnNext
                    (
                        new InvalidOperationException(string.Format("Function Code {0} is not supported.", request.FunctionCode))
                    );

                    break;
            }

            return serverResponse;
        }

        /// <summary>
        /// Send address changed arguments to subscribed clients.
        /// </summary>
        /// <param name="args"></param>
        public void NotifyChanged(AddressChangedArgs args) {
            _notificationSubject.OnNext(args);
        }

        #endregion        
    }
}
