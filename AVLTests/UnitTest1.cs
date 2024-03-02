using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CustomAVLTree;

namespace AVLTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCountInc()
        {
            AVLTree<int, int> AVL = new AVLTree<int, int>();
            AVL.Add(1, 1);
            Assert.AreEqual(1, AVL.Count);
            AVL.Add(2, 1);
            Assert.AreEqual(2, AVL.Count);
        }

        [TestMethod]
        public void TestCountDec()
        {
            AVLTree<int, int> AVL = new AVLTree<int, int>();
            AVL.Add(1, 1);
            AVL.Add(2, 1);
            AVL.Delete(1);
            Assert.AreEqual(1, AVL.Count);
        }

        [TestMethod]
        public void TestAddElement()
        {
            AVLTree<int, int> AVL = new AVLTree<int, int>();
            AVL.Add(1, 1);
            AVL.Add(5, 1);
            Assert.AreEqual(true, AVL.Contains(1));
            Assert.AreEqual(true, AVL.Contains(5));
        }

        [TestMethod]
        public void TestDeleteElement()
        {
            AVLTree<int, int> AVL = new AVLTree<int, int>();
            AVL.Add(1, 1);
            AVL.Add(5, 1);
            AVL.Add(6, 1);
            Assert.AreEqual(true, AVL.Delete(5));
            Assert.AreEqual(false, AVL.Contains(5));
        }

        [TestMethod]
        public void TestFindValue()
        {
            AVLTree<int, int> AVL = new AVLTree<int, int>();
            AVL.Add(1, 1);
            AVL.Add(5, 4);
            AVL.Add(6, 7);
            Assert.AreEqual(1, AVL.Find(1));
            Assert.AreEqual(4, AVL.Find(5));
            Assert.AreEqual(7, AVL.Find(6));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddDuplicateKey()
        {
            AVLTree<int, int> AVL = new AVLTree<int, int>();
            AVL.Add(1, 1);
            AVL.Add(1, 7); 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestFindForEmptyTree()
        {
            AVLTree<int, int> AVL = new AVLTree<int, int>();
            AVL.Find(1);
        }
    }
}
