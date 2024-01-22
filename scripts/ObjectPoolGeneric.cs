using Godot;
using System;
using System.Collections.Generic;

public interface IPoolable<T>
{
    //ObjectPool<T> Pool { get; set; }
    void ReturnToPool();
}
public class ObjectPool<T> where T : Node2D, IPoolable<T>
{
    private int minCount;
    private readonly PackedScene scene;
    private readonly Queue<T> objects = new Queue<T>();
    private readonly Action<T> onReturnObject;

    public ObjectPool(PackedScene scene,int minCount, Action<T> onReturnObject = null)
    {
        this.minCount=minCount;
        this.scene = scene;
        this.onReturnObject = onReturnObject ?? DefaultReturnAction;
        for(int i=0; i<minCount; i++){
            T newNode = (T)scene.Instance();
            objects.Enqueue(newNode);
        }
    }

    private void DefaultReturnAction(T obj)
    {
        // По умолчанию ничего не делаем при возврате объекта
    }

    public T GetObject()
    {
        if (objects.Count > 0)
        {
            return objects.Dequeue();
        }
        else
        {
            //GD.Print("new created "+objects.Count);
            // Если в пуле нет объектов, создаем новый из PackedScene
            T newNode = (T)scene.Instance();
            return newNode;
        }
    }

    public void ReturnObject(T obj)
    {
       
        // Вызываем пользовательский делегат или используем дефолтный метод
        onReturnObject(obj);

        // Возвращаем объект в пул
        objects.Enqueue(obj);
        //GD.Print("returned "+objects.Count);
    }
}
