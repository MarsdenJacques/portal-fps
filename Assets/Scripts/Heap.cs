using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapNode<T>
{
    T[] heap;
    public int Count;

    public Heap(int maxSize)
    {
        heap = new T[maxSize];
        Count = 0;
    }
    public void Reset()
    {
        Count = 0;
        heap[0] = default(T);
    }
    public bool Contains(T item)
    {
        return Equals(heap[item.Index], item);
    }
    public void Push(T item)
    {
        heap[Count] = item;
        item.Index = Count;
        UpdatePostion(item);
        Count++;
    }
    public T Pop()
    {
        Count--;
        T min = heap[0];
        T max = heap[Count];
        max.Index = 0;
        heap[0] = max;
        int left = 1;
        int right = 2;
        int swap;
        bool swapping = true;
        while(swapping)
        {
            left = max.Index * 2 + 1;
            right = max.Index * 2 + 2;
            if(left < Count)
            {
                swap = left;
                if (right < Count)
                {
                    if (heap[left].CompareTo(heap[right]) > 0)
                    {
                        swap = right;
                    }
                }
                if (max.CompareTo(heap[swap]) > 0)
                {
                    Swap(max, heap[swap]);
                }
                else
                {
                    swapping = false;
                }
            }
            else
            {
                swapping = false;
            }
        }
        return min;
    }
    public void UpdatePostion(T item)
    {
        int parent = (item.Index - 1) / 2;
        while(parent >= 0 && item.CompareTo(heap[parent]) < 0)
        {
            Swap(item, heap[parent]);
            parent = (item.Index - 1) / 2;
        }
    }
    private void Swap(T item1, T item2)
    {
        heap[item1.Index] = item2;
        heap[item2.Index] = item1;
        item1.Index = item1.Index ^ item2.Index;
        item2.Index = item1.Index ^ item2.Index;
        item1.Index = item1.Index ^ item2.Index;
    }
    //For Fibonacci heap for later
/*    HeapNode<T> min;
    int nodeCount;

    public void Push(T item, int key)
    {

    }
    public void Merge(Heap<T> otherHeap)
    {

    }
    public T Pop()
    {
        return min.item;
    }
    public void UpdateKey(int oldKey, int newKey)
    {

    }
    public void DeleteKey(int deleteKey)
    {

    }*/
}
public interface IHeapNode<T> : IComparable<T>
{
    int Index { get; set; }
    // for fibonacci heap later
    /*HeapNode<T> parent { get; set; }
    HeapNode<T> left { get; set; }
    HeapNode<T> right { get; set; }
    HeapNode<T> child { get; set; }
    int degree { get; set; }
    bool isMarked { get; set; }
    int key { get; set; }
    T item { get; set; }*/
}
























































































































