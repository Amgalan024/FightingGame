﻿using System;
using MVC.Gameplay.Models;
using MVC.Views;
using MVC_Pattern.Scripts.Gameplay.Services.StateMachine;
using UnityEngine;
using VContainer.Unity;

namespace MVC.Controllers
{
    public class StateMachineController : IInitializable, IFixedTickable, IDisposable
    {
        private readonly IStateMachine _stateMachine;
        private readonly IStateMachineProxy _stateMachineProxy;
        private readonly StateMachineModel _stateMachineModel;

        private readonly PlayerView _playerView;

        public StateMachineController(StateMachineModel stateMachineModel, IStateMachine stateMachine,
            PlayerView playerView, IStateMachineProxy stateMachineProxy)
        {
            _stateMachineModel = stateMachineModel;
            _stateMachine = stateMachine;
            _playerView = playerView;
            _stateMachineProxy = stateMachineProxy;
        }

        void IInitializable.Initialize()
        {
            _playerView.MainTriggerDetector.OnTriggerEntered += OnTriggerEntered;
            _playerView.MainTriggerDetector.OnTriggerExited += OnTriggerExited;

            _stateMachineProxy.SetStateMachine(_stateMachine);
        }

        void IFixedTickable.FixedTick()
        {
            _stateMachineModel.FixedTickState?.OnFixedTick();
        }

        void IDisposable.Dispose()
        {
            _playerView.MainTriggerDetector.OnTriggerEntered -= OnTriggerEntered;
            _playerView.MainTriggerDetector.OnTriggerExited -= OnTriggerExited;
        }

        private void OnTriggerEntered(Collider collider)
        {
            _stateMachineModel.TriggerEnterState?.OnTriggerEnter(collider);
        }

        private void OnTriggerExited(Collider collider)
        {
            _stateMachineModel.TriggerExitState?.OnTriggerExit(collider);
        }
    }
}