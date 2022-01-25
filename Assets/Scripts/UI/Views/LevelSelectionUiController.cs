using System.Collections.Generic;
using Configuration.DataStructures;
using Signal.DataStructures;
using Signal.Implementation;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class LevelSelectionUiController : GuiController
    {
        [SerializeField] private LevelSelectionItem itemPrefab;
        [SerializeField] private Button backGameButton;
        [SerializeField] private RectTransform levelsRoot;

        private List<LevelSelectionItem> installedLevels;

        public override void Show(object data)
        {
            if (installedLevels == null &&
                data is List<LevelConfiguration> levels)
            {
                SetupLevels(levels);
            }

            backGameButton.onClick.AddListener(OnBackToMainMenu);
            gameObject.SetActive(true);
        }

        private void OnBackToMainMenu()
        {
            GuiSystem.HideView(GuiScreens.LevelSelection);
            GuiSystem.ShowView(GuiScreens.MainMenu);
        }

        private void SetupLevels(List<LevelConfiguration> levels)
        {
            installedLevels = new List<LevelSelectionItem>();
            foreach (var lvl in levels)
            {
                var item = Instantiate(itemPrefab, levelsRoot);
                item.Setup(lvl.number);
                item.EvLevelSelected += OnStartGame;
                item.gameObject.SetActive(true);
                installedLevels.Add(item);
            }
        }

        public override void Hide()
        {
            backGameButton.onClick.RemoveListener(OnBackToMainMenu);
            gameObject.SetActive(false);
        }

        private void OnStartGame(int level)
        {
            StaticSignalBus.DispatchSignal(
                SignalType.SelectLevel,
                new StartGameSignalData(level)
            );
        }
    }
}