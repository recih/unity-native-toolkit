using System;
using NativeToolkitImpl;

public interface INativeToolkit
{
    string GetText(string key);
    string PrepareScreenshotPath(string albumName, string screenshotFilename);
    SaveStatus SaveToGallery(string path);
    void SetNoBackupFlag(string path);
    void PickImage();
    void TakeCameraShot();
    void PickContact();
    void SendEmail(string subject, string body, string pathToImageAttachment, string to, string cc, string bcc);
    void ShowConfirm(string title, string message, string positiveBtnText, string negativeBtnText);
    void ShowAlert(string title, string message, string btnText);
    void RateApp(string title, string message, string positiveBtnText, string neutralBtnText, string negativeBtnText,
        string appleId);
    void StartLocation();
    double GetLongitude();
    double GetLatitude();
    string GetCountryCode();
    string GetLanguage();
    void ScheduleLocalNotification(string title, string message, int id, int delayInMinutes, string sound,
        bool vibrate, string smallIcon, string largeIcon);
    void ClearLocalNotification(int id);
    void ClearAllLocalNotifications();
    bool WasLaunchedFromNotification();
}
