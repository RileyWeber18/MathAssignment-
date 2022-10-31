using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MathAssignment
{
    public partial class Instructor : Form
    {

        public const int TOTAL_ROWS = 6;
        public const int TOTAL_COLS = 5;
        public bool exitStatus = false;
        public const int BYTE_SIZE = 1024;
        public const string HOST_NAME = "localhost";
        public const int PORT_NUMBER = 8888;
        // listens for and accept incoming connection requests
        private TcpListener serverListener;
        // TcpClient is used to connect with the TcpListener object
        private TcpClient serverSocket;
        // set up a client connection for TCP network service


        // set up data stream object
        private NetworkStream netStream;
        // set up thread to run ReceiveStream() method
        private Thread serverThread = null;
        // set up delegate 
        delegate void SetTextCallback(string text);
        MathQues currentMathQues;


        private List<MathQues> mathQuesList;

        private LinkedList<MathQues> mathLinkedList;

        private BinaryTree<MathQues> btMathQuestions;

        private Hashtable MathQuesHashTable;

        

        // declare and initialise 5 math questions


        public Instructor()
        {



            // List of math questions 
            mathQuesList = new List<MathQues>();
            // Add 5 math questions to the list

            // Linked List
            mathLinkedList = new LinkedList<MathQues>();
            // add question objects from Math List - using AddLast() method

            btMathQuestions = new BinaryTree<MathQues>();



            MathQuesHashTable = new Hashtable();

            currentMathQues = null;
            















            InitializeComponent();

            // add columns to Data Grid View
            for (int count = 0; count < TOTAL_COLS; count++)
            {
                dataGridView1.Columns.Add("Column", string.Format((count).ToString()));
            }
            // add rows to Data Grid
            for (int count = 0; count < TOTAL_ROWS - 1; count++)
            {
                dataGridView1.Rows.Add();
            }
            // set row header with (for row header numbers)
            dataGridView1.RowHeadersWidth = 50;
            // set row header numbers (start with 0)
            for (int count = 0; count <= dataGridView1.Rows.Count - 1; count++)
            {
                dataGridView1.Rows[count].HeaderCell.Value =
                    string.Format((count).ToString());
            }
            // change font and font size for each cell
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 10);




            // run server
            StartServer();

            DisplayTable();




        }



        private void StartServer()
        {

            try
            {
                // create listener and start
                serverListener = new TcpListener(IPAddress.Loopback, PORT_NUMBER);
                serverListener.Start();
                // create acceptance socket
                // this creates a socket connection for the server
                serverSocket = serverListener.AcceptTcpClient();
                // create stream
                netStream = serverSocket.GetStream();
                // set up thread to run ReceiveStream() method
                serverThread = new Thread(ReceiveStream);
                // start thread
                serverThread.Start();
                // Question_TextBox.Text = "Server started ..." + Environment.NewLine;
            }
            catch (Exception e)
            {
                // display exception message

            }
        }
        private void DisplayTable()
        {
            if (mathQuesList.Count == 0)
            {
                return;
            }
            else
            {
                // first remove rows that are displayed
                dataGridView1.Rows.Clear();

                for (int i = 0; i < mathQuesList.Count; i++)
                {

                    dataGridView1.Rows.Add(mathQuesList[i].GetTableArray());

                    dataGridView1.Refresh();

                }
            }

        }

        static void DisplayGenericList<T>(List<T> genericList)
        {
            Console.WriteLine();
            foreach (T genericItem in genericList)

            {
                Console.WriteLine(genericItem.ToString());
            }
            Console.WriteLine();

        }


        static void DisplayGenericLinkedList<T>(LinkedList<T>
       genericLinkedList)
        {
            Console.WriteLine();
            int i = 1;
            foreach (T genericItem in genericLinkedList)
            {
                Console.WriteLine(i + ". " + genericItem.ToString());
                i++;
            }
            Console.WriteLine();

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
                    Console.WriteLine("Server has exited!");
                    exitStatus = true;
                }
            }
        }
        private void SetText(string text)
        {
            // InvokeRequired compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // if these threads are different, it returns true.
            if (this.First_Number_Text.InvokeRequired)
            {
                // d is a Delegate reference to the SetText() method
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                // if answer is yes
                // if (answer == "y")
                if (text.Equals("y"))
                {
                    BinaryTree_Box.Text = "Correct";
                }
                else if (text.Equals("n"))
                {
                    BinaryTree_Box.Text = "Incorrect";
                    mathLinkedList.AddLast(currentMathQues);
                }
                Send_button.Enabled = true;
                Answer_TextBox.Text = " ";
                // else if (answer == "n")
                // display in linkedList if incorrect

            }
        }

        public void DisplayLinkedList<T>(LinkedList<T> mathLinkedList)
        {
            String displayText = "";

            int i = 1;
            foreach (T currentMathQues in mathLinkedList)
            {
                displayText += "Question - " + i + ") " + " " + currentMathQues.ToString() + ". " + "\r\n";
                i++;
            }
            LinkedList_TextBox.Text = displayText;

        } // END DisplayLinkedList<T>(LinkedList<T> mathLinkedList) Method 

        private void ValidateAnswer()
        {

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

        private void Send_button_Click(object sender, EventArgs e)
        {
            try
            {



                // send message in Send_TextBox if any text present


                if (ValidateQuestion())
                {



                    // get components of question
                    int number1 = Int32.Parse(First_Number_Text.Text);
                    int number2 = Int32.Parse(Second_Number.Text);
                    string mathOp = Operator_ComboBox.SelectedItem.ToString();
                    // get answer
                    int answer = 0;
                    if (mathOp.Equals("+"))
                    {
                        answer = number1 + number2;
                    }
                    else if (mathOp.Equals("*"))
                    {
                        answer = number1 * number2;
                    }
                    else if (mathOp.Equals("/"))
                    {
                        answer = number1 / number2;
                    }
                    else if (mathOp.Equals("-"))
                    {
                        answer = number1 - number2;
                    }
                    Answer_TextBox.Text = answer.ToString();

                    // Create math object
                    currentMathQues = new MathQues(number1, mathOp, number2, answer);

                    // add instance to List
                    mathQuesList.Add(currentMathQues);


                    // add instance to BinaryTree
                    btMathQuestions.Add(currentMathQues);


                    // add instance to Hashtable
                    MathQuesHashTable.Add(currentMathQues.QuestionToSend(), currentMathQues.QuestionToSend());


                    // construct byte array to stream in write mode
                    String strToSend = currentMathQues.QuestionToSend();

                    byte[] bytesToSend = Encoding.ASCII.GetBytes(strToSend);
                    netStream.Write(bytesToSend, 0, bytesToSend.Length);

                    First_Number_Text.Text = "";
                    Second_Number.Text = "";

                    Send_button.Enabled = false;
                    DisplayTable();


                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR:Cannot divide 0 by itself");
            }







        }

        private int BinarySearch(List<MathQues> mList, string mathQToSearch)


        {

            


            int posFound = -1;
                // break up the input string "3 + 4 = 7"
                string[] mathQSplit = mathQToSearch.Split(' ');
                int answer = Int32.Parse(mathQSplit[0]);

                bool foundStatus = false;
                int first = 0;
                int last = mList.Count - 1;
                int mid;

                while (!foundStatus && first <= last)
                {
                    // mid point 
                    mid = (first + last) / 2;

                    // check if the answer is less than the mid point
                    if (answer < mList[mid].Answer)
                    {
                        last = mid - 1;
                    }
                    else if (answer > mList[mid].Answer)
                    {
                        first = mid + 1;
                    }
                    else
                    {
                        int leftOp = Int32.Parse(mathQSplit[0]);
                        int rightOp = Int32.Parse(mathQSplit[2]);
                        if (leftOp == mList[mid].LeftOperand &&
                            rightOp == mList[mid].RightOperand)
                        {
                            foundStatus = true;
                            posFound = mid;
                        }
                        else
                        {
                            foundStatus = true;
                        }
                    }

                }
                return posFound;
            


        }

        public void DisplayHashTable()
        {
            string mathQToSearch = Search_textBox.Text;
            // Console.WriteLine("Hashtable Search:");
            if (MathQuesHashTable.ContainsKey(mathQToSearch))
            {
                MessageBox.Show(mathQToSearch + " FOUND in Hashtable");
                BinaryTree_Box.Text = mathQToSearch + "Found";
            }
            else
            {
                MessageBox.Show(mathQToSearch + " NOT FOUND in Hashtable");
            }
        }



        private void PostOrder()
        {
            if (mathQuesList.Count == 0)
            {
                MessageBox.Show("No data found");
            }
            else
            {
                btMathQuestions.NodeValues = "";
                btMathQuestions.Postorder(btMathQuestions.GetRoot());
                BinaryTree_Box.Text = ("PostOrder" + btMathQuestions.NodeValues);
            }
        }private void PreOrder()
        {
            if (mathQuesList.Count == 0)
            {
                MessageBox.Show("No data found");
            }
            else
            {
                btMathQuestions.NodeValues = "";
                btMathQuestions.Preorder(btMathQuestions.GetRoot());
                BinaryTree_Box.Text = ("PreOrder" + btMathQuestions.NodeValues);
            }
        }
        private void InOrder()
        {
            if (mathQuesList.Count == 0)
            {
                MessageBox.Show("No data found");
            }
            else
            {
                btMathQuestions.NodeValues = "";
                btMathQuestions.Inorder(btMathQuestions.GetRoot());
                BinaryTree_Box.Text = ("InOrder" + btMathQuestions.NodeValues);
            }
        }






        private void SaveText(string Traversal)
        {
            {  
            SaveFileDialog  saveFileDialog = new SaveFileDialog();
            // Simplify object initialization
            saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

                if (btMathQuestions.GetCount() > 0 & saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = "";
                    filePath = saveFileDialog.FileName;

                    string strToWrite = "";
                    if (Traversal.Equals("PreOrder"))
                    {
                        strToWrite = "PreOrder: ";
                        btMathQuestions.NodeValues = "";
                        btMathQuestions.Preorder(btMathQuestions.GetRoot());
                        strToWrite += btMathQuestions.NodeValues;
                    }
                    else if (Traversal.Equals("InOrder"))
                    {
                        strToWrite = "InOrder: ";
                        btMathQuestions.NodeValues = "";
                        btMathQuestions.Inorder(btMathQuestions.GetRoot());
                        strToWrite += btMathQuestions.NodeValues;
                    }
                    else if (Traversal.Equals("PostOrder"))
                    {
                        strToWrite = "PostOrder: ";
                        btMathQuestions.NodeValues = "";
                        btMathQuestions.Postorder(btMathQuestions.GetRoot());
                        strToWrite += btMathQuestions.NodeValues;
                    }
                    FileStream f = new FileStream(filePath, FileMode.OpenOrCreate);

                 //   Console.WriteLine("File opened");

                    //declared stream writer
                    StreamWriter s = new StreamWriter(f);

                //    Console.WriteLine("Writing data to file");

                    s.WriteLine(strToWrite);

                    //closing stream writer
                    
                    s.Close();
                    f.Close();
                   
                    

                    
                }
        }
    }

    private bool ValidateQuestion()
        {
            bool questionOK = true;

           

            int num1;
            int num2;

            if (First_Number_Text.Text.Length == 0 || Second_Number.Text.Length == 0)
            {
                MessageBox.Show("Error: Both or one text field is empty");

            }
            else if (int.TryParse(First_Number_Text.Text, out int result))
                {
               
                }
            else
            {
                MessageBox.Show("Both fields are non numeric");
            }

                try
                {
                    num2 = Int32.Parse(Second_Number.Text);
                    if (MathOp.Equals("/") && num2 == 0)
                    {
                        MessageBox.Show("ERROR: You cant divide by 0");
                        
                    }

                }
                catch (Exception)
                {
                    questionOK = false;
                }

                return questionOK;
            
        }
       
        public MathQues[]  BubbleSort(MathQues[] mQArray)
        {


            for (int i = 0; i < mQArray.Length - 1; i++)
            {
                for (int j = i + 1; j < mQArray.Length; j++)
                {
                    if (mQArray[j].Answer < mQArray[i].Answer)
                    {
                        //swap values
                        MathQues temp = mQArray[j];
                        mQArray[j] = mQArray[i];
                        mQArray[i] = temp;
                    }
                }
               
            }
            return mQArray;
        }
       
        public MathQues[] SelectionSort(MathQues[] mQArray)
        {
            // decending order
            for (int i = 0; i < mQArray.Length - 1; i++)
            {
                for (int j = i + 1; j < mQArray.Length; j++)
                {
                    

                        if (mQArray[j].Answer > mQArray[i].Answer)
                        {
                            //swap values
                            MathQues temp = mQArray[j];
                            mQArray[j] = mQArray[i];
                            mQArray[i] = temp;
                        }
                    
                }
            }
            return mQArray;
        }
        public MathQues[] InsertionSort(MathQues[] mQArray)
        {
           
            for (int i = 1; i < mQArray.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                   
                        if (mQArray[j].Answer < mQArray[j - 1].Answer)
                        {
                            MathQues temp = mQArray[j];
                            mQArray[j] = mQArray[j - 1];
                            mQArray[j - 1] = temp;
                            
                        }
                    }
                    
                

            }
            return mQArray;
            
            
        }

        private void Insertion_Sort_Click(object sender, EventArgs e)
        {
            if (mathQuesList.Count == 0)
            {
                MessageBox.Show("No data found");
            }
            else
            {
                MathQues[] mathQuesArray = mathQuesList.ToArray();
                mathQuesArray = InsertionSort(mathQuesArray);
                mathQuesList = mathQuesArray.ToList<MathQues>();
                DisplayGenericList(mathQuesList);
                DisplayTable();
            }



        }



        private void Instructor_FormClosed(object sender, FormClosedEventArgs e)
        {
            // terminate thread if still running
            if (serverThread.IsAlive)
            {
                Console.WriteLine("Instructor thread is alive");
                serverThread.Interrupt();
                if (serverThread.IsAlive)
                {
                    Console.WriteLine("Instructor thread is now terminated");
                }
            }
            else
            {
                Console.WriteLine("Instructor thread is terminated");
            }

            // close the application for good
            Environment.Exit(0);

        }



        private void BinarySearch_Button_Click(object sender, EventArgs e)
        {
            
            
            string mathQToSearch = Search_textBox.Text;
            int indexPos = BinarySearch(mathQuesList, mathQToSearch);
            Console.WriteLine("Binary Search ...");
            if (indexPos >= -1)
            {
                // Console.WriteLine(mathQToSearch + "Found at index position " + indexPos);
                BinaryTree_Box.Text = (mathQToSearch + " Found at index position " + indexPos);
            }
            else
            {
                Console.WriteLine(mathQToSearch + " Not Found");
            }
            
            
        }

        private void LinkedList_Button_Click(object sender, EventArgs e)
        {
            if(mathLinkedList.Count == 0)
            {
                MessageBox.Show("Nothing present in Linked List");
            }
            else
            {
                DisplayLinkedList(mathLinkedList);

            }
           
        }

        private void HashSearch_Button_Click(object sender, EventArgs e)
        {
            DisplayHashTable();
        }
        private void Bubble_Sort_Click(object sender, EventArgs e)
        {
            
            if (mathQuesList.Count == 0)
            {
                MessageBox.Show("No data found");
            }
            else
            {
                MathQues[] mathQuesArray = mathQuesList.ToArray();
                mathQuesArray = BubbleSort(mathQuesArray);
                mathQuesList = mathQuesArray.ToList<MathQues>();
                DisplayGenericList(mathQuesList);
                DisplayTable();
            }
            
        }


            private void Selection_Sort_Click(object sender, EventArgs e)
        {
            if (mathQuesList.Count == 0)
            {
                MessageBox.Show("No data found");
            }
            else
            {
                MathQues[] mathQuesArray = mathQuesList.ToArray();
                mathQuesArray = SelectionSort(mathQuesArray);
                mathQuesList = mathQuesArray.ToList<MathQues>();
                DisplayGenericList(mathQuesList);
                DisplayTable();

            }

        }

        private void PreOrderSave_Button_Click(object sender, EventArgs e)
        {
           


                SaveText("PreOrder");
            
        }

        private void Preorder_Button_Click(object sender, EventArgs e)
        {
           
            string strToWrite = "PreOrder: ";
            btMathQuestions.NodeValues = "";
            btMathQuestions.Preorder(btMathQuestions.GetRoot());
            strToWrite += btMathQuestions.NodeValues;
            BinaryTree_Box.Text = strToWrite;
            
        }

        private void InOrder_Button_Click(object sender, EventArgs e)
        {
            
            string strToWrite = "InOrder: ";
            btMathQuestions.NodeValues = "";
            btMathQuestions.Inorder(btMathQuestions.GetRoot());
            strToWrite += btMathQuestions.NodeValues;
            BinaryTree_Box.Text = strToWrite;
        }

        private void PostOrder_Button_Click(object sender, EventArgs e)
        {
          
            string strToWrite = "PostOrder: ";
            btMathQuestions.NodeValues = "";
            btMathQuestions.Postorder(btMathQuestions.GetRoot());
            strToWrite += btMathQuestions.NodeValues;
            BinaryTree_Box.Text = strToWrite;
        }

        private void InOrderSave_Button_Click(object sender, EventArgs e)
        {
            

                SaveText("InOrder");
            
        }

        private void PostOrderSave_Button_Click(object sender, EventArgs e)
        {
          
            


                SaveText("PostOrder");
            
        }

        private void First_Number_Text_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    

