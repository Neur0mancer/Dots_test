using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct TankMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach(var (transform, entity) in
            SystemAPI.Query<RefRW<LocalTransform>>()
            .WithAll<Tank>()
            .WithNone<Player>()
            .WithEntityAccess())
        {
            var pos = transform.ValueRO.Position;
            //Dependency from entity index to smoothe randomization
            pos.y = (float)entity.Index;
            var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;
            var dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);

            transform.ValueRW.Position += dir * deltaTime * 0.5f;
            transform.ValueRW.Rotation = quaternion.RotateY(angle);
        }
    }
}
    


