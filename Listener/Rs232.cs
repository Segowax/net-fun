using Listener.Options;
using System.Collections.Concurrent;
using System.IO.Ports;

namespace Listener
{
    internal class Rs232 : IDisposable
    {
        private bool _isDisposed;
        private SerialPort? _serialPort;
        private readonly CancellationTokenSource _cts;
        private readonly Rs232Options _rs232Options;

        public Rs232(Rs232Options rs232Options)
        {
            _cts = new CancellationTokenSource();
            _rs232Options = rs232Options;
        }

        internal async Task Listen()
        {
            using (_serialPort = new SerialPort(_rs232Options.Port, _rs232Options.BaudRate))
            {
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                _serialPort.Open();

                try
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        await Task.Delay(100, _cts.Token);
                    }
                }
                catch (Exception) { }
                finally
                {
                    if (_serialPort != null && _serialPort.IsOpen)
                        _serialPort.Close();
                }
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string data = "";

                while (true)
                {
                    string received = sp.ReadExisting();
                    data += received;
                    if (data.Contains('@'))
                    {
                        break;
                    }
                }

                BufforToSend.rs232Data.TryAdd(Guid.NewGuid().ToString(), data.Replace("@", string.Empty));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }
                    _serialPort.Dispose();
                    _serialPort = null;
                }
                if (_cts != null)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                }
            }

            _isDisposed = true;
        }

        ~Rs232()
        {
            Dispose(false);
        }
    }
}