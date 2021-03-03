using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaRD2
{
    public class Binarytree<T> where  T:IComparable
    {

        public Nodetree<T> root { get; set; }
        //public Nodetree<T> parent { get; set; }
        public Nodetree<T> before { get; set; }
       public Nodetree<T> after { get; set; }

        public Nodetree<T> info { get; set; }
        public bool insert(T value)
        {
             before = null;  
           after = this.root;
            
            while (after != null)
            {
                before = after;
           
                if (after.Data.CompareTo(value)< 0)
                {
                    after = after.leftnode;
                }
                else if (after.Data.CompareTo(value) > 0)
                {
                    after = after.rightnode;
                }
                else
                {
                    return false;
                }
            }
            Nodetree<T> newnode = new Nodetree<T>();
            newnode.Data = value;
            if (this.root == null)
            {
                this.root = newnode;
            }
            
            else
            {
                if (before.Data.CompareTo(value) <0)
                {
                    before.leftnode = newnode;
              
                }
                else
                {
                    before.rightnode = newnode;
           
                }
            }

            return true;
        }
     
        public Nodetree<T> find(string value)
        {
            return this.find(value, this.root);
        }

     private Nodetree<T> find (string value , Nodetree<T> parent)
        {
            if  (parent != null)
            {
                if (parent.Data.CompareTo(value) == 0)
                {
                    return parent;
                }
                if (parent.Data.CompareTo(value)< 0)
                {
                    return find(value, parent.leftnode);
                }
                else
                {
                    return find(value, parent.rightnode);
                }
            }
            return null;
        }
       

        
        public string preorder(Nodetree<T> parent, string  info )
        {
            if  (parent != null)
            {
            
              
                info += parent.Data+"\n"; 
                preorder(parent.leftnode,info);
                preorder(parent.rightnode,info);
            }
            return info; 
        
        }
  
        public string  inorder(Nodetree<T> root, string info)
        {
            if (root != null)
            {
                inorder(root.leftnode, info);
                info += root.Data + "\n";
                inorder(root.rightnode, info);
            }
            return info; 
        }

        public string postorder(Nodetree<T> root, string info)
        {
            if (root != null)
            {

             postorder(root.leftnode, info);
                postorder(root.rightnode, info);

                info += root.Data + "\n";
             
            }
            return info;

        }

      
    }  

}

