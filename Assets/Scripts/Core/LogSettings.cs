using UnityEngine;

public static class LogSettings
{
    // 첫 씬이 로드되기 전에 자동으로 한 번 실행된다 (오브젝트에 붙일 필요 없음)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // 에디터와 개발 빌드에서는 로그를 그대로 둔다
        if (Debug.isDebugBuild) return;

        // 릴리스에서는 경고와 에러만 남기고 일반 로그는 끈다
        Debug.unityLogger.filterLogType = LogType.Warning;
    }
}