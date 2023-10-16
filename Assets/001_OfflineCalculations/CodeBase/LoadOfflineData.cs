using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _001_OfflineCalculations.CodeBase
{
    public class LoadOfflineData : MonoBehaviour
    {
        private const string LAST_ONLINE_DATE = "LastOnlineDate";
        
        [SerializeField] private TMP_Text _currentResources;
        [SerializeField] private int _maxResources = 240;
        [SerializeField] private int _resourcesByTime = 10;
        
        private TimeSpan _difference;
        
        private void Start() => 
            ReadLoadData();

        private void ReadLoadData()
        {
            if (PlayerPrefs.HasKey(LAST_ONLINE_DATE))
                ReadDate();
            else
                SaveDateToPrefs();
        }

        private void ReadDate() => 
            CalculateOfflineTime(DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString(LAST_ONLINE_DATE))));

        private void CalculateOfflineTime(DateTime lastDate)
        {
            _difference = DateTime.Now.Subtract(lastDate);
            Debug.Log("Offline time: " + _difference);
            
            SaveDateToPrefs();
            CalculateResources(_difference);
        }

        private void CalculateResources(TimeSpan difference)
        {
            int minutes = difference.Days * 24 * 60 +
                          difference.Hours * 60 +
                          difference.Minutes;

            if (minutes <= 0) 
                return;

            int resources = minutes * _resourcesByTime;

            if (resources > _maxResources)
                resources = _maxResources;
            
            UpdateInfo(resources);
        }

        private void UpdateInfo(int count) => 
            _currentResources.text = count.ToString();

        private static void SaveDateToPrefs() => 
            PlayerPrefs.SetString(LAST_ONLINE_DATE, DateTime.Now.ToBinary().ToString());

        private void OnApplicationQuit() => 
            SaveDateToPrefs();
    }
}
