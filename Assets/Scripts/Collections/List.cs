using UnityEngine;
public class List <T>
{
    private Node<T> root = null;
    private Node<T> last = null;
    private int size = 0;
    public int Size { get { return size; } }

    public List()
    {
    }

    public List(T value)
    {
        Push_back(value);
    }

    public void Push_back(T data)
    {
        if (root != null)
        {
            last.Next = new Node<T>(data);
            last.Next.Prev = last;
            last = last.Next;
            size++;
        }
        else
        {
            root = new Node<T>(data);
            last = root;
            size++;
        }
    }

    public T Get(T data)
    {
        if (root != null)
        {
            var tmp = root;
            for (int i = 0; i < size; i++)
            {
                if (tmp.Data.Equals(data))
                    return tmp.Data;
                tmp = tmp.Next;
            }
        }
        return default;
    }

    public T Get_at(int index)
    {
        if (index == 0) return root.Data;
        if (index == size - 1) return last.Data;
        if(index < size - 1)
        {
            var tmp = root;
            for(int i = 0; i < index; i++)
            {
                tmp = tmp.Next;
            }
            return tmp.Data;
        }
        else
        {
            throw new System.IndexOutOfRangeException();
        }
    }

    public void Remove(T data)
    {
        if(root != null)
        {
            var tmp = root;
            for(int i = 0; i < size; i++)
            {
                if(tmp.Data.Equals(data))
                {
                    if( tmp == root)
                    {
                        root = tmp.Next;
                        size--;
                        return;
                    }
                    if(tmp == last)
                    {
                        last = tmp.Prev;
                        size--;
                        return;
                    }

                    tmp.Prev.Next = tmp.Next;
                    tmp.Next.Prev = tmp.Prev;
                }
            }
        }
    }

    public void Print()
    {
        var tmp = root;
        for (int i = 0; i < size; i++)
        {
            Debug.Log(tmp.Data.ToString());
            tmp = tmp.Next;
        }
    }

    public bool IsEmpty()
    {
        if (size != 0) return false;
        return true;
    }
}
