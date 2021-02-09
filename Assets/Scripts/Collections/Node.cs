
public class Node<T>
{
    private T data;
    private Node<T> next;
    private Node<T> prev;

    public Node()
    {
        data = default(T);
        next = null;
        prev = null;
    }

    public Node(T data)
    {
        this.data = data;
        next = null;
        prev = null;
    }

    public T Data { get { return data; } set { data = value; } }
    public Node<T> Next { get { return next; } set { next = value; } }
    public Node<T> Prev { get { return prev; } set { prev = value; } }
}
