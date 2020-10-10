#if UNITY_IOS
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.iOS;

namespace NativeToolkitImpl
{
    public class IosImpl : BaseImpl
    {
        [DllImport("__Internal")]
        private static extern string getText(string key);
        
        [DllImport("__Internal")]
        private static extern int saveToGallery(string path);

        [DllImport("__Internal")]
        private static extern void pickImage();

        [DllImport("__Internal")]
        private static extern void openCamera();

        [DllImport("__Internal")]
        private static extern void pickContact();

        [DllImport("__Internal")]
        private static extern string getLocale();

        [DllImport("__Internal")]
        private static extern string getLanguage();

        [DllImport("__Internal")]
        private static extern void sendEmail(string to, string cc, string bcc, string subject, string body, string imagePath);

        [DllImport("__Internal")]
        private static extern void scheduleLocalNotification(string id, string title, string message, int delayInMinutes, string sound);

        [DllImport("__Internal")]
        private static extern void clearLocalNotification(string id);

        [DllImport("__Internal")]
        private static extern void clearAllLocalNotifications();

        [DllImport("__Internal")]
        private static extern bool wasLaunchedFromNotification();

        [DllImport("__Internal")]
        private static extern void rateApp(string title, string message, string positiveBtnText, string neutralBtnText, string negativeBtnText, string appleId);

        [DllImport("__Internal")]
        private static extern void showConfirm(string title, string message, string positiveBtnText, string negativeBtnText);

        [DllImport("__Internal")]
        private static extern void showAlert(string title, string message, string confirmBtnText);

        [DllImport("__Internal")]
        private static extern void startLocation();

        [DllImport("__Internal")]
        private static extern double getLongitude();

        [DllImport("__Internal")]
        private static extern double getLatitude();

        public override string GetText(string key)
        {
            return getText(key);
        }

        public override string PrepareScreenshotPath(string albumName, string screenshotFilename)
        {
            return Application.persistentDataPath + "/" + screenshotFilename;
        }
        
        public override SaveStatus SaveToGallery(string path)
        {
            return (SaveStatus)saveToGallery(path);
        }

        public override void SetNoBackupFlag(string path)
        {
            Device.SetNoBackupFlag(path);
        }

        public override void PickImage()
        {
            pickImage();
        }

        public override void TakeCameraShot()
        {
            openCamera();
        }

        public override void PickContact()
        {
            pickContact();
        }

        public override void SendEmail(string subject, string body, string pathToImageAttachment, string to, string cc,
            string bcc)
        {
            sendEmail(to, cc, bcc, subject, body, pathToImageAttachment);
        }

        public override void ShowConfirm(string title, string message, string positiveBtnText, string negativeBtnText)
        {
            showConfirm(title, message, positiveBtnText, negativeBtnText);
        }

        public override void ShowAlert(string title, string message, string btnText)
        {
            showAlert(title, message, btnText);
        }

        public override void RateApp(string title, string message, string positiveBtnText, string neutralBtnText, string negativeBtnText,
            string appleId)
        {
            if(!string.IsNullOrEmpty(appleId))
                rateApp(title, message, positiveBtnText, neutralBtnText, negativeBtnText, appleId);
        }

        public override void StartLocation()
        {
            startLocation();
        }
        
        public override double GetLongitude()
        {
            return getLongitude();
        }

        public override double GetLatitude()
        {
            return getLatitude();
        }

        public override string GetCountryCode()
        {
            return getLocale();
        }

        public override string GetLanguage()
        {
            return getLanguage();
        }

        public override void ScheduleLocalNotification(string title, string message, int id, int delayInMinutes, string sound, bool vibrate,
            string smallIcon, string largeIcon)
        {
            scheduleLocalNotification(id.ToString(), title, message, delayInMinutes, sound);
        }

        public override void ClearLocalNotification(int id)
        {
            clearLocalNotification(id.ToString());
        }

        public override void ClearAllLocalNotifications()
        {
            clearAllLocalNotifications();
        }

        public override bool WasLaunchedFromNotification()
        {
            return wasLaunchedFromNotification();
        }
    }
}
#endif
