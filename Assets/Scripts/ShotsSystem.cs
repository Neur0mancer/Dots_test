using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ShotsSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var shotsJob = new ShotsJob
        {
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        shotsJob.Schedule();
    }
}
[BurstCompile]
public partial struct ShotsJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public float DeltaTime;

    void Execute(Entity entity, ref CannonBall cannonBall, ref LocalTransform transform)
    {
        var gravity = new float3(0.0f, -9.81f, 0.0f);
        transform.Position += cannonBall.Velocity * DeltaTime;

        if(transform.Position.y <= 0.0f)
        {
            ECB.DestroyEntity(entity);
        }
        cannonBall.Velocity += gravity * DeltaTime;
    }
}
