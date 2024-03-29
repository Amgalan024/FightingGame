﻿using MVC.Controllers;
using MVC.Gameplay.Controllers;
using MVC.Gameplay.Models;
using MVC.Gameplay.Models.Player;
using MVC.Models;
using MVC.StateMachine.States;
using MVC_Pattern.Scripts.Gameplay.Models.StateMachineModels.StateModels;
using MVC_Pattern.Scripts.Gameplay.Services;
using VContainer;
using VContainer.Unity;

namespace MVC.Gameplay.Services
{
    public class FightScopesFactory
    {
        private readonly LifetimeScope _gameplayLifeTimeScope;

        private readonly FightScopesStorage _storage;

        public FightScopesFactory(LifetimeScope gameplayLifeTimeScope, FightScopesStorage storage)
        {
            _gameplayLifeTimeScope = gameplayLifeTimeScope;
            _storage = storage;
        }

        public void CreateCameraScope()
        {
            var scope = _gameplayLifeTimeScope.CreateChild(builder =>
            {
                builder.RegisterEntryPoint<CameraController>();
            });

            scope.name = "CameraControllerScope";

            _storage.CameraControllerScope = scope;
        }

        public void CreatePlayerScope(PlayerContainer playerContainer,
            PlayerHUDView playerHUD)
        {
            var scope = _gameplayLifeTimeScope.CreateChild(builder =>
            {
                builder.RegisterInstance(playerContainer);
                builder.RegisterInstance(playerHUD);

                builder.Register<StatesContainer>(Lifetime.Scoped);
                builder.Register<StateMachineModel>(Lifetime.Scoped);

                builder.Register<StateMachine.StateMachine>(Lifetime.Scoped).AsImplementedInterfaces();

                builder.RegisterEntryPoint<PlayerStateMachineController>(Lifetime.Scoped);
                builder.RegisterEntryPoint<PlayerInputController>(Lifetime.Scoped);
                builder.RegisterEntryPoint<PlayerComboController>(Lifetime.Scoped);
                builder.RegisterEntryPoint<PlayerAnimatorController>(Lifetime.Scoped);
                builder.RegisterEntryPoint<PlayerInteractionsController>(Lifetime.Scoped);
                builder.RegisterEntryPoint<PlayerMainController>(Lifetime.Scoped);
                builder.RegisterEntryPoint<PlayerHUDController>(Lifetime.Scoped);

                BuildStates(builder);

                BuildStateModels(builder);
            });

            scope.name = "PlayerScope";

            _storage.PlayerLifetimeScopes.Add(scope);
        }

        private void BuildStates(IContainerBuilder builder)
        {
            builder.Register<BlockState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<ComboState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<CrouchState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<LoseState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<WinState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<FallState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<IdleState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<JumpState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<KickState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<PunchState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<RunState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<DashForwardState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<DashBackwardState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<StunnedState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        }

        private void BuildStateModels(IContainerBuilder builder)
        {
            builder.Register<FallStateModel>(Lifetime.Scoped);
            builder.Register<JumpStateModel>(Lifetime.Scoped);
            builder.Register<RunStateModel>(Lifetime.Scoped);
            builder.Register<ComboStateModel>(Lifetime.Scoped);
        }
    }
}