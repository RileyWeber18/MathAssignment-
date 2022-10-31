using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAssignment
{
    // Node <T> generic class
    // <T> means that the class accepts any reference data type as its data
    // IComparable interface contains one method: CompareTo()
    // CompareTo() returns 1 of 3 integer values
    // e.g. node.data.CompareTo(data)
    // if result is > 0, then node.data > data
    // if result is < 0, then node.data < data
    // if result is 0, then node.data == data
    // NOTE: The actual CompareTo() method is implemented in the class specified in <T>
    // and not here ... this means that any class used for the Node must have the IComparable interface implemented

    class Node<T> where T : IComparable<T>
    {
        // data component
        public T data;
        // reference to the left child node (a memory address)
        public Node<T> leftChild;
        // reference to the right child node (a memory address)
        public Node<T> rightChild;

        // constructor method for Node()
        // initialise data
        // set right and left child nodes to null
        public Node(T data)
        {
            // when initialised, this default constructor sets up the input data
            // to get assigned to the new object's data
            // the references for the left and right child nodes are made null
            // as none exist at this point
            this.data = data;
            rightChild = null;
            leftChild = null;
        }

        public void Display()
        {
            Console.Write("[");
            Console.Write(data);
            Console.Write("]");
        }

        // Search() method to find a particular data value
        // as this is a generic data type <T>, this uses a CompareTo() method
        public bool Search(Node<T> node, T data)
        {
            // if node is null, then this is the root node
            if (node == null)
            {
                return false;
            }
            else
            {
                // existing node --- check if data is > node.data
                if (node.data.CompareTo(data) > 0)
                {
                    return Search(node.rightChild, data);
                }
                // existing data --- check if data is < node.data
                else if (node.data.CompareTo(data) < 0)
                {
                    return Search(node.leftChild, data);
                }
                // if existing data is the same as node.data
                else
                {
                    return true;
                }
            }
        }  
    }
}


