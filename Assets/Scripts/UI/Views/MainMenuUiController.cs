using Signal.DataStructures;
using Signal.Implementation;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MainMenuUiController : GuiController
    {
        [SerializeField] private Button startGameButton;

        public override void Show(object data)
        {
            startGameButton.onClick.AddListener(OnStartGame);
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            startGameButton.onClick.RemoveListener(OnStartGame);
        }

        private void OnStartGame()
        {
            StaticSignalBus.DispatchSignal(SignalType.SelectLevel, new LevelSelectSignalData());
        }
    }
}