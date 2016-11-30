using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int size;

    public Heap(int size)
    {
        items = new T[size];
    }

    public void Add(T item)
    {
        item.HeapIndex = size;
        items[size] = item;
        Sortup(item);
        size++;
    }

    public T Remove()
    {
        T top = items[0];
        size--;
        items[0] = items[size];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return top;
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }


    public int Count
    {
        get { return size; }
    }

    private void SortDown(T t)
    {
        while (true)
        {
            int childIndexLeft = t.HeapIndex * 2 + 1;
            int childIndexRight = t.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < size)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < size)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        swapIndex = childIndexRight;
                }

                if (t.CompareTo(items[swapIndex]) < 0)
                    Swap(t, items[swapIndex]);
                else
                    return;
            }
            else
                return;
                
        }
    }

    private void Sortup(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
                Swap(item, parentItem);
            else
                break;
        }
    }

    void Swap(T A, T B)
    {
        items[A.HeapIndex] = B;
        items[B.HeapIndex] = A;
        int Aindex = A.HeapIndex;
        A.HeapIndex = B.HeapIndex;
        B.HeapIndex = Aindex;
    }

    public void UpdateItem(T item)
    {
        Sortup(item);
    }

}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}
