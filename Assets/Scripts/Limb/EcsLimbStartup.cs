using Data;
using Leopotam.Ecs;
using Limb.Data;
using Limb.Systems;
using Limb.UnityComponents;
using UnityComponents;
using UnityEngine;

namespace Limb
{
    internal sealed class EcsLimbStartup : MonoBehaviour
    {
        [SerializeField] private ZombieData zombieData;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LimbData limbData;
        [SerializeField] private EmitterUc emitterUc;

        private EcsWorld _world;
        private EcsSystems _systems;

        private void Start()
        {
            _ = SoundManager.Instance;
            
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
#if UNITY_EDITOR
            // EcsWorldObserver.Create(_world);
            // EcsSystemsObserver.Create(_systems);
#endif
            _systems
                .Add(new CreateZombieSystem())
                .Add(new CreateTraySystem())
                .Add(new MoveTraySystem())
                .Add(new MoveCameraSystem())
                .Add(new MoveHandSystem())
                .Add(new MoveSystem())
                .Add(new EmitterSystem())
                .Add(new ScrewSystem())
                .Add(new ScrewdriverSystem())
                .Add(new SpinSystem())
                .Add(new DetachSystem())
                .Add(new VolumeSystem())
                .Add(new FaceAnimatinSystem())
                .Add(new EyeTrackingSystem())

                .Inject(mainCamera)
                .Inject(limbData)
                .Inject(emitterUc)
                .Inject(zombieData);
                
                //.Init();
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