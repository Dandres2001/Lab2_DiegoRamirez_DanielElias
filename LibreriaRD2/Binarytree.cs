using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaRD2
{
    public class Binarytree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public Binarytree<T> leftree { get; private set; }
        public Binarytree<T> rightree { get; private set; }

        public T Node { get; private set; }

        public Binarytree(T node)
        {
            this.Node = node;
        }

        public void insert(T item)
        {
            if (this.Node.CompareTo(item) > 0)
            {
                if (this.leftree == null)
                {
                    this.leftree = new Binarytree<T>(item);
                }
                else
                {
                    this.leftree.insert(item);
                }

            }
            else
            {
                if (this.rightree == null)
                {
                    this.rightree = new Binarytree<T>(item);
                }
                else
                {
                    this.rightree.insert(item);
                }
            }
        }



        System.Collections.Generic.IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (this.leftree != null)
            {
                foreach (T item in this.leftree)
                {
                    yield return item;
                }
            }

            yield return this.Node;

            if (this.rightree != null)
            {
                foreach (T item in this.rightree)
                {
                    yield return item;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }



    }

}
