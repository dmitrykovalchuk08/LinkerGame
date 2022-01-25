using Configuration.DataStructures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class GameplayUIGoalController : MonoBehaviour
    {
        public BlockType BlockType { get; private set; }

        [SerializeField] private TextMeshProUGUI countLabel;
        [SerializeField] private Image goalsIcon;
        [SerializeField] private Image completedIcon;

        public void Init(LevelCondition condition, Sprite sprite)
        {
            countLabel.text = condition.Quantity.ToString();
            goalsIcon.sprite = sprite;
            UpdateLabelsState(condition.Quantity);
            BlockType = condition.BlockType;
        }

        private void UpdateLabelsState(int count)
        {
            completedIcon.gameObject.SetActive(count <= 0);
            countLabel.gameObject.SetActive(count > 0);
        }

        public void UpdateCount(int count)
        {
            countLabel.text = count.ToString("N0");
            UpdateLabelsState(count);
        }
    }
}