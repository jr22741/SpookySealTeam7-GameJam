using TMPro;
using UnityEngine;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        private float _timeRemaining = 300f; 
        private bool _timerIsRunning = true;

        private void Update()
        {
            if (_timerIsRunning)
            {
                if (_timeRemaining > 0)
                {
                    _timeRemaining -= Time.deltaTime;
                    UpdateTimerDisplay(_timeRemaining);
                }
                else
                {
                    _timeRemaining = 0;
                    _timerIsRunning = false;
                    UpdateTimerDisplay(_timeRemaining);
                    // Optionally, you can add additional logic here for when the timer reaches zero
                }
            }
        }

        void UpdateTimerDisplay(float timeToDisplay)
        {
            timeToDisplay += 1; // Optional: To make the display countdown look smoother

            int minutes = Mathf.FloorToInt(timeToDisplay / 60);
            int seconds = Mathf.FloorToInt(timeToDisplay % 60);

            timerText.text = $"Timer: {minutes:00}:{seconds:00}";
        }
    }
}