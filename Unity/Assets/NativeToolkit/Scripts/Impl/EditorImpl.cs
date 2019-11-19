#if UNITY_EDITOR
using UnityEditor;

namespace NativeToolkitImpl
{
    public class EditorImpl : BaseImpl
    {
        public override void ShowConfirm(string title, string message, string positiveBtnText, string negativeBtnText)
        {
            var ret = EditorUtility.DisplayDialog(title, message, positiveBtnText, negativeBtnText);
            NativeToolkit.Instance.OnDialogPress(ret ? "Yes" : "No");
        }

        public override void ShowAlert(string title, string message, string btnText)
        {
            EditorUtility.DisplayDialog(title, message, btnText);
            NativeToolkit.Instance.OnDialogPress("Yes");
        }
    }
}
#endif