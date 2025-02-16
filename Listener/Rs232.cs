using System.Collections.Concurrent;
using System.IO.Ports;
using static Listener.Common;

namespace Listener
{
    internal class Rs232 : IDisposable
    {
        internal ConcurrentDictionary<string, string> rs232Data = [];
        private SerialPort? _serialPort;
        private CancellationTokenSource _cts;
        private bool _isDisposed;

        public Rs232()
        {
            _cts = new CancellationTokenSource();
        }

        internal async Task Listen()
        {
            using (_serialPort = new SerialPort(Rs232Configurations.PortName, Rs232Configurations.BaudRate))
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

                rs232Data.TryAdd(Guid.NewGuid().ToString(), data.Replace("@", string.Empty));
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

        internal virtual void Dispose(bool disposing)
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
