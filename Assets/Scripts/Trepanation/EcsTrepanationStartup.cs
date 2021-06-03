using Data;
using Leopotam.Ecs;
using Trepanation.Data;
using Trepanation.Systems;
using Trepanation.UnityComponents;
using UnityComponents;
using UnityEngine;

namespace Trepanation 
{
    internal sealed class EcsTrepanationStartup : MonoBehaviour
    {
        [SerializeField] private ZombieData zombieData;
        [SerializeField] private EmitterUc emitterUc;
        [SerializeField] private Camera trepanationCamera;
        [SerializeField] private TrepanationData trepanationData;
        
        private EcsWorld _world;
        private EcsSystems _systems;

        private void Start () 
        {
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif
            _systems
                .Add(new CreateZombieSystem())
                .Add(new CameraMoveSystem())
                .Add(new GrinderToolSystem())
                .Add(new ZombieRotateSystem())
                .Add(new CupOfHeadSystem())
                .Add(new BrainSystem())
                .Add(new TraySystem())
                .Add(new MoveSystem())
                .Add(new MoveItemSystem())
                .Add(new EmitterSystem())
                .Add(new StaplerSystem())
                .Add(new ZombieRotateAngleSystem())
                .Add(new VolumeSystem())
                .Add(new AnimationSystem())
                .Add(new EyeTrackingSystem())

                .Inject(emitterUc)
                .Inject(trepanationCamera)
                .Inject(trepanationData)
                .Inject(zombieData);
                // .Init ();

            if (ZombieManager.Instance.tutorial) _systems.Add(new TutorialSystem());
            _systems.Init();
        }

        private void Update () 
        {
            _systems?.Run ();
        }

        private void OnDestroy ()
        {
            if (_systems == null) return;
            _systems.Destroy ();
            _systems = null;
            _world.Destroy ();
            _world = null;
        }
    }
}