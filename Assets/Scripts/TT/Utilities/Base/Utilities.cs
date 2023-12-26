using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class Utilities
    {
        public static string MoneyToString(long money)
        {
            if (money >= 1000000000)
            {
                return ((float)money / 1000000000).ToString("#.##") + "B";
            }
            if (money >= 1000000)
            {
                return ((float)money / 1000000).ToString("#.##") + "M";
            }
            if (money >= 1000)
            {
                return ((float)money / 1000).ToString("#.##") + "K";
            }
            return money.ToString();
        }

        public string ConvertToHH_MM_DD(int totalSeconds)
        {
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            string formattedTime = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
            return formattedTime;
        }

        public static string FormatTime(int hours, int minutes, int seconds)
        {
            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }
}
