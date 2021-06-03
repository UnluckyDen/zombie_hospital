using System.Text;
using Leopotam.Ecs;
using Limb.Components;
using UnityEngine;

namespace Limb.Systems
{
    public class EyeTrackingSystem : IEcsInitSystem,IEcsRunSystem
    {
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        
        private Camera mainCamera;

        public void Init()
        {
            mainCamera = Camera.main;
        }
        
        public void Run()
        {
            foreach (var idx in _zombieFilter)
            {
                var mousePosition = Input.mousePosition;
                var mouseWordPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));

                ref var zombieComponent = ref _zombieFilter.Get1(idx);
                var eyeL = zombieComponent.ZombiePointsUc.eyeLeft;
                var eyeR = zombieComponent.ZombiePointsUc.eyeRight;
                
                eyeL.transform.LookAt(mouseWordPosition);
                eyeR.transform.LookAt(mouseWordPosition);
            }
        }
    }
}