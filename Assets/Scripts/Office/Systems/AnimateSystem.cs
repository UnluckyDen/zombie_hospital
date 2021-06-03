using Leopotam.Ecs;
using Office.Components;
using Office.Data;
using UnityEngine;

namespace Office.Systems
{
    public class AnimateSystem : IEcsRunSystem
    {
        private readonly OfficeData _officeData = null;
        private readonly EcsFilter<ReadyToAnimateEvent, AnimateEvent> _animateFilter = null;
        
        public void Run()
        {
            foreach (var idx in _animateFilter)
            {
                ref var readyToAnimateComponent = ref _animateFilter.Get1(idx);
                var hideTransform = readyToAnimateComponent.HideGameObject.transform;
                var showTransform = readyToAnimateComponent.ShowGameObject.transform;
                
                if (hideTransform.localScale.x > 0)
                {
                    var localScaleValue = hideTransform.localScale.x - _officeData.scaleSpeed *Time.deltaTime;
                    hideTransform.localScale = new Vector3(localScaleValue, localScaleValue, localScaleValue);
                    break;
                }
                hideTransform.localScale = Vector3.zero;

                if (showTransform.localScale.x < 1)
                {
                    var localScaleValue = showTransform.localScale.x + _officeData.scaleSpeed * Time.deltaTime;
                    showTransform.localScale = new Vector3(localScaleValue, localScaleValue, localScaleValue);
                    break;
                }
                showTransform.localScale = Vector3.one;
                
                _animateFilter.GetEntity(idx).Destroy();
            }
        }
    }
}