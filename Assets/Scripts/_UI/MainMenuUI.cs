using UnityEngine;
using UnityEngine.SceneManagement;   // 씬 전환
#if UNITY_EDITOR
using UnityEditor;                  // 에디터 종료용(빌드엔 포함 X)
#endif

public class MainMenuUI : MonoBehaviour
{
    // “Play” 버튼에서 호출
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");   // 본편 씬 이름으로 교체
    }

    // “Quit” 버튼에서 호출
    public void QuitGame()
    {
        Application.Quit();                    // 빌드 실행 시 종료 :contentReference[oaicite:0]{index=0}
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;   // 에디터 상태일 때는 Play 모드만 해제
#endif
    }
}
