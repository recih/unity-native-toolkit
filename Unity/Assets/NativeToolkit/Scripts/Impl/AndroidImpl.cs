#if UNITY_ANDROID
using System.IO;
using UnityEngine;

namespace NativeToolkitImpl
{
    public class AndroidImpl : BaseImpl
    {
        private AndroidJavaClass _obj;
        
        internal AndroidImpl()
        {
            _obj = new AndroidJavaClass("com.secondfury.nativetoolkit.Main");
        }
        
        public override string PrepareScreenshotPath(string albumName, string screenshotFilename)
        {
            string androidPath = Path.Combine(albumName, screenshotFilename);
            string path = Path.Combine(Application.persistentDataPath, androidPath);
            string pathonly = Path.GetDirectoryName(path);
            Directory.CreateDirectory(pathonly);
            return path;
        }

        public override SaveStatus SaveToGallery(string path)
        {
            return (SaveStatus)_obj.CallStatic<int>("addImageToGallery", path);
        }

        public override void PickImage()
        {
            _obj.CallStatic("pickImageFromGallery");
        }

        public override void TakeCameraShot()
        {
            _obj.CallStatic("takeCameraShot");
        }

        public override void PickContact()
        {
            _obj.CallStatic("pickContact");
        }

        public override void SendEmail(string subject, string body, string pathToImageAttachment, string to, string cc,
            string bcc)
        {
            _obj.CallStatic("sendEmail", to, cc, bcc, subject, body, pathToImageAttachment);
        }

        public override void ShowConfirm(string title, string message, string positiveBtnText, string negativeBtnText)
        {
            _obj.CallStatic("showConfirm", title, message, positiveBtnText, negativeBtnText);
        }

        public override void ShowAlert(string title, string message, string btnText)
        {
            _obj.CallStatic("showAlert", title, message, btnText);
        }

        public override void RateApp(string title, string message, string positiveBtnText, string neutralBtnText, string negativeBtnText,
            string appleId)
        {
            _obj.CallStatic("rateThisApp", title, message, positiveBtnText, neutralBtnText, negativeBtnText);
        }

        public override void StartLocation()
        {
            _obj.CallStatic("startLocation");
        }

        public override double GetLongitude()
        {
            return _obj.CallStatic<double>("getLongitude");
        }

        public override double GetLatitude()
        {
            return _obj.CallStatic<double>("getLatitude");
        }

        public override string GetCountryCode()
        {
            return _obj.CallStatic<string>("getLocale");
        }

        public override void ScheduleLocalNotification(string title, string message, int id, int delayInMinutes, string sound, bool vibrate,
            string smallIcon, string largeIcon)
        {
            _obj.CallStatic("scheduleLocalNotification", title, message, id, delayInMinutes, sound, vibrate, smallIcon, largeIcon);
        }

        public override void ClearLocalNotification(int id)
        {
            _obj.CallStatic("clearLocalNotification", id);
        }

        public override void ClearAllLocalNotifications()
        {
            _obj.CallStatic("clearAllLocalNotifications");
        }

        public override bool WasLaunchedFromNotification()
        {
            return _obj.CallStatic<bool>("wasLaunchedFromNotification");
        }
    }
}
#endif
