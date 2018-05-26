using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LahoreSocketAsync
{
    [Serializable]
    public class SocketDataTransfer
    {
        public string user_name;
        public string command;
        public string message;
        public Object obj;
        //public TcpClient tcp_client;

        public SocketDataTransfer(string user_name,string command,string message)
        {
            this.user_name = user_name;
            this.command = command;
            this.message = message;
        }     
                
    }
}
