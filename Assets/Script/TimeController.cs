using System;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float timeMultiplier;//toc do thoi gian troi qua
    public float startHour;
    public TextMeshProUGUI timeText;//text hien gio
    public DateTime currentTime;//thoi gian hien tai

    public Light sunLight;
    public float sunriseHour;//gio sun moc
    public float sunsetHour;//gio sun lan
    private TimeSpan sunriseTime;//doi gio sun moc sang Time
    private TimeSpan sunsetTime;//doi gio sun lan sang Time

    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
    }
    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    private TimeSpan TimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;
        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }

    private void RotateSun()
    {
        float rotation;
        //thoi gian hien tai trong ngay co nam trong rise->set khong
        if (currentTime.TimeOfDay > sunriseTime
                        && currentTime.TimeOfDay < sunsetTime)
        {
            //thoi gian tu luc rise->set
            TimeSpan riseToSet = TimeDifference(sunriseTime, sunsetTime);
            //thoi gian troi qua tu khi rise
            TimeSpan timeSinceSunRise = TimeDifference(sunriseTime, currentTime.TimeOfDay);
            //phan tram thoi gian trong ngay da troi qua
            double percent = timeSinceSunRise.TotalMinutes / riseToSet.TotalMinutes;
            //tinh toan viec xoay vong
            rotation = Mathf.Lerp(0, 180, (float)percent);
        }
        else //thoi gian ban dem
        {
            //thoi gian tu luc set -> rise
            TimeSpan setToRise = TimeDifference(sunsetTime, sunriseTime);
            //thoi gian troi qua khi set
            TimeSpan timeSinceSunset = TimeDifference(sunsetTime, currentTime.TimeOfDay);
            //thoi gian phan tram da troi qua
            double percent = timeSinceSunset.TotalMinutes / setToRise.TotalMinutes;
            //xoay
            rotation = Mathf.Lerp(180, 360, (float)percent);
        }
        //quay theo truc X goc rotation
        sunLight.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
    }
}
