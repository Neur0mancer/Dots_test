using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct TurretRotationSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var spin = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);

        foreach(var tank in SystemAPI.Query<RefRW<Tank>>())
        {
            var turretTransform = SystemAPI.GetComponentRW<LocalTransform>(tank.ValueRO.Turret);
            turretTransform.ValueRW.Rotation = math.mul(spin, turretTransform.ValueRO.Rotation);
        }
    }
}
