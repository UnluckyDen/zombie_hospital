using Leopotam.Ecs;
using Office.Data;
using Office.Systems;
using Office.UnityComponents;
using UnityEngine;

namespace Office 
{
    internal sealed class EcsOfficeStartup : MonoBehaviour
    {
        [SerializeField] private EmitterUc emitterUc;
        [SerializeField] private Camera officeCamera;
        [SerializeField] private OfficeData officeData;
        
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
                .Add(new CameraSystem())
                .Add(new EmitterSystem())
                .Add(new AnimateSystem())
                .Inject(officeCamera)
                .Inject(emitterUc)
                .Inject(officeData)
                .Init ();
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