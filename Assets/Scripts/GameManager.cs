using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject popupPanel;
    bool gameHasEnded = false;

    public float restartDelay = 1f;

    private Text resultText;

    void Start() {
        popupPanel.SetActive(false);
        resultText = popupPanel.GetComponent<Text>();
    }

    public void EndGame(){
        if (!gameHasEnded){
            gameHasEnded = true;
            Debug.Log("Game Over");

            ShowPopupMessage("실패...");

            // Restart the game
            Invoke("Restart", restartDelay);
        }
    }

    public void ClearGame(){
        if (!gameHasEnded){
            gameHasEnded = true;
            Debug.Log("Game Clear");

            // 게임 클리어 축하한다는 이펙트
            ShowPopupMessage("성공!");

            // Restart the game
            Invoke("Restart", restartDelay);
        }
    }

    void Restart(){
        // SceneManager.LoadScene("Level01");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update(){
        // 1스테이지 클리어 조건: 스포너와 보스 둘 다 없을 때
        if (FindObjectOfType<EnemyBossAttack>() == null && FindObjectOfType<EnemyBossSpawner>() == null)
            ClearGame();
    }

    void ShowPopupMessage(string message){
        popupPanel.SetActive(true);
        resultText.text = message;
    }
}
