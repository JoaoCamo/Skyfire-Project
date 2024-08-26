using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Navigation;
using Game.Saves;
using Game.Static;
using Game.Stage;

namespace Game.Menus
{
    public class ViewScoresController : MonoBehaviour
    {
        [SerializeField] private ScoreDisplayUI scoreDisplayPrefab;
        [SerializeField] private Transform scoreDisplayParent;
        [SerializeField] private TextMeshProUGUI titleMesh;
        [SerializeField] private TextMeshProUGUI idMesh;
        [SerializeField] private TextMeshProUGUI pilotMesh;
        [SerializeField] private TextMeshProUGUI scoreMesh;
        [SerializeField] private TextMeshProUGUI dateMesh;
        [SerializeField] private TextMeshProUGUI missionStatusMesh;
        [SerializeField] private TextMeshProUGUI missionDifficultyMesh;
        [SerializeField] private CanvasGroup mainCanvas;
        [SerializeField] private CanvasGroup infoCanvas;
        [SerializeField] private Button resetScoresButton;
        [SerializeField] private Button returnButton;

        private CanvasGroup _currentCanvas;
        private bool _onMainPage = true;

        private readonly List<ScoreDisplayUI> _scoreDisplays = new List<ScoreDisplayUI>();

        private void Awake()
        {
            resetScoresButton.onClick.AddListener(ResetScores);
            returnButton.onClick.AddListener(ReturnButtonOnClick);
            LoadScores();

            _currentCanvas = mainCanvas;
        }

        private void LoadScores()
        {
            GameData data = GameDataManager.GetSavedData();

            List<ScoreData> scores = data.scoreDatas.ToList();

            scores = scores.OrderByDescending(s => s.score).ToList();

            foreach (ScoreData scoreData in scores)
            {
                ScoreDisplayUI scoreDisplay = Instantiate(scoreDisplayPrefab, scoreDisplayParent);
                scoreDisplay.Initialize(scoreData, this);
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

        public void DisplayScoreInfo(ScoreData data)
        {
            idMesh.text = data.name;
            pilotMesh.text = GameDataManager.GetPlayerTypeNames(data.playerType);
            scoreMesh.text = data.score.ToString();
            dateMesh.text = data.date;
            missionStatusMesh.text = data.runStatus ? "SUCESS" : "DEFEAT";
            missionDifficultyMesh.text = GetDifficultyText(data.difficulty);
            resetScoresButton.gameObject.SetActive(false);

            ChangeCanvas(_currentCanvas, infoCanvas, "MISSION DATA");
            _currentCanvas = infoCanvas;
            _onMainPage = false;
        }

        private void ChangeCanvas(CanvasGroup canvasToHide, CanvasGroup canvasToShow, string newTitle)
        {
            titleMesh.text = newTitle;

            canvasToHide.alpha = 0;
            canvasToHide.interactable = false;
            canvasToHide.blocksRaycasts = false;

            canvasToShow.alpha = 1;
            canvasToShow.interactable = true;
            canvasToShow.blocksRaycasts = true;
        }

        private void ReturnButtonOnClick()
        {
            if (_onMainPage)
            {
                NavigationController.RequestSceneUnload();
            }
            else
            {
                ChangeCanvas(_currentCanvas, mainCanvas, "MISSION ARCHIVE");
                resetScoresButton.gameObject.SetActive(true);
                _currentCanvas = mainCanvas;
                _onMainPage = true;
            }
        }

        private string GetDifficultyText(DifficultyType difficultyType)
        {
            return difficultyType switch
            {
                DifficultyType.Easy => "EASY",
                DifficultyType.Normal => "NORMAL",
                DifficultyType.Hard => "HARD",
                DifficultyType.Lunatic => "LUNATIC",
                _ => ""
            };
        }
    }
}