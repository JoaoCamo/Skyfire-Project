using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.UI
{
    public class PopUpTextManager : MonoBehaviour
    {
        [SerializeField] private PopUpText popUpTextPrefab;
        [SerializeField] private Transform parentTransform;
        private readonly List<PopUpText> _popUpTexts = new List<PopUpText>();

        private Camera _mainCamera;
        
        public static Action<Vector3, string, float, Color> RequestPopUpText;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            RequestPopUpText += RequestText;
        }

        private void OnDisable()
        {
            RequestPopUpText -= RequestPopUpText;
        }

        private void RequestText(Vector3 position, string text, float fontSize, Color color)
        {
            PopUpText popUpText = GetPopUpText();
            popUpText.SetText(_mainCamera.WorldToScreenPoint(position), text, fontSize, color);
        }

        private PopUpText GetPopUpText()
        {
            PopUpText popUpText = _popUpTexts.Find(p => !p.gameObject.activeSelf) ?? CreatePopUpText();
            return popUpText;
        }

        private PopUpText CreatePopUpText()
        {
            PopUpText popUpText = Instantiate(popUpTextPrefab, parentTransform);
            _popUpTexts.Add(popUpText);
            return popUpText;
        }
    }
}