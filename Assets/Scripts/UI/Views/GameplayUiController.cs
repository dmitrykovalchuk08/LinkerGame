using System.Collections.Generic;
using Configuration.DataStructures;
using Signal.DataStructures;
using Signal.Implementation;
using Signal.Interfaces;
using TMPro;
using UI.UIIconsProvider.Implementation;
using UnityEngine;

namespace UI.Views
{
    public class GameplayUiController : GuiController, ISignalListener
    {
        [SerializeField] private TextMeshProUGUI moves;
        [SerializeField] private Transform goalsRoot;
        [SerializeField] private GameplayUIGoalController goalPrefab;

        private ScriptableObjectIconsProvider iconsProvider;
        private List<GameplayUIGoalController> goals;

        public GameplayUiController(GameplayUIGoalController goalPrefab)
        {
            this.goalPrefab = goalPrefab;
        }

        public override void Show(object data)
        {
            if (iconsProvider == null)
            {
                iconsProvider = ScriptableObjectIconsProvider.GetInstance();
            }

            if (data is LevelConfiguration cfg)
            {
                moves.text = cfg.Moves.ToString();
                goals = new List<GameplayUIGoalController>();
                foreach (var condition in cfg.WinConditions)
                {
                    CreateCondition(condition);
                }
            }

            StaticSignalBus.Subscribe(SignalType.LevelGoalUpdated, this);
            StaticSignalBus.Subscribe(SignalType.MovesUpdated, this);
            gameObject.SetActive(true);
        }

        private void CreateCondition(LevelCondition condition)
        {
            var go = Instantiate(goalPrefab.gameObject, goalsRoot);
            var goal = go.GetComponent<GameplayUIGoalController>();
            goal.Init(condition, iconsProvider.GetIcon(condition.BlockType.ToString()));
            goal.gameObject.SetActive(true);
            goals.Add(goal);
        }

        public override void Hide()
        {
            foreach (var goal in goals)
            {
                goal.gameObject.SetActive(false);
                Destroy(goal);
            }

            goals.Clear();
            gameObject.SetActive(false);
            StaticSignalBus.Unsubscribe(SignalType.LevelGoalUpdated, this);
            StaticSignalBus.Unsubscribe(SignalType.MovesUpdated, this);
        }

        public void HandleSignal(SignalData data)
        {
            switch (data.Key)
            {
                case SignalType.LevelGoalUpdated:
                {
                    if (!(data is LevelGoalUpdatedData typedData)) return;
                    var gls = goals.Find(
                        g => g.BlockType == typedData.LevelGoal.Condition.BlockType);
                    gls.UpdateCount(typedData.LevelGoal.Condition.Quantity -
                                    typedData.LevelGoal.Quantity);
                    break;
                }
                case SignalType.MovesUpdated:
                    moves.text = (((MovesUpdatedData) data).MovesLimit -
                                  ((MovesUpdatedData) data).MovesValue).ToString();
                    break;
            }
        }
    }
}