using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using NativeToolkitImpl;
using UnityEngine;

public class NativeToolkit : MonoBehaviour {

	enum ImageType { IMAGE, SCREENSHOT };
	
	public static event Action<Texture2D> OnScreenshotTaken;
	public static event Action<string> OnScreenshotSaved;
	public static event Action<string> OnImageSaved;
	public static event Action<Texture2D, string> OnImagePicked;
	public static event Action<bool> OnDialogComplete;
	public static event Action<string> OnRateComplete;
	public static event Action<Texture2D, string> OnCameraShotComplete;
	public static event Action<string, string, string> OnContactPicked;
	
	private static NativeToolkit instance = null;
	private static GameObject go; 
	private INativeToolkit _impl;

    //=============================================================================
    // Init singleton
    //=============================================================================

    public static NativeToolkit Instance 
	{
		get {
			if(instance == null)
			{
				go = new GameObject();
				go.name = "NativeToolkit";
				instance = go.AddComponent<NativeToolkit>();
				instance._impl = ImplFactory.CreateImpl();
			}
			
			return instance; 
		}
	}

	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
		}
	}


	//=============================================================================
	// Grab and save screenshot
	//=============================================================================

	public static void SaveScreenshot(string fileName, string albumName = "MyScreenshots", string fileType = "jpg", Rect screenArea = default(Rect))
	{
		Debug.Log("Save screenshot to gallery " + fileName);

		if(screenArea == default(Rect))
			screenArea = new Rect(0, 0, Screen.width, Screen.height);

		Instance.StartCoroutine(Instance.GrabScreenshot(fileName, albumName, fileType, screenArea));
	}
	
	IEnumerator GrabScreenshot(string fileName, string albumName, string fileType, Rect screenArea)
	{
		yield return new WaitForEndOfFrame();

		Texture2D texture = new Texture2D ((int)screenArea.width, (int)screenArea.height, TextureFormat.RGB24, false);
		texture.ReadPixels (screenArea, 0, 0);
		texture.Apply ();
		
		byte[] bytes;
		string fileExt;
		
		if(fileType == "png")
		{
			bytes = texture.EncodeToPNG();
			fileExt = ".png";
		}
		else
		{
			bytes = texture.EncodeToJPG();
			fileExt = ".jpg";
		}

		if (OnScreenshotTaken != null)
			OnScreenshotTaken (texture);
		else
			Destroy (texture);
		
		string date = DateTime.Now.ToString("hh-mm-ss_dd-MM-yy");
		string screenshotFilename = fileName + "_" + date + fileExt;
		string path = _impl.PrepareScreenshotPath(albumName, screenshotFilename);

		Instance.StartCoroutine(Instance.Save(bytes, fileName, path, ImageType.SCREENSHOT));
	}

	//=============================================================================
	// Save texture
	//=============================================================================

	public static void SaveImage(Texture2D texture, string fileName, string fileType = "jpg")
	{
		Debug.Log("Save image to gallery " + fileName);

		Instance.Awake();

		byte[] bytes;
		string fileExt;
		
		if(fileType == "png")
		{
			bytes = texture.EncodeToPNG();
			fileExt = ".png";
		}
		else
		{
			bytes = texture.EncodeToJPG();
			fileExt = ".jpg";
		}

		string path = Application.persistentDataPath + "/" + fileName + fileExt;

		Instance.StartCoroutine(Instance.Save(bytes, fileName, path, ImageType.IMAGE));
	}
	
	
	IEnumerator Save(byte[] bytes, string fileName, string path, ImageType imageType)
	{
		int count = 0;
		SaveStatus saved = SaveStatus.NOTSAVED;
		File.WriteAllBytes(path, bytes);
		
		while(saved == SaveStatus.NOTSAVED)
		{
			count++;
			saved = count > 30 ? SaveStatus.TIMEOUT : _impl.SaveToGallery(path);
				
			yield return Instance.StartCoroutine(Instance.Wait(.5f));
		}
		_impl.SetNoBackupFlag(path);
		
		switch(saved)
		{
			case SaveStatus.DENIED:
				path = "DENIED";
				break;
				
			case SaveStatus.TIMEOUT:
				path = "TIMEOUT";
				break;
		}
		
		switch(imageType)
		{
			case ImageType.IMAGE:
				OnImageSaved?.Invoke(path);
				break;
				
			case ImageType.SCREENSHOT:
				OnScreenshotSaved?.Invoke(path);
				break;
		}
	}


	//=============================================================================
	// Image Picker
	//=============================================================================

	public static void PickImage()
	{
		Instance.Awake();
		Instance._impl.PickImage();
	}
	
	public void OnPickImage(string path)
	{
        Texture2D texture = LoadImageFromFile(path);
        OnImagePicked?.Invoke(texture, path);
	}


	//=============================================================================
	// Camera
	//=============================================================================
	
	public static void TakeCameraShot()
	{
		Instance.Awake();
		Instance._impl.TakeCameraShot();
	}

	public void OnCameraFinished(string path)
	{
        Texture2D texture = LoadImageFromFile(path);
        OnCameraShotComplete?.Invoke(texture, path);
	}


	//=============================================================================
	// Contacts
	//=============================================================================
	
	public static void PickContact()
	{
		Instance.Awake();
		Instance._impl.PickContact();
	}

	public void OnPickContactFinished(string data)
	{
		Dictionary<string, object> details = Json.Deserialize(data) as Dictionary<string, object>;
		if (details == null) return;
		
		string name = details.TryGetValue("name", out var v) ? v.ToString() : "";
		string number = details.TryGetValue("number", out v) ? v.ToString() : "";;
		string email = details.TryGetValue("email", out v) ? v.ToString() : "";;

		OnContactPicked?.Invoke(name, number, email);
	}
	

	//=============================================================================
	// Email with optional attachment
	//=============================================================================

	public static void SendEmail(string subject, string body, string pathToImageAttachment = "", string to = "", string cc = "", string bcc = "")
	{
		Instance.Awake();
		Instance._impl.SendEmail(subject, body, pathToImageAttachment, to, cc, bcc);
	}


	//=============================================================================
	// Confirm Dialog / Alert
	//=============================================================================
	
	public static void ShowConfirm(string title, string message, Action<bool> callback = null, string positiveBtnText = "Ok", string negativeBtnText = "Cancel")
	{
		Instance.Awake ();
		OnDialogComplete = callback;
		Instance._impl.ShowConfirm(title, message, positiveBtnText, negativeBtnText);
	}

	public static void ShowAlert(string title, string message, Action<bool> callback = null, string btnText = "Ok")
	{
		Instance.Awake ();
		OnDialogComplete = callback;
		Instance._impl.ShowAlert(title, message, btnText);
	}

	public void OnDialogPress(string result)
	{
		if(OnDialogComplete != null)
		{
			if(result == "Yes")
				OnDialogComplete(true);
			else if(result == "No")
				OnDialogComplete(false);
		}
	}


	//=============================================================================
	// Rate this app
	//=============================================================================
	
	public static void RateApp(string title = "Rate This App", string message = "Please take a moment to rate this App", 
	                           string positiveBtnText = "Rate Now", string neutralBtnText = "Later", string negativeBtnText = "No, Thanks",
	                           string appleId = "", Action<string> callback = null)
	{
		Instance.Awake();
		OnRateComplete = callback;
		Instance._impl.RateApp(title, message, positiveBtnText, neutralBtnText, negativeBtnText, appleId);
	}

	public void OnRatePress(string result)
	{
		OnRateComplete?.Invoke(result);
	}


	//=============================================================================
	// Location / Locale
	//=============================================================================

	public static bool StartLocation()
	{
        Instance.Awake();

        if(!Input.location.isEnabledByUser)
		{
			Debug.Log ("Location service disabled");
			return false;
		}

        Instance._impl.StartLocation();
        return true;
	}

	public static double GetLongitude()
	{
        Instance.Awake();
        return Input.location.isEnabledByUser ? Instance._impl.GetLongitude() : 0;
	}
	
	public static double GetLatitude()
	{
        Instance.Awake();
        return Input.location.isEnabledByUser ? Instance._impl.GetLatitude() : 0;
    }

	public static string GetCountryCode()
	{
		Instance.Awake ();
		return Instance._impl.GetCountryCode();
	}


	//=============================================================================
	// Local notifications
	//=============================================================================

	public static void ScheduleLocalNotification(string title, string message, int id = 0, int delayInMinutes = 0, string sound = "default_sound", 
	                                         bool vibrate = false, string smallIcon = "ic_notification", string largeIcon = "ic_notification_large")
	{
		Instance.Awake();
		Instance._impl.ScheduleLocalNotification(title, message, id, delayInMinutes, sound, vibrate, smallIcon,
			largeIcon);
	}

	public static void ClearLocalNotification(int id)
	{
		Instance.Awake();
		Instance._impl.ClearLocalNotification(id);
	}

	public static void ClearAllLocalNotifications()
	{
		Instance.Awake();
		Instance._impl.ClearAllLocalNotifications();
	}

	public static bool WasLaunchedFromNotification()
	{
		Instance.Awake();
		return Instance._impl.WasLaunchedFromNotification();
	}


	//=============================================================================
	// General functions
	//=============================================================================

	public static Texture2D LoadImageFromFile(string path)
	{
		if(path == "Cancelled") return null;

		byte[] bytes;
		Texture2D texture = new Texture2D(128, 128, TextureFormat.RGB24, false);

		#if UNITY_WINRT

		bytes = UnityEngine.Windows.File.ReadAllBytes(path);
		texture.LoadImage(bytes);

		#else

		bytes = File.ReadAllBytes(path);
		texture.LoadImage(bytes);

		#endif

		return texture;
	}
	
	
	IEnumerator Wait(float delay)
	{
		float pauseTarget = Time.realtimeSinceStartup + delay;
		
		while(Time.realtimeSinceStartup < pauseTarget)
		{
			yield return null;	
		}
	}
}