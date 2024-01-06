using Godot;
using System;
using System.Diagnostics;

public class Player : KinematicBody2D
{
    [Export] private float _acceleration =0.25f;
    [Export] private float _maxSpeed ;
    [Export] private float _friction = 0.5f;
    [Export] private float _minSpeed=200 ;
    [Export] private float _shootCooldown = 0.5f;
    [Export] private float _dashCoolDown = 0.5f;
    [Export] private int _dashSpeed = 2000;
    [Export] private int DASH_TIME = 100;
    private Vector2 velocity = new Vector2();
    private Vector2 dashDirection = new Vector2();
    private bool canShoot = true;
    private bool canDash = true;
    private AnimatedSprite sprite;
    private RayCast2D shootRay;
    private Timer shootCooldown;
    private Timer dashCooldown;
    private Vector2 input ;
    [Export]private float _speed=200;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite>("%AnimatedSprite");
        shootRay = GetNode<RayCast2D>("ShootRay");
        shootCooldown = GetNode<Timer>("ShootCooldown");
        dashCooldown = GetNode<Timer>("DashCooldown");
        shootCooldown.WaitTime = _shootCooldown;
        dashCooldown.WaitTime=_dashCoolDown;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput(delta);
        AnimateCharacter();
        MovePlayer();
        Shoot();
        Dash();
    }

    private void AnimateCharacter(){
        if(input.x!=0 && input.y==0){
            sprite.Animation="Run";
        }
        else if(input.y>0)
        {
            sprite.Animation="DashDown";
        }
        else if(input.y<0)
        {
            sprite.Animation="DashUp";
        }else if(input.y==0 && input.x==0){
            sprite.Animation="default";
        }
    }
    private void GetInput(float delta)
    {
        input=Input.GetVector("ui_left","ui_right", "ui_up","ui_down");
        
        if (Input.IsActionPressed("ui_right"))
        {
            
            sprite.FlipH = false;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            
            sprite.FlipH = true;
        }

        if(Input.IsActionPressed("get_axis_X_left"))
        {
            input.x -= Input.GetActionStrength("get_axis_X_left");
            sprite.FlipH = true;
        }
        if(Input.IsActionPressed("get_axis_X_right"))
        {
            input.x += Input.GetActionStrength("get_axis_X_right");
            sprite.FlipH = false;
        }
        if(Input.IsActionPressed("get_axis_Y_up"))
        {
            input.y -= Input.GetActionStrength("get_axis_Y_up");
        }
        if(Input.IsActionPressed("get_axis_Y_down"))
        {
            input.y += Input.GetActionStrength("get_axis_Y_down");
        }
        if (input.Length() > 0)
        {
            _speed=Mathf.Lerp(_speed,_maxSpeed,1 - Mathf.Pow(1 - _acceleration, delta)/*_acceleration*/);
            velocity = input.Normalized() * _speed;
            
        }
        else
        {
            velocity.x = Mathf.Lerp(velocity.x,0,_friction);
            velocity.y = Mathf.Lerp(velocity.y,0,_friction);
            _speed=_minSpeed;
        }

        dashDirection = Vector2.Zero;

        if (Input.IsActionJustPressed("ui_select"))
        {
            dashDirection = input.Normalized();
        }
    }

    private void MovePlayer()
    {
        MoveAndSlide(velocity);
    }

    private void Shoot()
    {
        if (Input.IsActionJustPressed("ui_shoot") && canShoot)
        {
            shootRay.CastTo = shootRay.ToLocal(GetGlobalMousePosition());
            shootRay.ForceRaycastUpdate();

            if (shootRay.IsColliding())
            {
                var collider = shootRay.GetCollider();
                /*var health = collider.GetParent().GetNode<Health>("Health");

                if (health != null)
                {
                    health.TakeDamage(10);
                }*/
            }

            canShoot = false;
            shootCooldown.Start();
        }
    }

    private void Dash()
    {
        if (dashDirection != Vector2.Zero && canDash)
        {
            MoveAndSlide(dashDirection * _dashSpeed);
            dashCooldown.Start();
            canDash = false;
        }
    }

    private void _on_ShootCooldown_timeout()
    {
        canShoot = true;
    }

    private void _on_DashCooldown_timeout()
    {
        canDash = true;
    }
}
