﻿using Cysharp.Threading.Tasks;
using MVC.Menu.Configs;
using MVC.Utils.Disposable;
using MVC_Pattern.Scripts.MainMenu.Views;
using MVC_Pattern.Scripts.Utils.LoadingScreen.Services;
using MVC_Pattern.Scripts.Utils.LoadingScreen.Views;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace MVC_Pattern.Scripts.MainMenu.Controllers
{
    public class MainMenuController : DisposableWithCts, IInitializable
    {
        private readonly MainMenuView _mainMenuView;
        private readonly MainMenuConfig _mainMenuConfig;

        private readonly LoadingScreenService _screenService;

        public MainMenuController(MainMenuView mainMenuView, MainMenuConfig mainMenuConfig,
            LoadingScreenService screenService)
        {
            _mainMenuView = mainMenuView;
            _mainMenuConfig = mainMenuConfig;
            _screenService = screenService;
        }

        void IInitializable.Initialize()
        {
            _mainMenuView.PlayerVsPlayer.onClick.AddListener(() => LoadPvPCharacterSelectScene().Forget());
        }

        public override void Dispose()
        {
            base.Dispose();

            _mainMenuView.PlayerVsPlayer.onClick.RemoveAllListeners();
        }

        private async UniTaskVoid LoadPvPCharacterSelectScene()
        {
            await _screenService.ShowAsync<MenuLoadingScreenView>(Cts.Token);

            await Addressables.LoadSceneAsync(_mainMenuConfig.PlayerVsPlayerScene);

            await _screenService.HideAsync<MenuLoadingScreenView>(Cts.Token);
        }
    }
}