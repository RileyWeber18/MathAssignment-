using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAssignment 
{
    public class MathQues : IComparable<MathQues>
    {
        // private data fields for a single math question
        // 2 + 3 = 5
        // leftOperand is 2
        // rightOperand is 3
        // mathOp is +
        // answer is 5
        /// <summary>
        /// These are private data fields for a single math question
        /// </summary>
        private int leftOperand;
        private string mathOp;
        private int rightOperand;
        private int answer;



        /// <summary>
        ///  public properties (provide public get() and set() methods for private instance data)
        /// </summary>
        public int LeftOperand { get; set; }
        public string MathOp { get; set; }
        public int RightOperand { get; set; }
        public int Answer { get; set; }


        /// <summary>
        ///  This is is the constructor method this is called when the object of the class is created
        /// </summary>
        /// <param name="leftOperand"></param>
        /// <param name="mathOp"></param>
        /// <param name="rightOperand"></param>
        /// <param name="answer"></param>
        // constructor method
        public MathQues(int leftOperand, string mathOp, int rightOperand, int answer)
        {
            LeftOperand = leftOperand;
            MathOp = mathOp;
            RightOperand = rightOperand;
            Answer = answer;




        }
        /// <summary>
        /// This is a public compareTo method from the IComparable interface.
        /// This compares the answer of (this) object with that of the input otherMathObj answer
        /// This returns 3 values 0 if both answers are the same
        /// -1 if 'This' object is numerically less than the otherMathObj answer
        /// 1  if 'This' object is numerically greater than the otherMathObj answer
        /// </summary>
        /// <param name="otherQues"></param>
        /// <returns></returns>
        // CompareTo() method implementation from IComparable interface
        // compares answer of (this) object with that of the input otherMathObj answer
        // returns 0 if both answers are the same
        // returns -1 if the 'this' object is numerically less than the otherMathObj answer
        // returns 1 if the 'this' object is numerically greater than the otherMathObj answer

        public int CompareTo(MathQues otherQues)
        {
            if (Answer == otherQues.Answer)
            {
                return 0;
            }
            else if (Answer < otherQues.Answer)
            {
                return -1;
            }
            else
            {
                return 1;
            }
            //return Answer.CompareTo(otherQues.Answer);
        }
        /// <summary>
        /// Sets up the data grid view in the format of the first, number the mathOp, the second number, the equals sign and the answer of the math question
        /// </summary>
        /// <returns></returns>
        public string[] GetTableArray()
        {
            string lStr = LeftOperand.ToString();
            string mathOp = MathOp.ToString();
            string RStr = RightOperand.ToString();
            string AnStr = Answer.ToString();
            string[] strArray = new string[5];
            strArray[0] = lStr;
            strArray[1] = mathOp;
            strArray[2] = RStr;
            strArray[3] = " = ";
            strArray[4] = AnStr;
            return strArray;
        }
        /// <summary>
        /// ToString, this method is an overide method to display all data for the instance 
        /// </summary>
        /// <returns></returns>
        
        // ToString() over-ride method to display all data for the instance
        // Format example 24(3 * 8)
        public override string ToString()
        {
            return Answer + "(" + LeftOperand + " " + MathOp + " " + RightOperand + ")";
        }
        /// <summary>
        /// public string method that returns LeftOperand + " " + MathOp + " " + RightOperand + " = " + Answer; 
        /// </summary>
        /// <returns></returns>
        // QuestionToSend method
        public string QuestionToSend()
        {
            return LeftOperand + " " + MathOp + " " + RightOperand + " = " + Answer; 
        }
       

    }
}
