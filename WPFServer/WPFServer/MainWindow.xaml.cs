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
        }

        private void btnStopServer_Click(object sender, RoutedEventArgs e)
        {
            mServer.StopServer();
        }

        private void btnSendToAll_Click(object sender, RoutedEventArgs e)
        {
            SocketDataTransfer socketDataTransfer = new SocketDataTransfer("server","new_message", txtMessage.Text.Trim());
            mServer.SendToAll(socketDataTransfer);
        }

        void HandleClientConnected(object sender, ClientConnectedEventsArgs ccea)
        {
            txtConsole.AppendText(string.Format("{0} - New client connected: {1}{2}", DateTime.Now, ccea.NewClient, Environment.NewLine));
        }

        void HandleSocketTransferDataReceived(object sender,SocketDataTransferEventsArgs sdtea)
        {
            proceedSocketData(sdtea.socketDataTransfer);
        }

        void HandleClientDisconnected(object sender,ClientDisconnectedEventsArgs cdea)
        {
            proceedClientDisconnectedEvent(cdea.connected_users);
        }

        //void HandleTextReceived(object sender, TextReceivedEventsArgs trea)
        //{
        //    txtConsole.AppendText(string.Format("{0} - Received from {3}: {1}{2}", DateTime.Now, trea.TextReceived, Environment.NewLine, trea.ClientWhoSentText));
        //    //mServer.SendToAll(trea.TextReceived);
        //}

        private void proceedSocketData(SocketDataTransfer socketDataTransfer)
        {
            if (socketDataTransfer.command == Globals.cmd_new_message)
            {
                txtConsole.AppendText(socketDataTransfer.user_name + ": " + socketDataTransfer.message + Environment.NewLine);
                //socketDataTransfer.tcp_client = null;
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
