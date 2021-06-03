using Data;
using Leopotam.Ecs;
using Teeth.Data;
using Teeth.Systems;
using Teeth.UnityComponents;
using UnityComponents;
using UnityEngine;

namespace Teeth
{
    internal sealed class EcsTeethStartup : MonoBehaviour
    {
        [SerializeField] private ZombieData zombieData = null;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private TeethData teethData;
        [SerializeField] private EmitterUc emitterUc;

        private EcsWorld _world;
        private EcsSystems _systems;

        private void Start()
        {
            _ = SoundManager.Instance;
            
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            ZombieManager.Instance.zombieData = zombieData;
            
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
            _systems
                .Add(new CreateZombieSystem())
                .Add(new CameraSystem())
                .Add(new HammerSystem())
                .Add(new TraySystem())
                .Add(new ToothSystem())
                .Add(new ToothMoveSystem())
                .Add(new EmitterSystem())
                .Add(new VolumeSystem())
                .Add(new EyeTrackingSystem())

                .Inject(mainCamera)
                .Inject(teethData)
                .Inject(emitterUc)
                .Inject(zombieData);
                // .Init();
            
            if (ZombieManager.Instance.tutorial) _systems.Add(new TutorialSystem());
            _systems.Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems == null) return;
            _systems.Destroy();
            _systems = null;
            _world.Destroy();
            _world = null;
        }
    }
}