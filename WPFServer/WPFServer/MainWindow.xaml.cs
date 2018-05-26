using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LahoreSocketAsync;

namespace WPFServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LahoreSocketServer mServer;
        //List<string> connected_users;

        public MainWindow()
        {
            InitializeComponent();
            mServer = new LahoreSocketServer();
            mServer.RaiseClientConnectedEvent += HandleClientConnected;
            mServer.SocketDataTransferEvent += HandleSocketTransferDataReceived;
            mServer.ClientDisconnectedEvent += HandleClientDisconnected;
            //mServer.RaiseTextReceivedEvent += HandleTextReceived;
            //connected_users = new List<string>();


            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
        }

        private void btnAcceptIncomingConnections_Click(object sender, RoutedEventArgs e)
        {
            mServer.StartListeningForIncomingConnection();
            txtConsole.AppendText(string.Format("<<{0}>> - Server started listening for incoming connections{1}", DateTime.Now,Environment.NewLine));
        }

        private void btnStopServer_Click(object sender, RoutedEventArgs e)
        {
            mServer.StopServer();
            txtConsole.AppendText(string.Format("<<{0}>> - Server stopped listening for incoming connections{1}",DateTime.Now,Environment.NewLine));
        }

        private void btnSendToAll_Click(object sender, RoutedEventArgs e)
        {
            SocketDataTransfer socketDataTransfer = new SocketDataTransfer(Globals.server_name,"new_message", txtMessage.Text.Trim());
            mServer.SendToAll(socketDataTransfer);
            txtConsole.AppendText(string.Format("<<{0}>> - Message from ({1}): {2}{3}",DateTime.Now,Globals.server_name,txtMessage.Text.Trim(),Environment.NewLine));
        }

        void HandleClientConnected(object sender, ClientConnectedEventsArgs ccea)
        {         
            txtConsole.AppendText(string.Format("<<{0}>> - New client connected: Name:{1}, IP:{2}, Port:{3}{4}",DateTime.Now,ccea.UserName,ccea.IPAddress,ccea.Port,Environment.NewLine));
        }

        void HandleSocketTransferDataReceived(object sender,SocketDataTransferEventsArgs sdtea)
        {
            proceedSocketData(sdtea.socketDataTransfer);
        }

        void HandleClientDisconnected(object sender,ClientDisconnectedEventsArgs cdea)
        {
            proceedClientDisconnectedEvent(cdea.connected_users);
        }       

        private void proceedSocketData(SocketDataTransfer socketDataTransfer)
        {
            if (socketDataTransfer.command == Globals.cmd_new_message)
            {              
                txtConsole.AppendText(string.Format("<<{0}>> - Message from ({1}): {2}{3}",DateTime.Now,socketDataTransfer.user_name,socketDataTransfer.message,Environment.NewLine));
                mServer.SendToAll(socketDataTransfer);
            }

            if(socketDataTransfer.command==Globals.cmd_update_users_list)
            {    
                List<string>users= mServer.getConnectedUsers();
                socketDataTransfer.obj = users;
                mServer.SendToAll(socketDataTransfer);

                txtConnectedUsers.Document.Blocks.Clear();
                txtConnectedUsers.AppendText(Environment.NewLine);
                foreach (string user in users)
                    txtConnectedUsers.AppendText(user+Environment.NewLine);               
            }
        }

        private void proceedClientDisconnectedEvent(List<string>connected_users)
        {
            SocketDataTransfer socketDataTransfer = new SocketDataTransfer(Globals.server_name,Globals.cmd_update_users_list,"");
            socketDataTransfer.obj = connected_users;
            mServer.SendToAll(socketDataTransfer);

            txtConnectedUsers.Document.Blocks.Clear();
            txtConnectedUsers.AppendText(Environment.NewLine);
            foreach (string user in connected_users)
                txtConnectedUsers.AppendText(user + Environment.NewLine);
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            mServer.StopServer();
        }
    }
}
