using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class LevelSelectionItem : MonoBehaviour
    {
        public event Action<int> EvLevelSelected;

        [SerializeField] private TextMeshProUGUI levelLabel;
        [SerializeField] private Button buttonStart;

        private int levelNumber;

        public void Setup(int level)
        {
            var tr = transform;
            tr.localScale = Vector3.one;
            tr.localPosition = Vector3.zero;
            levelNumber = level;
            levelLabel.text = $"level\n{level}";
            buttonStart.onClick.AddListener(LevelSelected);
        }

        private void LevelSelected()
        {
            EvLevelSelected?.Invoke(levelNumber);
        }
    }
}