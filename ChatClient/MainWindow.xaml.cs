using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConncted = false;

        // Create an object of the host type (see ChatHost console app) in the client in order to interect with its methods
        ServiceChatClient client;

        int ID;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Initialization happens when MainWindow (client) is loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        void ConnectUser()
        {
            // If user is not connected then connect and change button text to disconnect
            if (!isConncted)
            {          
                client = new ServiceChatClient(new InstanceContext(this));
                ID = client.Connect(textBoxUserName.Text);
                textBoxUserName.IsEnabled = false; // when connected, user cannot change user name
                btnConDiscon.Content = "Disconnect";
                isConncted = true;
            }
        }

        void DisconnectUser() 
        {
            // If user is connected then disconnect and change button text to connect
            if (isConncted)
            {               
                client.Disconnect(ID);
                client = null;
                textBoxUserName.IsEnabled = true; // when disconnected, user can change user name
                btnConDiscon.Content = "Connect";
                isConncted = false;
            }
        }
        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            if (isConncted)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MessageCallBack(string msg)
        {
            // Every time server sends a message it will be displayed in List Box
            listBoxChat.Items.Add(msg);

            // Create a scroll bar to scroll down to the very last element
            listBoxChat.ScrollIntoView(listBoxChat.Items[listBoxChat.Items.Count - 1]); 
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // When user closes the window it will be automatically disconnected
            DisconnectUser();
        }

        private void textBoxMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client != null)
                {
                    client.SendMessage(textBoxMessage.Text, ID);
                    textBoxMessage.Text = string.Empty;
                }                
            }
        }

        private void textBoxUserName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
