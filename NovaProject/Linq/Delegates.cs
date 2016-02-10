using System;
using Microsoft.SPOT;

namespace NovaProject.Linq
{
    public delegate void Action(object obj);
    public delegate void Action<T>(T obj);

    public delegate object ActionWithReturn(object obj);
    public delegate R ActionWithReturn<T, R>(T obj);
    public delegate R ActionWithReturn<R>(R obj);

    public delegate object Aggregate(object obj1, object obj2);
    public delegate R Aggregate<R, T>(T obj1, T obj2);
    public delegate R Aggregate<R>(R obj1, R obj2);

    public delegate int Comparer(object obj1, object obj2);
    public delegate int Comparer<T>(T obj1, T obj2);

    public delegate bool Predicate(object obj);
    public delegate bool Predicate<T>(T obj);
    
    public delegate object Selector(object obj);
    public delegate R Selector<T,R>(T obj);
    public delegate R Selector<R>(R obj);

}
