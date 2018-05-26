using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace LahoreSocketAsync
{
    public class LahoreSocketServer
    {
        IPAddress mIP;
        int mPort;
        TcpListener mTCPListener;

        //List<TcpClient> mClients;
        Dictionary<string, TcpClient> connected_user_to_socket_map;

        public EventHandler<ClientConnectedEventsArgs> RaiseClientConnectedEvent;
        public EventHandler<SocketDataTransferEventsArgs> SocketDataTransferEvent;
        public EventHandler<ClientDisconnectedEventsArgs> ClientDisconnectedEvent;
        //public EventHandler<TextReceivedEventsArgs> RaiseTextReceivedEvent;

        public bool KeepRunning { get; set; }

        public LahoreSocketServer()
        {
            //mClients = new List<TcpClient>();
            connected_user_to_socket_map = new Dictionary<string, TcpClient>();
        }

        protected virtual void OnRaiseClientConnectedEvent(ClientConnectedEventsArgs e)
        {
            EventHandler<ClientConnectedEventsArgs> handler = RaiseClientConnectedEvent;

            if(handler!=null)
            {
                handler(this,e);
            }
        }

        protected virtual void OnRaiseSocketDataTransferEvent(SocketDataTransferEventsArgs sdtea)
        {
            EventHandler<SocketDataTransferEventsArgs> handler = SocketDataTransferEvent;

            if(handler!=null)
            {
                handler(this,sdtea);
            }
        }

        protected virtual void OnRaiseClientDisconnectedEvent(ClientDisconnectedEventsArgs cdea)
        {
            EventHandler<ClientDisconnectedEventsArgs> handler = ClientDisconnectedEvent;

            if(handler!=null)
            {
                handler(this,cdea);
            }
        }



        //protected virtual void OnRaiseTextReceivedEvent(TextReceivedEventsArgs trea)
        //{
        //    EventHandler<TextReceivedEventsArgs> handler = RaiseTextReceivedEvent;

        //    if (handler != null)
        //    {
        //        handler(this, trea);
        //    }
        //}

        public async void StartListeningForIncomingConnection(IPAddress ipaddr = null, int port = 23000)
        {
            if (ipaddr == null)
                ipaddr = IPAddress.Any;
            if (port <= 0)
                port = 23000;

            mIP = ipaddr;
            mPort = port;

            Debug.WriteLine(string.Format("IP Address: {0} - Port: {1}", mIP.ToString(), mPort));

            mTCPListener = new TcpListener(mIP, mPort);

            try
            {
                mTCPListener.Start();

                KeepRunning = true;
                while (KeepRunning)
                {
                    TcpClient returnedByAccept = await mTCPListener.AcceptTcpClientAsync();

                    //mClients.Add(returnedByAccept);
                    Debug.WriteLine("Client connected successfully, count: {0} - {1}",connected_user_to_socket_map.Count,returnedByAccept.Client.RemoteEndPoint);

                    TakeCareOfTCPClient(returnedByAccept);                   

                    
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        

        }

        public void StopServer()
        {
            try
            {
                if(mTCPListener!=null)
                    mTCPListener.Stop();

                //foreach(TcpClient c in mClients)
                //{
                //    c.Close();
                //}

                //mClients.Clear();

                List<string> users = connected_user_to_socket_map.Keys.ToList();
                foreach(string user in users)
                {
                    connected_user_to_socket_map[user].Close();
                }
                connected_user_to_socket_map.Clear();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        //private async void TakeCareOfTCPClient(TcpClient paramClient)
        //{
        //    NetworkStream stream = null;
        //    StreamReader reader = null;

        //    try
        //    {
        //        stream = paramClient.GetStream();
        //        reader = new StreamReader(stream);

        //        char[] buff = new char[64];

        //        while(KeepRunning)
        //        {
        //            Debug.WriteLine("*** Ready to read");
        //            int nRet=await reader.ReadAsync(buff,0,buff.Length);
        //            System.Diagnostics.Debug.WriteLine("Returned: "+nRet);

        //            if(nRet==0)
        //            {
        //                RemoveveClient(paramClient);
        //                Debug.WriteLine("Socket disonnected");
        //                break;
        //            }

        //            string receivedText = new string(buff);

        //            Debug.WriteLine("*** RECEIVED: "+receivedText);

        //            OnRaiseTextReceivedEvent(new TextReceivedEventsArgs(paramClient.Client.RemoteEndPoint.ToString(),receivedText));
        //            Array.Clear(buff,0,buff.Length);

        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        RemoveveClient(paramClient);
        //        Debug.WriteLine(ex.ToString());
        //    }

        //}


        private async void TakeCareOfTCPClient(TcpClient paramClient)
        {
            NetworkStream stream = null;
            StreamReader reader = null;

            try
            {
                stream = paramClient.GetStream();
                reader = new StreamReader(stream);              

                byte[] buff = new byte[1024];

                while (KeepRunning)
                {
                    Debug.WriteLine("*** Ready to read");
                    int nRet = await stream.ReadAsync(buff, 0, buff.Length);
                    System.Diagnostics.Debug.WriteLine("Returned: " + nRet);

                    if (nRet == 0)
                    {
                        RemoveClient(paramClient);
                        Debug.WriteLine("Socket disonnected");
                        break;
                    }

                    SocketDataTransfer socketDataTransfer = ByteArrayToObject(buff,nRet);
                    if(socketDataTransfer.command==Globals.cmd_update_users_list)
                    {
                        connected_user_to_socket_map[socketDataTransfer.user_name] = paramClient;

                        string UserIPAddress, UserPort, UserName;
                        ClientConnectedEventsArgs eaClientConnected;
                        UserIPAddress = ((IPEndPoint)paramClient.Client.RemoteEndPoint).Address.ToString();
                        UserPort = ((IPEndPoint)paramClient.Client.RemoteEndPoint).Port.ToString();
                        UserName = socketDataTransfer.user_name;

                        //eaClientConnected = new ClientConnectedEventsArgs(paramClient.Client.RemoteEndPoint.ToString());
                        eaClientConnected = new ClientConnectedEventsArgs(UserIPAddress, UserPort, UserName);
                        OnRaiseClientConnectedEvent(eaClientConnected);                      
                    }
                    OnRaiseSocketDataTransferEvent(new SocketDataTransferEventsArgs(socketDataTransfer));
                    Array.Clear(buff, 0, buff.Length);

                }
            }
            catch (Exception ex)
            {
                RemoveClient(paramClient);
                Debug.WriteLine(ex.ToString());
            }

        }

        private void RemoveClient(TcpClient paramClient)
        {
            //if(mClients.Contains(paramClient))
            //{
            //    mClients.Remove(paramClient);               
            //    Debug.WriteLine(string.Format("Client removed,count: {0}",mClients.Count));               
            //}

            List<string> users = connected_user_to_socket_map.Keys.ToList();
            foreach (string user in users)
            {
                if (connected_user_to_socket_map[user] == paramClient)
                {
                    connected_user_to_socket_map.Remove(user);
                    break;
                    //return;
                }
            }

            OnRaiseClientDisconnectedEvent(new ClientDisconnectedEventsArgs(getConnectedUsers()));
            //SocketDataTransfer socketDataTransfer = new SocketDataTransfer(Globals.server_name,Globals.cmd_update_users_list,"");
            //socketDataTransfer.obj = getConnectedUsers();
            //SendToAll(socketDataTransfer);
        }

        //public void RemoveClient(string user_name)
        //{
        //    if(user_to_socket_map.ContainsKey(user_name))
        //    {
        //        TcpClient client = user_to_socket_map[user_name];
        //        connected_users.Remove(user_name);
        //        RemoveClient(client);
        //        user_to_socket_map.Remove(user_name);
        //    }
        //}

        //public List<string> getConnectedUsers()
        //{
        //    return connected_users;
        //}

        //public void addNewUserName(string user_name,TcpClient client)
        //{
        //    if (!connected_users.Contains(user_name))
        //        connected_users.Add(user_name);

        //    user_to_socket_map[user_name] = client;
        //}



        //public async void SendToAll(string leMessage)
        //{
        //    if(string.IsNullOrEmpty(leMessage))
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        byte[] buffMessage = Encoding.ASCII.GetBytes(leMessage);

        //        foreach(TcpClient c in mClients)
        //        {
        //           c.GetStream().WriteAsync(buffMessage, 0, buffMessage.Length);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}

        public string getIP()
        {
            return mIP.ToString();
        }

        public string getPort()
        {
            return mPort.ToString();
        }

        public List<string>getConnectedUsers()
        {
            return connected_user_to_socket_map.Keys.ToList();
        }


        public async void SendToAll(SocketDataTransfer socketDataTransfer)
        {
            if (socketDataTransfer==null)
            {
                return;
            }

            try
            {
                byte[] buffMessage = ObjectToByteArray(socketDataTransfer);

                //foreach (TcpClient c in mClients)
                //{
                //    c.GetStream().WriteAsync(buffMessage, 0, buffMessage.Length);
                //}

                List<string> users = connected_user_to_socket_map.Keys.ToList();
                foreach(string user in users)
                {
                    connected_user_to_socket_map[user].GetStream().WriteAsync(buffMessage, 0, buffMessage.Length);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private SocketDataTransfer ByteArrayToObject(byte[] arrBytes, int buff_length)
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
    }
}
