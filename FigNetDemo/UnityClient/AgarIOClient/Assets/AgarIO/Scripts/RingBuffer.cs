public class RingBuffer<T>
{
    private readonly T[] Data;

    private int head;

    private int tail;

    private int capacity;

    public bool IsEmpty => head == tail;

    public RingBuffer(int capacity)
    {
        Data = new T[capacity];
        this.capacity = capacity;
    }

    public void Enqueue(T data)
    {
        Data[head] = data;
        head++;
        if (head >= capacity)
        {
            head = 0;
        }
    }

    public T Dequeue()
    {
        T result = Data[tail];
        tail++;
        if (tail >= capacity)
        {
            tail = 0;
        }

        return result;
    }
}