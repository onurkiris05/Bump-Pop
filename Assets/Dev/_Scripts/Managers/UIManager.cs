using UnityEngine;

namespace Game.Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject uiCanvas;
        [SerializeField] private GameObject levelFailedCanvas;
        [SerializeField] private GameObject levelCompleteCanvas;

        private void OnEnable()
        {
            GameManager.Instance.OnInitializeLevel += Init;
            GameManager.Instance.OnNextLevel += OnNextLevel;
            GameManager.Instance.OnLevelFailed += OnLevelFailed;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnInitializeLevel -= Init;
            GameManager.Instance.OnNextLevel -= OnNextLevel;
            GameManager.Instance.OnLevelFailed -= OnLevelFailed;
        }

        private void Init()
        {
            uiCanvas.SetActive(true);
            levelFailedCanvas.SetActive(false);
            levelCompleteCanvas.SetActive(false);
        }

        private void OnNextLevel()
        {
            levelCompleteCanvas.SetActive(true);
            uiCanvas.SetActive(false);
            levelFailedCanvas.SetActive(false);
        }

        private void OnLevelFailed()
        {
            levelFailedCanvas.SetActive(true);
            uiCanvas.SetActive(false);
            levelCompleteCanvas.SetActive(false);
        }
    }
}