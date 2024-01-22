using Godot;
using System;

public class Boss : KinematicBody2D
{
    private Spawner spawner;
    [Export]
    private float spawnerRotationSpeed = 5;
    public override void _Ready()
    {
        spawner = GetNode<Spawner>("Spawner");
        //spawner.IsEnable = true;
    }
    
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //spawner.Rotation+=spawnerRotationSpeed*delta;

    }
}
