using Godot;
using Godot.Collections;

public class Spawner : Node2D
{
    public bool IsEnable=true;
    [Export]
    private PackedScene projectileScene;
    private Array spawnPositions;
    private ObjectPool<Projectile> projectilePool;

    public override void _Ready()
    {
        spawnPositions = GetNode("SpawnPositions").GetChildren();
        // Создаем объектный пул с PackedScene projectile
        projectilePool = new ObjectPool<Projectile>(projectileScene,200, onReturnObject: (Projectile projectile) =>
        {
            projectile.Visible = false;
            projectile.SetProcess(false);
            //projectile.GetParent()?.RemoveChild(projectile);
            
        });
    }

    public void SpawnProjectile(Vector2 position)
    {
        // Получаем объект из пула
        Projectile projectile = projectilePool.GetObject();


        // Устанавливаем позицию объекта
        projectile.Position = position;
        
        // Добавляем объект в сцену
        AddChild(projectile);
    }
    public void onTimerTimeout()
    {
        if(IsEnable)
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            var obj = projectilePool.GetObject();
            var spawnPosition = (Node2D)spawnPositions[i];
            obj.Position = spawnPosition.GlobalPosition;
            
            obj.Direction = (spawnPosition.GlobalPosition - this.GlobalPosition).Normalized();
            obj.Pool = projectilePool;
            obj.Visible = true;
            obj.SetProcess(true);
            
            if(obj.GetParent()==null){
                spawnPosition.AddChild(obj);
            }else{
                obj.ResetTimer();
            }
            

        }
    }
}

// using Godot;
// using Godot.Collections;


// public class Spawner : Node2D
// {
//     const string PATH ="res://Vortex.tscn";
//     private ObjectPool pool;
//     private Array spawnPositions;

//     public override void _Ready()
//     {
//         pool=ObjectPool.Instance;
//         GD.Print("Pizdec");
//         spawnPositions=GetNode("SpawnPositions").GetChildren();
//     }

//     public void onTimerTimeout()
//     {
//         for(int i=0; i<spawnPositions.Count;i++){
//             var obj=pool.GetObject(PATH);
//             obj.Position = Vector2.Zero;
//             var spawnPosition = (Node2D)spawnPositions[i];
//             spawnPosition.AddChild(obj);
//         }
//     }
// }
