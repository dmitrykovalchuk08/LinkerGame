using Signal.DataStructures;
using Signal.Implementation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class GameoverUiController : GuiController
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button selectLevelButton;
        [SerializeField] private TextMeshProUGUI titleLabel;

        public override void Show(object data)
        {
            restartButton.onClick.AddListener(OnRestart);
            selectLevelButton.onClick.AddListener(OnSelectLevel);
            titleLabel.text = ((bool?) data).Value ? "VICTORY!" : "YOU LOST!";
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            restartButton.onClick.RemoveListener(OnRestart);
            selectLevelButton.onClick.RemoveListener(OnSelectLevel);
        }

        private void OnRestart()
        {
            GuiSystem.HideView(GuiScreens.GameOver);
            StaticSignalBus.DispatchSignal(
                SignalType.RestartGame,
                new RestartGameSignalData());
        }

        private void OnSelectLevel()
        {
            GuiSystem.HideView(GuiScreens.GameOver);
            GuiSystem.ShowView(GuiScreens.LevelSelection);
        }
    }
}