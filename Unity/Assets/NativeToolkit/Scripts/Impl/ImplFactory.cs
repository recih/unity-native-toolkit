using MiniJSON;
using UnityEngine;

namespace NativeToolkitImpl
{
    public static class ImplFactory
    {
        public static INativeToolkit CreateImpl()
        {
#if UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android)
                return new AndroidImpl();
#elif UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return new IOSImpl();
#endif
            return new DummyImpl();
        }
    }
}