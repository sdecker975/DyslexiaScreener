using System;
using UnityEngine;
using UnityEngine.UI;

namespace MHLab.Patch.Launcher.Scripts.UI
{
    public sealed class Dialog : MonoBehaviour
    {
        public Text MainMessage;
        public Text DetailsMessage;
        public Button CloseButton;
        public Button ContinueButton;

        private void ClearListeners()
        {
            CloseButton.onClick.RemoveAllListeners();
            ContinueButton.onClick.RemoveAllListeners();
        }
        
        public void ShowDialog(string main, string detail, Action onClose, Action onContinue)
        {
            ClearListeners();
            
            MainMessage.text = main;
            DetailsMessage.text = detail;
            CloseButton.onClick.AddListener(() =>
            {
                CloseButton.onClick.RemoveAllListeners();
                onClose?.Invoke();
                gameObject.SetActive(false);
            });
            CloseButton.gameObject.SetActive(true);
            
            ContinueButton.onClick.AddListener(() =>
            {
                ContinueButton.onClick.RemoveAllListeners();
                onContinue?.Invoke();
                gameObject.SetActive(false);
            });
            ContinueButton.gameObject.SetActive(true);
            
            gameObject.SetActive(true);
        }

        public void ShowCloseDialog(string main, string detail, Action onClose)
        {
            ClearListeners();
            
            MainMessage.text = main;
            DetailsMessage.text = detail;
            CloseButton.onClick.AddListener(() =>
            {
                CloseButton.onClick.RemoveAllListeners();
                onClose?.Invoke();
                gameObject.SetActive(false);
            });
            CloseButton.gameObject.SetActive(true);
            
            ContinueButton.gameObject.SetActive(false);
            
            gameObject.SetActive(true);
        }

        public void CloseDialog()
        {
            gameObject.SetActive(false);
        }
    }
}