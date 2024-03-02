using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomAVLTree
{
    public class Node<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public int Height { get; private set; } = 1;
        public Node<TKey, TValue> Parent { get; set; }
        public Node<TKey, TValue> RChild { get; set; }
        public Node<TKey, TValue> LChild { get; set; }
        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        private static int GetHeight(Node<TKey, TValue> node)
        {
            return node != null ? node.Height : 0;
        }

        public int Balance_factor()
        {
            return GetHeight(RChild) - GetHeight(LChild);
        }

        public void FixHeight()
        {
            var LChildHeight = GetHeight(LChild);
            var RChildHeight = GetHeight(RChild);
            Height = (LChildHeight > RChildHeight ? LChildHeight : RChildHeight) + 1;
        }
    }
    public class AVLTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public AVLTree() { }
        private Node<TKey, TValue> _root;
        public int Count { get; private set; }

        private void BalanceTree(Node<TKey, TValue> balance_node)
        {
            var currentNode = balance_node;
            while (currentNode != null)
            {
                currentNode.FixHeight();
                if (currentNode.Balance_factor() == 2)
                {
                    //Большой левый поворот
                    if (currentNode.RChild.Balance_factor() < 0)
                        SmallRight_Rotate(currentNode.RChild);
                    currentNode = SmallLeft_Rotate(currentNode);
                }
                if (currentNode.Balance_factor() == -2)
                {
                    //Большой правый поворот
                    if (currentNode.LChild.Balance_factor() > 0)
                        SmallLeft_Rotate(currentNode.LChild);
                    currentNode = SmallRight_Rotate(currentNode);
                }
                currentNode = currentNode.Parent;
            }
        }

        public void Add(TKey key, TValue value)
        {
            Node<TKey, TValue> addNode = new Node<TKey, TValue>(key, value);
            if (_root == null)
            {
                _root = addNode;
            }
            else
            {
                Insert(addNode);
            }
            Count++;
        }

        public TValue Find(TKey key)
        {
            if (_root == null)
            {
                throw new InvalidOperationException("Дерево пустое!");
            }
            Node<TKey, TValue> current = _root;
            while (current != null)
            {
                if (key.CompareTo(current.Key) < 0)
                {
                    current = current.LChild;
                }
                else if (key.CompareTo(current.Key) > 0)
                {
                    current = current.RChild;
                }
                else
                {
                    return current.Value;
                }
            }
            return default(TValue);
        }

        public bool Contains(TKey key)
        {
            if (_root == null)
            {
                throw new InvalidOperationException("Дерево пустое!");
            }
            Node<TKey, TValue> current = _root;
            while (current != null)
            {
                if (key.CompareTo(current.Key) < 0)
                {
                    current = current.LChild;
                }
                else if (key.CompareTo(current.Key) > 0)
                {
                    current = current.RChild;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private void Insert(Node<TKey, TValue> addNode)
        {
            var currentNode = _root;
            var ParentNode = _root;
            while (currentNode != null)
            {
                ParentNode = currentNode;
                if (currentNode.Key.CompareTo(addNode.Key) > 0)
                    currentNode = currentNode.LChild;
                else if (currentNode.Key.CompareTo(addNode.Key) < 0)
                    currentNode = currentNode.RChild;
                else
                    throw new ArgumentException("Такой ключ уже добавлен");
            }
            if (ParentNode.Key.CompareTo(addNode.Key) > 0)
                ParentNode.LChild = addNode;
            if (ParentNode.Key.CompareTo(addNode.Key) < 0)
                ParentNode.RChild = addNode;
            addNode.Parent = ParentNode;
            BalanceTree(addNode.Parent);
        }


        private Node<TKey, TValue> GetNode(TKey key)
        {
            var currentNode = _root;
            while (currentNode != null)
            {
                if (currentNode.Key.CompareTo(key) > 0)
                    currentNode = currentNode.LChild;
                else if (currentNode.Key.CompareTo(key) < 0)
                    currentNode = currentNode.RChild;
                else
                    return currentNode;
            }
            return null;
        }

        private void RemoveNodeNotSubtree(Node<TKey, TValue> currentNode)
        {
            var ParentNode = currentNode.Parent;
            //Если узел не является корнем
            if (ParentNode != null)
            {
                //Проверяем где находится узел
                if (ParentNode.LChild == currentNode)
                    ParentNode.LChild = null;
                else
                    ParentNode.RChild = null;
                currentNode.Parent = null;
                BalanceTree(ParentNode);
            }
            else
                _root = null;
        }

        private void RemoveNodeNotRightSubtree(Node<TKey, TValue> currentNode)
        {
            var ParentNode = currentNode.Parent;
            //Если узел не является корнем
            if (ParentNode != null)
            {
                //Проверяем где находится узел
                if (ParentNode.LChild == currentNode)
                {
                    ParentNode.LChild = currentNode.LChild;
                    currentNode.LChild.Parent = ParentNode;
                    BalanceTree(ParentNode.LChild);
                }
                else
                {
                    ParentNode.RChild = currentNode.LChild;
                    currentNode.LChild.Parent = ParentNode;
                    BalanceTree(ParentNode.RChild);
                }
            }
            else
            {
                _root = _root.LChild;
                _root.Parent = null;
                BalanceTree(_root);
            }
        }

        private void RemoveNodeHaveRightSubtree(Node<TKey, TValue> currentNode)
        {
            var ParentNode = currentNode.Parent;
            var minNode = FindMin_RightSubTree(currentNode.RChild);
            var ParentMinNode = minNode.Parent;
            //Если узел не является корнем
            if (ParentNode != null)
            {
                //Проверяем где находится узел
                if (ParentNode.LChild == currentNode)
                    ParentNode.LChild = minNode;
                else
                    ParentNode.RChild = minNode;
                SwapLinksDuringRemoveNode(currentNode, minNode);
            }
            else
            {
                minNode.LChild = _root.LChild;

                if (minNode.RChild != null)
                {
                    minNode.RChild.Parent = minNode.Parent;
                    minNode.Parent.LChild = minNode.RChild;
                }
                else
                    minNode.Parent.LChild = null;

                if (_root.RChild != minNode)
                {
                    minNode.RChild = _root.RChild;
                }

                if (minNode.LChild != null)
                    minNode.LChild.Parent = minNode;
                if (minNode.RChild != null)
                    minNode.RChild.Parent = minNode;

                _root = minNode;
                _root.Parent = null;
            }
            if (ParentMinNode != currentNode)
                BalanceTree(ParentMinNode);
            else
                BalanceTree(minNode);
        }

        private void SwapLinksDuringRemoveNode(Node<TKey, TValue> currentNode, Node<TKey, TValue> minNode)
        {
            var ParentNode = currentNode.Parent;
            minNode.LChild = currentNode.LChild;

            if (minNode.RChild != null)
            {
                minNode.RChild.Parent = minNode.Parent;
                minNode.Parent.LChild = minNode.RChild;
            }
            else
                minNode.Parent.LChild = null;

            if (currentNode.RChild != minNode)
            {
                minNode.RChild = currentNode.RChild;
            }

            if (minNode.LChild != null)
                minNode.LChild.Parent = minNode;
            if (minNode.RChild != null)
                minNode.RChild.Parent = minNode;

            minNode.Parent = ParentNode;
        }

        public bool Delete(TKey key)
        {
            var currentNode = GetNode(key);
            if (currentNode != null)
            {
                if (currentNode.LChild == null && currentNode.RChild == null)
                    RemoveNodeNotSubtree(currentNode);
                else if (currentNode.LChild != null && currentNode.RChild == null)
                    RemoveNodeNotRightSubtree(currentNode);
                else
                    RemoveNodeHaveRightSubtree(currentNode);
                Count--;
                return true;
            }
            return false;
        }

        private Node<TKey, TValue> FindMin_RightSubTree(Node<TKey, TValue> current)
        {
            var currentNode = current;
            var parentNode = current;
            while (currentNode != null)
            {
                parentNode = currentNode;
                currentNode = currentNode.LChild;
            }
            return parentNode;
        }

        #region Rotates
        private Node<TKey, TValue> SmallLeft_Rotate(Node<TKey, TValue> balance_node)
        {
            Node<TKey, TValue> temp_node = balance_node.RChild;
            balance_node.RChild = temp_node.LChild;
            if (temp_node.LChild != null)
            {
                temp_node.LChild.Parent = balance_node;
            }
            temp_node.Parent = balance_node.Parent;
            if (balance_node.Parent == null)
            {
                _root = temp_node;
            }

            else if (balance_node == balance_node.Parent.LChild)
            {
                balance_node.Parent.LChild = temp_node;
            }
            else
            {
                balance_node.Parent.RChild = temp_node;
            }
            temp_node.LChild = balance_node;
            balance_node.Parent = temp_node;
            balance_node.FixHeight();
            temp_node.FixHeight();
            return temp_node;
        }
        private Node<TKey, TValue> SmallRight_Rotate(Node<TKey, TValue> balance_node)
        {
            Node<TKey, TValue> temp_node = balance_node.LChild;
            balance_node.LChild = temp_node.RChild;
            if (temp_node.RChild != null)
            {
                temp_node.RChild.Parent = balance_node;
            }
            temp_node.Parent = balance_node.Parent;
            if (balance_node.Parent == null)
            {
                _root = temp_node;
            }
            else if (balance_node == balance_node.Parent.RChild)
            {
                balance_node.Parent.RChild = temp_node;
            }
            else
            {
                balance_node.Parent.LChild = temp_node;
            }
            temp_node.RChild = balance_node;
            balance_node.Parent = temp_node;
            balance_node.FixHeight();
            temp_node.FixHeight();
            return temp_node;
        }
        #endregion
    }
}
