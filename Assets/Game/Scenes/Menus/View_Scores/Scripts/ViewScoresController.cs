using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Game.Navigation;
using Game.Saves;
using Game.Static;

namespace Game.Menus
{
    public class ViewScoresController : MonoBehaviour
    {
        [SerializeField] private ScoreDisplayUI scoreDisplayPrefab;
        [SerializeField] private Transform scoreDisplayParent;
        [SerializeField] private Button resetScoresButton;
        [SerializeField] private Button returnButton;

        private readonly List<ScoreDisplayUI> _scoreDisplays = new List<ScoreDisplayUI>();

        private void Awake()
        {
            resetScoresButton.onClick.AddListener(ResetScores);
            returnButton.onClick.AddListener(() => NavigationController.RequestSceneUnload());
            LoadScores();
        }

        private void LoadScores()
        {
            GameData data = GameDataManager.GetSavedData();

            List<ScoreData> scores = data.scoreDatas.ToList();

            scores = scores.OrderByDescending(s => s.score).ToList();

            foreach (ScoreData scoreData in scores)
            {
                ScoreDisplayUI scoreDisplay = Instantiate(scoreDisplayPrefab, scoreDisplayParent);
                scoreDisplay.Initialize(scoreData);
                _scoreDisplays.Add(scoreDisplay);
            }
        }

        private void ResetScores()
        {
            GameDataManager.ResetScores();

            foreach (ScoreDisplayUI scoreDisplay in _scoreDisplays)
                Destroy(scoreDisplay.gameObject);

            _scoreDisplays.Clear();
            LoadScores();
        }
    }
}