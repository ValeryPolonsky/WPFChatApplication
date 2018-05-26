using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LahoreSocketAsync
{
    public class ClientConnectedEventsArgs : EventArgs
    {
        //public string NewClient { get; set; }

        //public ClientConnectedEventsArgs(string _newClient)
        //{
        //    NewClient = _newClient;
        //}

        public string IPAddress { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }

        public ClientConnectedEventsArgs(string IPAddress,string Port,string UserName)
        {
            this.IPAddress = IPAddress;
            this.Port = Port;
            this.UserName = UserName;
        }
    }

    public class ClientDisconnectedEventsArgs : EventArgs
    {
        public List<string> connected_users{get;set;}

        public ClientDisconnectedEventsArgs(List<string> connected_users)
        {
            this.connected_users = connected_users;
        }
    }

    public class TextReceivedEventsArgs : EventArgs
    {
        public string ClientWhoSentText { get; set; }
        public string TextReceived { get; set; }

        public TextReceivedEventsArgs(string _clientWhoSentText, string _textReceived)
        {
            ClientWhoSentText = _clientWhoSentText;
            TextReceived = _textReceived;
        }
    }

    public class SocketDataTransferEventsArgs : EventArgs
    {
        public SocketDataTransfer socketDataTransfer { get; set; }

        public SocketDataTransferEventsArgs(SocketDataTransfer socketDataTransfer)
        {
            this.socketDataTransfer = socketDataTransfer;
        }
    }
}