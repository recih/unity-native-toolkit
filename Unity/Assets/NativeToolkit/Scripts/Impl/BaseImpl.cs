using UnityEngine;

namespace NativeToolkitImpl
{
    public abstract class BaseImpl : INativeToolkit
    {
        public virtual string GetText(string key)
        {
            return key;
        }

        public virtual string PrepareScreenshotPath(string albumName, string screenshotFilename)
        {
            Debug.Log($"[NativeToolkit] PrepareScreenshotPath: {albumName}, {screenshotFilename}");
            return Application.persistentDataPath + "/" + screenshotFilename;
        }

        public virtual SaveStatus SaveToGallery(string path)
        {
            Debug.Log($"[NativeToolkit] SaveToGallery: {path}");
            return SaveStatus.SAVED;
        }

        public virtual void SetNoBackupFlag(string path)
        {
            Debug.Log($"[NativeToolkit] SetNoBackupFlag: {path}");
        }

        public virtual void PickImage()
        {
            Debug.Log("[NativeToolkit] PickImage");
        }

        public virtual void TakeCameraShot()
        {
            Debug.Log("[NativeToolkit] TakeCameraShot");
        }

        public virtual void PickContact()
        {
            Debug.Log("[NativeToolkit] PickContact");
        }

        public virtual void SendEmail(string subject, string body, string pathToImageAttachment, string to, string cc,
            string bcc)
        {
            Debug.Log($"[NativeToolkit] SendEmail: {subject}, {body}, {pathToImageAttachment}, {to}, {cc}, {bcc}");
        }

        public virtual void ShowConfirm(string title, string message, string positiveBtnText, string negativeBtnText)
        {
            Debug.Log($"[NativeToolkit] ShowConfirm: {title}, {message}, {positiveBtnText}, {negativeBtnText}");
        }

        public virtual void ShowAlert(string title, string message, string btnText)
        {
            Debug.Log($"[NativeToolkit] ShowAlert: {title}, {message}, {btnText}");
        }

        public virtual void RateApp(string title, string message, string positiveBtnText, string neutralBtnText, string negativeBtnText,
            string appleId)
        {
            Debug.Log(
                $"[NativeToolkit] RateApp: {title}, {message}, {positiveBtnText}, {neutralBtnText}, {negativeBtnText}, {appleId}");
        }

        public virtual void StartLocation()
        {
            Debug.Log("[NativeToolkit] StartLocation");
        }

        public virtual double GetLongitude()
        {
            Debug.Log("[NativeToolkit] GetLongitude");
            return 0;
        }

        public virtual double GetLatitude()
        {
            Debug.Log("[NativeToolkit] GetLatitude");
            return 0;
        }

        public virtual string GetCountryCode()
        {
            Debug.Log("[NativeToolkit] GetCountryCode");
            return string.Empty;
        }

        public virtual string GetLanguage()
        {
            var lang = Application.systemLanguage;
			switch (lang) {
				case SystemLanguage.Afrikaans: return "af";
				case SystemLanguage.Arabic: return "ar";
				case SystemLanguage.Basque: return "eu";
				case SystemLanguage.Belarusian: return "by";
				case SystemLanguage.Bulgarian: return "bg";
				case SystemLanguage.Catalan: return "ca";
				case SystemLanguage.Chinese: return "zh";
				case SystemLanguage.Czech: return "cs";
				case SystemLanguage.Danish: return "da";
				case SystemLanguage.Dutch: return "nl";
				case SystemLanguage.English: return "en";
				case SystemLanguage.Estonian: return "et";
				case SystemLanguage.Faroese: return "fo";
				case SystemLanguage.Finnish: return "fi";
				case SystemLanguage.French: return "fr";
				case SystemLanguage.German: return "de";
				case SystemLanguage.Greek: return "el";
				case SystemLanguage.Hebrew: return "iw";
				case SystemLanguage.Hungarian: return "HU";
				case SystemLanguage.Icelandic: return "is";
				case SystemLanguage.Indonesian: return "in";
				case SystemLanguage.Italian: return "it";
				case SystemLanguage.Japanese: return "ja";
				case SystemLanguage.Korean: return "ko";
				case SystemLanguage.Latvian: return "lv";
				case SystemLanguage.Lithuanian: return "lt";
				case SystemLanguage.Norwegian: return "no";
				case SystemLanguage.Polish: return "pl";
				case SystemLanguage.Portuguese: return "pt";
				case SystemLanguage.Romanian: return "ro";
				case SystemLanguage.Russian: return "ru";
				case SystemLanguage.SerboCroatian: return "sh";
				case SystemLanguage.Slovak: return "sk";
				case SystemLanguage.Slovenian: return "sl";
				case SystemLanguage.Spanish: return "es";
				case SystemLanguage.Swedish: return "sv";
				case SystemLanguage.Thai: return "th";
				case SystemLanguage.Turkish: return "tr";
				case SystemLanguage.Ukrainian: return "uk";
				case SystemLanguage.Unknown: return "en";
				case SystemLanguage.Vietnamese: return "vi";
				case SystemLanguage.ChineseSimplified: return "zh-CN";
				case SystemLanguage.ChineseTraditional: return "zh-TW";
				default:
					return "en";
			}
        }

        public virtual void ScheduleLocalNotification(string title, string message, int id, int delayInMinutes, string sound, bool vibrate,
            string smallIcon, string largeIcon)
        {
            Debug.Log(
                $"[NativeToolkit] ScheduleLocalNotification: {title}, {message}, {id}, {delayInMinutes}, {sound}, {vibrate}, {smallIcon}, {largeIcon}");
        }

        public virtual void ClearLocalNotification(int id)
        {
            Debug.Log($"[NativeToolkit] ClearLocalNotification: {id}");
        }

        public virtual void ClearAllLocalNotifications()
        {
            Debug.Log("[NativeToolkit] ClearAllLocalNotifications");
        }

        public virtual bool WasLaunchedFromNotification()
        {
            Debug.Log("[NativeToolkit] WasLaunchedFromNotification");
            return false;
        }
    }
}