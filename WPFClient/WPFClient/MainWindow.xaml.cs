using System;
using System.Collections.Generic;
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

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LahoreSocketClient client;
        private string client_name;

        public MainWindow()
        {
            client_name = "";
            InitializeComponent();
            //connectClient();
        }

        //Some very good shit added

        private void connectClient()
        {
            client = new LahoreSocketClient();
            client.RaiseSocketDataTransferEvent +=HandleSocketDataTansfer;
          
            string strIPAddress = "127.0.0.1";            
            string strPortNumber = "23000";

            if (!client.SetServerIPAddress(strIPAddress) || !client.SetPortNumber(strPortNumber))
            {
                Console.WriteLine(string.Format("Wrong IP address or port supplied - {0} - {1} - Press a key to exit", strIPAddress, strPortNumber));
                Console.ReadKey();
                return;
            }

            client.ConnectToServer();

            string strInputUser = null;

            /*do
            {
                strInputUser = Console.ReadLine();
                if (strInputUser.Trim() != "<EXIT>")
                {
                    client.SendToServer(strInputUser);
                }
                else
                {
                    client.CloseAndDisconnect();
                }

            } while (strInputUser != "<EXIT>");*/
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            SocketDataTransfer socketDataTransfer = new SocketDataTransfer(client_name,Globals.cmd_new_message,txtMessage.Text);
            client.SendToServer(socketDataTransfer);
        }

        //private  void HandleTextReceived(object sender, TextReceivedEventsArgs trea)
        //{
        //    //Console.WriteLine(string.Format("{0} - Received: {1}{2}", DateTime.Now, trea.TextReceived, Environment.NewLine));
        //    txtMessagesList.AppendText(trea.TextReceived);
        //}

        private void HandleSocketDataTansfer(object sender,SocketDataTransferEventsArgs sdtea)
        {
            //SocketDataTransfer socketDataTransfer = sdtea.socketDataTransfer;
            proceedSocketData(sdtea.socketDataTransfer);
        }

        private void btnSetName_Click(object sender, RoutedEventArgs e)
        {
            client_name = txtName.Text;
            SocketDataTransfer socketDataTransfer = new SocketDataTransfer(client_name, Globals.cmd_update_users_list, client_name);
            connectClient();
            client.SendToServer(socketDataTransfer);
        }

        private void proceedSocketData(SocketDataTransfer socketDataTransfer)
        {
            if(socketDataTransfer.command==Globals.cmd_new_message)
            {
                //txtMessagesList.AppendText(socketDataTransfer.user_name+": "+socketDataTransfer.message+Environment.NewLine);
                txtMessagesList.AppendText(string.Format("<<{0}>> - {1}: {2}{3}",DateTime.Now,socketDataTransfer.user_name,socketDataTransfer.message,Environment.NewLine));
            }
            if(socketDataTransfer.command==Globals.cmd_update_users_list)
            {
                List<string> connectedUsers = (List<string>)socketDataTransfer.obj;

               
                txtConnectedUsers.Document.Blocks.Clear();

                txtConnectedUsers.AppendText(Environment.NewLine);
                foreach (string user in connectedUsers)
                    txtConnectedUsers.AppendText(user+ Environment.NewLine);
            }
        }
    }
}
