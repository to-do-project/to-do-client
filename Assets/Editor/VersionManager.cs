using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class Version_Manager : MonoBehaviour
{
    private static bool AutoIncrease = false;
    private const string AutoIncreaseMenuName = "Build/Auto Increase Build Version";

    [PostProcessBuild(1)] // PostProcessBuild - ���� �� ����Ǵ� �ݹ� �Լ�
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (AutoIncrease) EditVersion(0, 0, 1);
    }

    /// <summary>
    /// �ڵ����� ������
    /// </summary>
    [MenuItem(AutoIncreaseMenuName, false, 1)]
    private static void SetAutoIncrease()
    {
        AutoIncrease = !AutoIncrease;
        EditorPrefs.SetBool(AutoIncreaseMenuName, AutoIncrease);
        Debug.Log("Auto Increase : " + AutoIncrease);
    }

    [MenuItem(AutoIncreaseMenuName, true)]
    private static bool SetAutoIncreaseValidate()
    {
        Menu.SetChecked(AutoIncreaseMenuName, AutoIncrease);
        return true;
    }

    /// <summary>
    /// ���� ����(������)
    /// </summary>
    static void EditVersion(int majorIncr, int minorIncr, int buildIncr)
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');

        int MajorVersion = int.Parse(lines[0]) + majorIncr;
        int MinorVersion = int.Parse(lines[1]) + minorIncr;
        int Build = int.Parse(lines[2]) + buildIncr;

        PlayerSettings.bundleVersion = MajorVersion.ToString("0") + "." +
                                       MinorVersion.ToString("0") + "." +
                                       Build.ToString("0");
        //PlayerSettings.Android.bundleVersionCode =
        //     MajorVersion * 10000 + MinorVersion * 1000 + Build;
        PlayerSettings.Android.bundleVersionCode = PlayerSettings.Android.bundleVersionCode + 1;
        CheckCurrentVersion();
    }

    //���� ���� üũ
    [MenuItem("Build/Check Current Version", false, 2)]
    private static void CheckCurrentVersion()
    {
        Debug.Log("Build v" + PlayerSettings.bundleVersion +
             " (" + PlayerSettings.Android.bundleVersionCode + ")"); //���� ���� ǥ��
    }

    //Major ���� ��
    [MenuItem("Build/Increase Major Version", false, 51)]
    private static void IncreaseMajor()
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');
        EditVersion(1, -int.Parse(lines[1]), -int.Parse(lines[2]));
    }

    //Minor ���� ��
    [MenuItem("Build/Increase Minor Version", false, 52)]
    private static void IncreaseMinor()
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');
        EditVersion(0, 1, -int.Parse(lines[2]));
    }
}