using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Manager
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private GameObject uiCanvas;
        [SerializeField] private GameObject levelFailedCanvas;
        [SerializeField] private GameObject levelCompleteCanvas;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private TextMeshProUGUI levelText;

        #region UNITY EVENTS

        private void OnEnable()
        {
            GameManager.Instance.OnInitializeLevel += Init;
            GameManager.Instance.OnNextLevel += OnNextLevel;
            GameManager.Instance.OnLevelFailed += OnLevelFailed;
            GameManager.Instance.OnLevelProgressUpdate += SetProgress;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnInitializeLevel -= Init;
            GameManager.Instance.OnNextLevel -= OnNextLevel;
            GameManager.Instance.OnLevelFailed -= OnLevelFailed;
            GameManager.Instance.OnLevelProgressUpdate -= SetProgress;
        }

        private void Start()
        {
            SetProgress(0f);
            SetLevelText();
        }

        #endregion

        #region PRIVATE METHODS

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

        private void SetProgress(float value)
        {
            progressSlider.value = value;
        }

        private void SetLevelText()
        {
            levelText.text = $"LEVEL {SceneManager.GetActiveScene().buildIndex + 1}";
        }

        #endregion
    }
}