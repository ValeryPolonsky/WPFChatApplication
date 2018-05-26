using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace LahoreSocketAsync
{
    public class LahoreSocketClient
    {
        IPAddress mServerIPAddress;
        int mServerPort;
        TcpClient mClient;

        //public EventHandler<TextReceivedEventsArgs> RaiseTextReceivedEvent;
        public EventHandler<SocketDataTransferEventsArgs> RaiseSocketDataTransferEvent;


        public LahoreSocketClient()
        {
            mClient = null;
            mServerPort = -1;
            mServerIPAddress = null;
        }

        public IPAddress ServerIPAddress
        {
            get
            {
                return mServerIPAddress;
            }
        }

        public int ServerPort
        {
            get
            {
                return mServerPort;
            }
        }

        public bool SetServerIPAddress(string _IPAddressServer)
        {
            IPAddress ipaddr = null;

            if(!IPAddress.TryParse(_IPAddressServer,out ipaddr))
            {
                Console.WriteLine("Invalid server IP supplied.");
                return false;
            }

            mServerIPAddress = ipaddr;
            return true;
        }

        public static object ResolveHostNameToIPAddress(string strHostName)
        {
            IPAddress[] retAddr = null;

            try
            {
                retAddr=Dns.GetHostAddresses(strHostName);

                foreach(IPAddress addr in retAddr)
                {
                    if(addr.AddressFamily==AddressFamily.InterNetwork)
                    {
                        return addr;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public bool SetPortNumber(string _ServerPort)
        {
            int portNumber = 0;
            
            if(!int.TryParse(_ServerPort.Trim(),out portNumber))
            {
                Console.WriteLine("Invalid port number supplied");
                return false;
            }

            if(portNumber<=0 || portNumber>65535)
            {
                Console.WriteLine("Port number must be between 0 and 65535");
                return false;
            }

            mServerPort = portNumber;

            return true;
        }

        public void CloseAndDisconnect()
        {
            if (mClient != null)
            {
                if (mClient.Connected)
                {
                    mClient.Close();
                }
            }
        }

        //public async Task SendToServer(string strInputUser)
        //{
        //    if(string.IsNullOrEmpty(strInputUser))
        //    {
        //        Console.WriteLine("Empty string supplied to send");
        //        return;
        //    }

        //    if(mClient!=null)
        //    {
        //        if(mClient.Connected)
        //        {
        //            StreamWriter clientStreamWriter = new StreamWriter(mClient.GetStream());
        //            clientStreamWriter.AutoFlush = true;

        //            await clientStreamWriter.WriteAsync(strInputUser);
        //            Console.WriteLine("Data sent...");
        //        }
        //    }
        //}

        public async Task SendToServer(SocketDataTransfer socketDataTransfer)
        {
            if (socketDataTransfer==null)
            {
                Console.WriteLine("Empty data supplied to send");
                return;
            }

            if (mClient != null)
            {
                if (mClient.Connected)
                {
                    byte[] buffMessage = ObjectToByteArray(socketDataTransfer);                 
                    await mClient.GetStream().WriteAsync(buffMessage, 0, buffMessage.Length);                      
                    Console.WriteLine("Data sent...");
                }
            }
        }




        public async Task ConnectToServer()
        {
            if(mClient==null)
            {
                mClient = new TcpClient();
            }

            try
            {
                await mClient.ConnectAsync(mServerIPAddress,mServerPort);
                Console.WriteLine(string.Format("Connected to server IP/Port: {0}/{1}",mServerIPAddress,mServerPort));

                ReadDataAsync(mClient);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //private async Task ReadDataAsync(TcpClient mClient)
        //{
        //    try
        //    {
        //        StreamReader clientStreamReader = new StreamReader(mClient.GetStream());
        //        char[] buff = new char[64];
        //        int readByteCount = 0;

        //        while(true)
        //        {
        //            readByteCount=await clientStreamReader.ReadAsync(buff,0,buff.Length);

        //            if(readByteCount<=0)
        //            {
        //                Console.WriteLine("Disconnect from server");
        //                mClient.Close();
        //                break;
        //            }

        //            Console.WriteLine(string.Format("Received bytes: {0} - Message: {1}",readByteCount,new string(buff)));                    
        //            OnRaiseTextReceivedEvent(new TextReceivedEventsArgs(mClient.Client.RemoteEndPoint.ToString(), new string(buff)));
        //            Array.Clear(buff, 0, buff.Length);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());

        //    }
        //}

        private async Task ReadDataAsync(TcpClient mClient)
        {
            try
            {
                //StreamReader clientStreamReader = new StreamReader(mClient.GetStream());
                byte[] buff = new byte[1024];
                int readByteCount = 0;

                while (true)
                {
                    readByteCount = await mClient.GetStream().ReadAsync(buff, 0, buff.Length);

                    if (readByteCount <= 0)
                    {
                        Console.WriteLine("Disconnect from server");
                        mClient.Close();
                        break;
                    }

                    SocketDataTransfer received_data = ByteArrayToObject(buff, readByteCount);
                    //Console.WriteLine(string.Format("Received bytes: {0} - Message: {1}", readByteCount, new string(buff)));
                    //OnRaiseTextReceivedEvent(new TextReceivedEventsArgs(mClient.Client.RemoteEndPoint.ToString(), new string(buff)));
                    OnRaiseSocketDataTransferEvent(new SocketDataTransferEventsArgs(received_data));

                    Array.Clear(buff, 0, buff.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
        }

        private SocketDataTransfer ByteArrayToObject(byte[] arrBytes,int buff_length)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, buff_length);
            memStream.Seek(0, SeekOrigin.Begin);
            SocketDataTransfer obj = (SocketDataTransfer)binForm.Deserialize(memStream);

            return obj;
        }

        private byte[] ObjectToByteArray(SocketDataTransfer obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }


        protected virtual void OnRaiseSocketDataTransferEvent(SocketDataTransferEventsArgs sdtea)
        {
            EventHandler<SocketDataTransferEventsArgs> handler = RaiseSocketDataTransferEvent;

            if(handler!=null)
            {
                handler(this, sdtea);
            }
        }


        //protected virtual void OnRaiseTextReceivedEvent(TextReceivedEventsArgs trea)
        //{
        //    EventHandler<TextReceivedEventsArgs> handler = RaiseTextReceivedEvent;

        //    if(handler!=null)
        //    {
        //        handler(this,trea);
        //    }
        //}
       
    }
}
