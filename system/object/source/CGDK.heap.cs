//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                       Ver 6.1 / Release 2012.05.28                        *
//*                                                                           *
//*                           Data Template Classes                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*  This Program is programmed by Cho sanghyun. sangducks@gmail.com          *
//*  Best for Game Developement and Optimized for Game Developement.          *
//*                                                                           *
//*                (c) 2008 Cho sanghyun. All right reserved.                 *
//*                           http://www.CGDK.co.kr                           *
//*                                                                           *
//*****************************************************************************

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

//-----------------------------------------------------------------------------
//
// CGD::priority_queue
//
// 1. CGD::priority_queue란!
//
//
// 2. CGD::seat의 표준 준수 사항
//
// 3. 추가설명 or 사용예제
//
//
//-----------------------------------------------------------------------------

namespace CGDK
{

//public class priority_queue
//{
//    int total_size;
//    SortedDictionary<int, Queue> storage;

//    public PrioQueue ()
//    {
//        this.storage = new SortedDictionary<int, Queue> ();
//        this.total_size = 0;
//    }

//    public bool empty ()
//    {
//        return (total_size == 0);
//    }

//    public object Dequeue ()
//    {
//        if (empty())
//        {
//            throw new Exception ("Please check that priorityQueue is not empty before dequeing");
//        } else
//            foreach (Queue q in storage.Values) {
//                // we use a sorted dictionary
//                if (q.Count > 0) {
//                    total_size--;
//                    return q.Dequeue ();
//                }
//            }

//            Debug.Assert(false,"not supposed to reach here. problem with changing total_size");

//            return null; // not supposed to reach here.
//    }

//    // same as above, except for peek.

//    public object Peek ()
//    {
//        if (empty())
//            throw new System.Exception ("Please check that priorityQueue is not empty before peeking");

//        foreach (Queue q in storage.Values) 
//        {
//            if (q.Count > 0)
//                return q.Peek ();
//        }

//        Debug.Assert(false,"not supposed to reach here. problem with changing total_size");

//        return null; // not supposed to reach here.
//    }

//    public object Dequeue (int prio)
//    {
//        total_size--;
//        return storage[prio].Dequeue ();
//    }

//    public void Enqueue (object item, int prio)
//    {
//        if (!storage.ContainsKey (prio)) 
//        {
//            storage.Add (prio, new Queue ());
//        }

//        storage[prio].Enqueue (item);
//        total_size++;

//    }
//}

}