using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace StudentApp
{
    public partial class Student : Form
    {
        public bool exitStatus = false;
        public const int BYTE_SIZE = 1024;
        public const int PORT_NUMBER = 8888;
        // listens for and accept incoming connection requests
       
        // TcpClient is used to connect with the TcpListener object

        private const string HOST_NAME = "localHost";
        private TcpClient clientSocket;
        // set up data stream object
        private NetworkStream netStream;
        // set up thread to run ReceiveStream() method
        private Thread clientThread = null;
        // set up delegate 
        delegate void SetTextCallback(string text);
        //  int answer;

        private int correctAns;
        private int studentAns;
        string num = "afbeddhsjdj";
        public Student()
        {
            InitializeComponent();
            // run server
          

            StartServer();








        }

        private void StartServer()
        {

            try
            {
                // create listener and start

                // create acceptance socket
                // this creates a socket connection for the server
                clientSocket = new TcpClient(HOST_NAME, PORT_NUMBER);

                // create stream
                netStream = clientSocket.GetStream();
                // set up thread to run ReceiveStream() method
                clientThread = new Thread(ReceiveStream);
                // start thread
                clientThread.Start();
                // Question_TextBox.Text = "Client started ..." + Environment.NewLine;
            }
            catch (Exception e)
            {
                // display exception message
                Console.WriteLine(e.StackTrace);
            }
        }
        
        public void ReceiveStream()
        {
            byte[] bytesReceived = new byte[BYTE_SIZE];
            // loop to read any incoming messages
            while (!exitStatus)
            {
                try
                {
                    int bytesRead = netStream.Read(bytesReceived, 0, bytesReceived.Length);
                    this.SetText(Encoding.ASCII.GetString(bytesReceived, 0, bytesRead));
                }
                catch (System.IO.IOException)
                {
                    Console.WriteLine("Client has exited!");
                    exitStatus = true;
                }
            }
        }
        private void SetText(string text)
        {
            // InvokeRequired compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // if these threads are different, it returns true.
            if (this.Question_Textbox.InvokeRequired)
            {
                // d is a Delegate reference to the SetText() method
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                // this.Question_TextBox.Text += text;
                string[] splitArray = text.Split(' ');
                string quesToDisplay = splitArray[0] + " " + splitArray[1] + " " + splitArray[2] + "=?";
                correctAns = Int32.Parse(splitArray[4]);
                Question_Textbox.Text = quesToDisplay;
            }
        }

        private void Submit_Button_Click(object sender, EventArgs e)
        {
           

         

            

            // send message in Send_TextBox if any text present
            if (StudentAnswer_TextBox.Text.Length > 0)
                {
                    // construct byte array to stream in write mode
                    string strToSend = "y";
                    studentAns = Int32.Parse(StudentAnswer_TextBox.Text);
                    if (studentAns != correctAns)
                    {
                        strToSend = "n";
                        MessageBox.Show("Incorrect");
                    }
                    else
                    {
                        MessageBox.Show("Correct");
                    }
                    byte[] bytesToSend = Encoding.ASCII.GetBytes(strToSend);
                    netStream.Write(bytesToSend, 0, bytesToSend.Length);
                    // Question_TextBox.Text += answerToSend + Environment.NewLine;
                    StudentAnswer_TextBox.Text = "";
                    Question_Textbox.Text = "";
                
                        
                }

            if(StudentAnswer_TextBox.Text.Length > 0)
            {

            }
            else
            {
                ValidateAnswer();
            }
            
             
           





        }
        private void Exit_Button_Click(object sender, EventArgs e)
        {

            {
                // exit the application
                if (System.Windows.Forms.Application.MessageLoop)
                {
                    // WinForms app
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    // Console app
                    System.Environment.Exit(1);
                }
            }
        }
        private void ValidateAnswer()
        {

            if (!StudentAnswer_TextBox.Text.Contains(num))
            {
                MessageBox.Show("This field only for numbers", "E.G 1,2,3,4,5...");
            }
        }
        private void Student_FormClosed(object sender, FormClosedEventArgs e)
        {
            // terminate client thread if still running
            if (clientThread.IsAlive)
            {
                Console.WriteLine("Student thread is alive");
                clientThread.Interrupt();
                if (clientThread.IsAlive)
                {
                    Console.WriteLine("Student thread is now terminated");
                }
            }
            else
            {
                Console.WriteLine("Student thread is terminated");
            }

            // close the application for good
            Environment.Exit(0);

        }
    }
    
}

