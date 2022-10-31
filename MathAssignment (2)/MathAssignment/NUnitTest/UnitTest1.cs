using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NUnitTest
{
    public class Tests
    {
        public class MathQues : IComparable<MathQues>
        {
            // private data fields for a single math question
            // 2 + 3 = 5
            // leftOperand is 2
            // rightOperand is 3
            // mathOp is +
            // answer is 5

            private int leftOperand;
            private string mathOp;
            private int rightOperand;
            private int answer;


            // public properties (provide public get() and set() methods for private instance data)
            /// <summary>
            /// 
            /// </summary>
            public int LeftOperand { get; set; }
            public string MathOp { get; set; }
            public int RightOperand { get; set; }
            public int Answer { get; set; }


            /// <summary>
            /// 
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
            /// 
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
            /// Sets up the data grid view in the format of the first number the mathOp, the second number, the equals sign and the answer of the math question
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
            /// 
            /// </summary>
            /// <returns></returns>

            // ToString() over-ride method to display all data for the instance
            // Format example 24(3 * 8)
            public override string ToString()
            {
                return Answer + "(" + LeftOperand + " " + MathOp + " " + RightOperand + ")";
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            // QuestionToSend method
            public string QuestionToSend()
            {
                return LeftOperand + " " + MathOp + " " + RightOperand + " = " + Answer;
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

        [SetUp]
        public void Setup()
        {



        }

        [Test]
        public void Test1()
        {
            List<MathQues> mathTest = new List<MathQues>();
            mathTest.Add(new MathQues(5, "+", 7, 12));
            mathTest.Add(new MathQues(12, "*", 2, 24));
            mathTest.Add(new MathQues(6, "-", 4, 2));
            mathTest.Add(new MathQues(15, "/", 5, 3));

            mathTest.Sort();
            int actualResult = BinarySearch(mathTest, "5 + 7 = 12");
            int expectedResult = 2;

            Assert.AreEqual(actualResult, expectedResult);


        }
    }

}

   