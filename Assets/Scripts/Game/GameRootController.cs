using System;
using Configuration.DataStructures;
using Configuration.Implementation;
using Configuration.Interfaces;
using ContentProvider;
using ContentProvider.Implementation;
using CoordinatesConvertor;
using Game.Level.Interfaces;
using Game.Level.LevelController;
using Game.Level.LevelVisual;
using PlayerInput;
using Signal.DataStructures;
using Signal.Implementation;
using Signal.Interfaces;
using UI;
using UnityEngine;

namespace Game
{
    public class GameRootController : MonoBehaviour, ISignalListener
    {
        [SerializeField] private LevelView levelView;

        private ILevelController levelController;
        private LevelsData levelsList;
        private LevelConfiguration selectedLvlConfig;

        private async void Start()
        {
            try
            {
                IConfigurationProvider configProvider = new ScriptableObjectConfigProvider();
                IVisualContentProvider contentProvider = new ScriptableObjectContentProvider();

                var visualData = await configProvider.GetVisualConfiguration();
                ICoordinatesProvider coordinatesProvider = new CoordinatesProvider(visualData);
                InputModule.GenerateInput(coordinatesProvider);
                levelView.Initialize(coordinatesProvider, contentProvider, visualData);
                levelController = new LevelController();
                await levelController.Initialize(levelView);
                levelsList = await configProvider.GetLevelsConfiguration();

                GuiSystem.ShowView(GuiScreens.MainMenu);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            Subscribe();
        }

        private void Subscribe()
        {
            StaticSignalBus.Subscribe(SignalType.StartGame, this);
            StaticSignalBus.Subscribe(SignalType.SelectLevel, this);
            StaticSignalBus.Subscribe(SignalType.RestartGame, this);
        }

        private async void PlayLevel(int level)
        {
            selectedLvlConfig = levelsList.Data.Find(l => l.number == level);
            if (selectedLvlConfig == null)
            {
                Debug.LogError($"No Level with number {level}");
                return;
            }

            GuiSystem.ShowView(GuiScreens.GamePlay, selectedLvlConfig);
            GuiSystem.HideView(GuiScreens.LevelSelection);
            try
            {
                var gameplay = await levelController.PlayLevel(selectedLvlConfig);
                await gameplay;
                Debug.Log($"LEVEL RESULT: {gameplay.Result}");
                GuiSystem.ShowView(GuiScreens.GameOver, gameplay.Result);
                GuiSystem.HideView(GuiScreens.GamePlay);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void HandleSignal(SignalData data)
        {
            switch (data.Key)
            {
                case SignalType.StartGame:
                {
                    PlayLevel(((StartGameSignalData) data).SelectedLevel);
                    break;
                }
                case SignalType.RestartGame:
                    PlayLevel(selectedLvlConfig.number);
                    break;
                case SignalType.SelectLevel:
                {
                    GuiSystem.ShowView(GuiScreens.LevelSelection, levelsList.Data);
                    GuiSystem.HideView(GuiScreens.MainMenu);
                    break;
                }
                case SignalType.MainMenu:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}