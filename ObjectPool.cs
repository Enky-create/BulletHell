using Godot;
using System;
using System.Collections.Generic;

public class ObjectPool : Node2D
{
    public static ObjectPool instance;

    // Массив для хранения адресов PackedScene
    [Export]
    private string[] scenePaths;

    // Словарь для хранения адресов PackedScene и соответствующих объектов
    private Dictionary<string, List<Node2D>> objectDictionary = new Dictionary<string, List<Node2D>>();
    [Export] private int minPoolSize;

    // Метод для получения экземпляра синглтона

    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GD.Load<PackedScene>("res://ObjectPool.tscn").Instance() as ObjectPool;
                instance.InitializePool();
                // Добавьте этот экземпляр в дерево сцены, чтобы он существовал между сценами
                Instance.AddToGroup("ObjectPoolGroup");
            }
            return instance;
        }
    } 

    public override void _Ready()
    {
        
    }
    // Метод для инициализации пула объектов
    public void InitializePool()
    {
        foreach (string scenePath in scenePaths)
        {
            if (!objectDictionary.ContainsKey(scenePath))
            {
                objectDictionary[scenePath] = new List<Node2D>();

                // Создание и добавление минимального количества объектов в пул
                for (int i = 0; i < minPoolSize; i++)
                {
                    PackedScene packedScene = (PackedScene)ResourceLoader.Load(scenePath);
                    Node2D newObj = (Node2D)packedScene.Instance();
                    objectDictionary[scenePath].Add(newObj);
                    
                }
            }
        }
    }

    // Метод для извлечения объекта из пула
    public Node2D GetObject(string scenePath)
    {
        
        if (!objectDictionary.ContainsKey(scenePath))
        {
            GD.PrintErr("Trying to get object from a pool that doesn't exist: " + scenePath);
            return null;
        }

        List<Node2D> objectList = objectDictionary[scenePath];

        if (objectList.Count > 0)
        {
            GD.Print("Instantiated");
            Node2D obj = objectList[0];
            objectList.RemoveAt(0);
            obj.Visible = true;
            obj.SetProcess(true);
            obj.GetParent()?.RemoveChild(obj);
            return obj;
        }
        else
        {
            // Если в пуле нет объектов, создайте новый
            PackedScene packedScene = (PackedScene)ResourceLoader.Load(scenePath);
            Node2D newObj = (Node2D)packedScene.Instance();
            // Настройка свойств перед использованием
            newObj.Visible = true;
            newObj.SetProcess(true); // Включить обработку
            GD.Print("Created!!!");
            return newObj;
        }
    }

    // Метод для возвращения объекта в пул
    public void ReturnObject(string scenePath, Node2D obj)
    {
        if (!objectDictionary.ContainsKey(scenePath))
        {
            GD.PrintErr("Trying to return object to a pool that doesn't exist: " + scenePath);
            return;
        }

        // Очистка свойств объекта перед возвращением в пул
        obj.Visible = false;
        obj.SetProcess(false);
        
        obj.GetParent()?.RemoveChild(obj);
        
        
        objectDictionary[scenePath].Add(obj);
        GD.Print("Returned");
    }
}
