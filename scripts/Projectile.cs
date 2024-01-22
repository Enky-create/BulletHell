using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public class Projectile : Node2D, IPoolable<Projectile>
{
    [Export] private float speed;
    [Export] private float lifetime;
    
    public Vector2 Direction{get;set;} =Vector2.Zero;
    public Timer timer;
    private Hitbox hitbox;
    private Area2D impactDetector;
    public ObjectPool<Projectile> Pool{
        get;set;
    }
    public override void _Ready()
    {
        SetAsToplevel(true);
        LookAt(Position+Direction);
        timer = GetNode<Timer>("Timer");
        hitbox = GetNode<Hitbox>("Hitbox");
        impactDetector = GetNode<Area2D>("ImpactDetector");
        impactDetector.Connect("body_entered",this,"OnBodyEntered");
        timer.WaitTime=lifetime;
        timer.Connect("timeout",this,"onTimerTimeout");

    }
    
    public override void _PhysicsProcess(float delta)
    {
        Position+=Direction*speed*delta;

    }
    public void ResetTimer()
    {
        timer?.Stop();  // Stop the timer if it's running
        timer.WaitTime = lifetime;
        timer?.Start();
    }
    public void onTimerTimeout()
    {
        ReturnToPool();
    }
    public void OnBodyEntered(Node body)
    {
        ReturnToPool();
    }

    public void ReturnToPool()
    {
        Pool?.ReturnObject(this);
    }
    public void OnDraw(){
        timer.WaitTime=lifetime;
    }
}
