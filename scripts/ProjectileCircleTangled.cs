using Godot;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
public class ProjectileCircleTangled : KinematicBody2D
{
    public Vector2 Velocity=new Vector2();
    public Vector2 TargetPosition ;
    public float Gravity = 10;
    public float MaxSpeed = 0.5f;
    ObjectPool pool;
    public override void _Ready()
    {
        base._Ready();
        TargetPosition = Vector2.Right.Rotated(RotationDegrees)*1000;
        pool=ObjectPool.Instance;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
       
        Velocity.y += Gravity * delta;
        if (Velocity.y > MaxSpeed) Velocity.y = MaxSpeed;

        Vector2 moveDirection = TargetPosition - Position;
        moveDirection.Normalized();

        Godot.Vector2 desiredVelocity = moveDirection * MaxSpeed;
        Velocity = Velocity.LinearInterpolate(desiredVelocity, 0.5f);
        
        MoveAndSlide(Velocity, Vector2.Up);
        
            
        
    }

    public void onTimerTimeout()
    {
        pool.ReturnObject("res://Vortex.tscn",this);
        
    }

}
