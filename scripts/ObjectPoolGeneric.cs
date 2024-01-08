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
    private readonly PackedScene scene;
    private readonly Queue<T> objects = new Queue<T>();
    private readonly Action<T> onReturnObject;

    public ObjectPool(PackedScene scene, Action<T> onReturnObject = null)
    {
        this.scene = scene;
        this.onReturnObject = onReturnObject ?? DefaultReturnAction;
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
    }
}
