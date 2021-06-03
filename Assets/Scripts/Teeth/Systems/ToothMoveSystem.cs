using System;
using Leopotam.Ecs;
using Teeth.Components;
using Teeth.UnityComponents;
using UnityComponents;
using UnityEngine;

namespace Teeth.Systems
{
    public class ToothMoveSystem : IEcsRunSystem
    {
        private readonly EmitterUc _emitterUc = null;
        private readonly EcsFilter<ToothComponent> _toothFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;
        
        private const float Tolerance = 0.01f;
        
        private bool _prevConnected;
        private bool _showInterface;

        public void Run()
        {
            foreach (var idx in _toothFilter)
            {
                ref var toothComponent = ref _toothFilter.Get1(idx);

                if (!_completedFilter.IsEmpty())
                    toothComponent.Speed = 10000;

                var currentRotation = toothComponent.Transform.rotation;

                var position = toothComponent.ToParentTransform
                    ? toothComponent.CurrentParentTransform.position
                    : toothComponent.NewPosition;
                var rotation = toothComponent.CurrentParentTransform.rotation;
                var scale = toothComponent.CurrentParentTransform.localScale;

                if (position.y < toothComponent.StartParentTransform.position.y)
                    position = new Vector3(position.x, toothComponent.StartParentTransform.position.y, position.z);

                toothComponent.Transform.position = Vector3.Lerp(
                    toothComponent.Transform.position,
                    position,
                    toothComponent.Speed * Time.deltaTime);
                toothComponent.Transform.rotation = Quaternion.Lerp(
                    currentRotation,
                    rotation,
                    toothComponent.Speed * Time.deltaTime);
                toothComponent.Transform.localScale = Vector3.Lerp(
                    toothComponent.Transform.localScale,
                    scale,
                    toothComponent.Speed * Time.deltaTime);
                
                if(!toothComponent.InMouth) continue;

                var toothPosition = toothComponent.Transform.position;
                var connected = Math.Abs(toothPosition.x - position.x) < Tolerance &&
                                Math.Abs(toothPosition.y - position.y) < Tolerance &&
                                Math.Abs(toothPosition.z - position.z) < Tolerance;
                if (connected && !_prevConnected)
                {
                    SoundManager.Instance.PlayZombieReaction();
                }
                _prevConnected = connected;
                if(_completedFilter.IsEmpty()) _emitterUc.chooseThisButton.gameObject.SetActive(connected);
                else
                {
                    if (!_showInterface)
                    {
                        _emitterUc.avatar.sprite = ZombieManager.Instance.GetAvatar();

                        var brain = LevelManager.Instance.trepanationComplete;
                        var hand = LevelManager.Instance.limbComplete;
                        var teeth = LevelManager.Instance.teethComplete;

                        _emitterUc.brainCheckMark.enabled = brain;
                        _emitterUc.handCheckMark.enabled = hand;
                        _emitterUc.teethCheckMark.enabled = teeth;
                        
                        var progressBarValue = 0;
                        if (brain) progressBarValue++;
                        if (hand) progressBarValue++;
                        if (teeth) progressBarValue++;

                        if (progressBarValue == 3)
                        {
                            _emitterUc.stamp.enabled = true;
                            _emitterUc.moneyText.enabled = true;
                        }

                        _emitterUc.progressBar.fillAmount = progressBarValue / 3f;

                        _emitterUc.finishScreenParent.SetActive(true);
                        _showInterface = true;
                    }
                }
            }
        }
    }
}