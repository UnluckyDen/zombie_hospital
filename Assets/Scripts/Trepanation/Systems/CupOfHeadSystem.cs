using System;
using Leopotam.Ecs;
// using NUnit.Framework.Constraints;
using Trepanation.Components;
using Trepanation.UnityComponents;
using UnityEngine;

namespace Trepanation.Systems
{
    public class CupOfHeadSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<ExtractionEvent> _extractionFilter = null;
        private readonly EcsFilter<FastenEvent> _fastenFilter = null;

        private Vector3? _cupOfHeadPosition;
        private bool _firstPlayed;
        private bool _secondPlayed;
        
        public void Run()
        {
            // Debug.Log("CupOfHead System");

            foreach (var idx in _zombieFilter)
            {
                ref var zombieComponent = ref _zombieFilter.Get1(idx);
                var cupOfHeadTransform = zombieComponent.ZombiePointsUc.cupOfHeadTransform;

                if (!_fastenFilter.IsEmpty())
                {
                    if (!_secondPlayed)
                    {
                        SoundManager.Instance.PlayOpenCupOfHead();
                        _secondPlayed = true;
                    }
                    if(_cupOfHeadPosition == null)
                        throw new Exception("_capOfHeadPosition = null");
                    cupOfHeadTransform.localPosition = Vector3.Lerp(
                        cupOfHeadTransform.localPosition, 
                        _cupOfHeadPosition.Value,
                        10 * Time.deltaTime);
                    return;
                }

                if (_extractionFilter.IsEmpty()) continue;

                if (_cupOfHeadPosition == null)
                    _cupOfHeadPosition = cupOfHeadTransform.localPosition;

                if (!_firstPlayed)
                {
                    SoundManager.Instance.PlayOpenCupOfHead();
                    _firstPlayed = true;
                }

                var newPosition = new Vector3(_cupOfHeadPosition.Value.x,
                    _cupOfHeadPosition.Value.y, 
                    _cupOfHeadPosition.Value.z+ 15f);
                
                cupOfHeadTransform.localPosition = Vector3.Lerp(
                    cupOfHeadTransform.localPosition, 
                    newPosition,
                    5 * Time.deltaTime);
            }
        }
    }
}