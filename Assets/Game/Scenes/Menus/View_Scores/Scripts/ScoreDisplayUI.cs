using UnityEngine;
using TMPro;
using Game.Saves;
using Game.Static;

namespace Game.Menus
{
    public class ScoreDisplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameMesh;
        [SerializeField] private TextMeshProUGUI playerTypeMesh;
        [SerializeField] private TextMeshProUGUI scoreMesh;
        [SerializeField] private TextMeshProUGUI dateMesh;

        public void Initialize(ScoreData data)
        {
            nameMesh.text = data.name;
            playerTypeMesh.text = GameDataManager.GetPlayerTypeNames(data.playerType);
            scoreMesh.text = data.score.ToString();
            dateMesh.text = data.date;
        }
    }
}