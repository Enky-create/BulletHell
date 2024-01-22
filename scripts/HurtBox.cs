using Godot;
using System;

public class HurtBox : Area2D
{
    
    public override void _Ready()
    {
        Connect("area_entered",this,"onHitboxEntered");
    }
    public void onHitboxEntered(Hitbox hitbox){
        if(Owner.HasMethod("TakeDamage")){
            Owner.Call("TakeDamage", hitbox.Damage);
        }
    }

}
