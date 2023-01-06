﻿using System;
using MVC.Gameplay.Models.Player;
using MVC.StateMachine.States;
using MVC.Views;
using MVC_Pattern.Scripts.Gameplay.Services.StateMachine;
using UnityEngine;
using VContainer.Unity;

namespace MVC.Controllers
{
    public class PlayerInteractionsController : IInitializable, IDisposable
    {
        private readonly IStateMachine _stateMachine;
        private readonly PlayerView _playerView;
        private readonly PlayerModel _playerModel;
        private readonly PlayerContainer _opponentContainer;

        public PlayerInteractionsController(PlayerContainer playerContainer, IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _playerView = playerContainer.View;
            _playerModel = playerContainer.Model;
            _opponentContainer = playerContainer.OpponentContainer;
        }

        void IInitializable.Initialize()
        {
            _playerModel.InvokeTurnCheck();

            HandlePlayerEvents();
        }

        void IDisposable.Dispose()
        {
            DisposePlayerEvents();
        }

        private void HandlePlayerEvents()
        {
            _playerView.MainTriggerDetector.OnTriggerEntered += HandleBlock;

            _playerView.CollisionDetector.OnCollisionEntered += OnCollisionEntered;
            _playerView.CollisionDetector.OnCollisionExited += OnCollisionExit;

            _playerView.SideDetectorView.OnTriggerEntered += InvokePlayerSideCheck;
            _playerView.SideDetectorView.OnTriggerExited += InvokePlayerSideCheck;
        }

        private void DisposePlayerEvents()
        {
            _playerView.MainTriggerDetector.OnTriggerEntered -= HandleBlock;

            _playerView.CollisionDetector.OnCollisionEntered -= OnCollisionEntered;
            _playerView.CollisionDetector.OnCollisionExited -= OnCollisionExit;

            _playerView.SideDetectorView.OnTriggerEntered -= InvokePlayerSideCheck;
            _playerView.SideDetectorView.OnTriggerExited -= InvokePlayerSideCheck;
        }

        private void HandleBlock(Collider collider)
        {
            if (collider.TryGetComponent(out TriggerDetectorView attackHitBox) &&
                attackHitBox == _opponentContainer.AttackHitBox)
            {
                if (_playerModel.IsBlocking)
                {
                    _stateMachine.ChangeState<BlockState>();
                }
                else
                {
                    _playerModel.InvokePlayerAttacked(attackHitBox);
                    _stateMachine.ChangeState<StunnedState>();
                }
            }
        }

        private void InvokePlayerSideCheck(Collider collider)
        {
            if (collider.GetComponent<PlayerView>())
            {
                _playerModel.InvokeTurnCheck();
            }
        }

        private void OnCollisionEntered(Collision collision)
        {
            if (collision.gameObject.GetComponent<PlatformView>())
            {
                _playerModel.IsGrounded.Value = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent<PlatformView>())
            {
                _playerModel.IsGrounded.Value = false;
            }
        }
    }
}