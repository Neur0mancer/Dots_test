using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public partial struct TankSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        var config = SystemAPI.GetSingleton<Config>();
        var random = new Random(123);

        for(int i = 0; i < config.TankCount; i++)
        {
            var tankEntity = state.EntityManager.Instantiate(config.TankPrefab);
            var transform = SystemAPI.GetComponentRW<LocalTransform>(tankEntity);
            transform.ValueRW.Position = random.NextFloat3(new float3(50, 0, 50));
            if(i == 0)
            {
                state.EntityManager.AddComponent<Player>(tankEntity);
            }
            var color = new URPMaterialPropertyBaseColor
            {
                Value = RandomColor(ref random)
            };
            var linkedEntities = state.EntityManager.GetBuffer<LinkedEntityGroup>(tankEntity);
            foreach(var entity in linkedEntities)
            {
                if (state.EntityManager.HasComponent<URPMaterialPropertyBaseColor>(entity.Value))
                {
                    state.EntityManager.SetComponentData(entity.Value, color);
                }
            }
        }
    }
    static float4 RandomColor(ref Random random)
    {
        var hue = (random.NextFloat() + 0.61803f) % 1;
        return (Vector4)Color.HSVToRGB(hue, 1.0f, 1.0f);
    }
}
