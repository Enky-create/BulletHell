using Godot;
using System;

public class Spawner : Node2D
{
    const string PATH ="res://Vortex.tscn";
    ObjectPool pool;
    

    public override void _Ready()
    {
        pool=ObjectPool.Instance;
        GD.Print("Pizdec");
    }

    public void onTimerTimeout(){
        var obj=pool.GetObject(PATH);
            obj.Position = Vector2.Zero;
            AddChild(obj);
    }
}
