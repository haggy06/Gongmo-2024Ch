using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    private PopupBase fadePopup;

    #region _Title Interfaces_
    [Header("Title Interfaces")]

    [SerializeField]
    private PopupBase titlePopup;

    [Space(5)]
    [SerializeField]
    private Button PlayButton;
    [SerializeField]
    private Button infoButton;
    [SerializeField]
    private Button gameQuitButton;

    [Space(5)]
    [SerializeField]
    private PopupBase infoPopup;
    [SerializeField]
    private Button infoCloseButton;
    #endregion

    #region _Player Interfaces_
    [Header("Player Interfaces")]

    [SerializeField]
    private PopupBase playerPopup;

    [Space(5)]
    [SerializeField]
    private TextMeshProUGUI stageText;

    [Space(5)]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [Space(5)]
    [SerializeField]
    private AudioClip moribundHPSound;

    [Space(5)]
    [SerializeField]
    private Image hpFill;
    [SerializeField]
    private ImageBlink hpIcon;
    [SerializeField]
    private TextMeshProUGUI hpText; // "현재 체력 / 최대 체력"

    [Space(5)]
    [SerializeField]
    private AudioClip skillChargedSound;

    [Space(5)]
    [SerializeField]
    private Image skillFill;
    [SerializeField]
    private ImageBlink skillIcon;
    [SerializeField]
    private TextMeshProUGUI skillText; // "현재 충전율(%)"

    [Space(5)]
    [SerializeField]
    private PopupBase levelTablePopup;
    [SerializeField]
    private TextMeshProUGUI tableText;
    [SerializeField]
    private TextMeshProUGUI expText;

    [Space(5)]
    [SerializeField]
    private Image levelFill;
    [SerializeField]
    private ImageBlink levelIcon;
    [SerializeField]
    private TextMeshProUGUI levelText; // "Lv.현재 레벨"

    [Space(5)]
    [SerializeField]
    private Image weaponImage;
    [SerializeField]
    private TextMeshProUGUI weaponText; // "무기 이름 + 강화율"  ex) 유도탄 ++
    #endregion

    #region _Boss Interfaces_
    [Header("Boss Interfaces")]
    [SerializeField]
    private AudioClip warningSound;

    [SerializeField]
    private PopupBase bossWarningPopup;
    [SerializeField]
    private Image warningLine;

    [Space(5)]
    [SerializeField]
    private PopupBase bossPopup;
    [SerializeField]
    private Image bossHPFill;
    [SerializeField]
    private TextMeshProUGUI bossText;
    #endregion

    #region _Game End Popup_
    [Header("Game End Popup")]
    [SerializeField]
    private AudioClip gameOverSound;
    [SerializeField]
    private AudioClip gameClearSound;

    [Space(5)]
    [SerializeField]
    private PopupBase gameEndPopup;

    [Space(5)]
    [SerializeField]
    private TextMeshProUGUI gameEndText;

    [Space(5)]
    [SerializeField]
    private TextMeshProUGUI finalScoreText;
    [SerializeField]
    private Image highScoreBG;
    [SerializeField]
    private TextMeshProUGUI highScoreText;

    [Space(5)]
    [SerializeField]
    private GameObject cheatCheck;

    [Space(5)]
    [SerializeField]
    private Button retryButton;
    [SerializeField]
    private Button menuButton;
    #endregion

    [Header("Ect")]
    [SerializeField]
    private float gameEndPopupTerm = 1f;

    private static PopupManager instance;
    public static PopupManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameObject>(Path.Combine("Singleton", "PopupManager"))).GetComponent<PopupManager>(); // 싱글톤 리소스 폴더에서 팝업매니저를 꺼내 생성 후 저장
            }
            return instance;
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("중복된 싱글톤 파괴");

            Destroy(gameObject);
        }

        SceneManager.activeSceneChanged += SceneChanged;

        tableText.text = "";
        for (int i = 0; i < GameManager.levelUpTable.Length; i++)
        {
            tableText.text += "레벨 " + (i + 1) + " : " + GameManager.levelUpTable[i] + "\n";
        }
        #region _Set Button Event_
        PlayButton.onClick.AddListener(() => SceneMove(SCENE.Play));
        infoButton.onClick.AddListener(() => infoPopup.PopupOpen());
        gameQuitButton.onClick.AddListener(() => Application.Quit());

        infoCloseButton.onClick.AddListener(() => infoPopup.PopupClose());

        retryButton.onClick.AddListener(() => SceneMove(SCENE.Play));
        menuButton.onClick.AddListener(() => SceneMove(SCENE.Title));
        #endregion

        // SceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene()); // 중간에 시작해 제대로 초기화가 안 되는 걸 막기 위함
    }

    #region _Scene Move_
    public void SceneMove(SCENE targetScene)
    {
        StartCoroutine(SceneMoveCor(targetScene));
    }
    private IEnumerator SceneMoveCor(SCENE targetScene)
    {
        fadePopup.PopupOpen(); // 페이드 아웃

        yield return YieldReturn.WaitForSeconds((1f - fadePopup.Popup.alpha) * fadePopup.FadeDuration); // 페이드가 완료될 때까지 기다림

        SceneManager.LoadScene((int)targetScene); // 씬 이동
    }
    #endregion

    #region _Scene Change Event_
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneChanged; // 오브젝트 파괴될 때 이벤트 구독 해지
    }
    private void SceneChanged(Scene replacedScene, Scene newScene)
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        infoPopup.PopupHide();
        titlePopup.PopupHide();

        levelTablePopup.PopupHide();

        playerPopup.PopupHide();

        bossPopup.PopupHide();
        bossWarningPopup.PopupHide();
        gameEndPopup.PopupHide();

        if (newScene.buildIndex == (int)SCENE.Title) // 타이틀 씬의 경우
        {
            titlePopup.PopupOpen();
        }
        else if (newScene.buildIndex == (int)SCENE.Play) // 플레이 씬의 경우
        {
            playerPopup.PopupShow();
        }

        fadePopup.PopupClose(true); // 페이드 인
    }
    #endregion

    #region _Play UI Change_
    public void ChangeScore()
    {
        scoreText.text = GameManager.Score.ToString();
    }
    public void ChangeStage()
    {
        stageText.text = "스테이지 " + GameManager.Stage;
    }

    private bool moribundHP = false;
    public void ChangeHP()
    {
        hpFill.fillAmount = (float)GameManager.CurHP / GameManager.MaxHP;
        if (hpFill.fillAmount <= 0.25f) // 피가 1/4 이하가 되었을 경우
        {
            if (!moribundHP)
            {
                moribundHP = true;
                AudioManager.Inst.PlaySFX(moribundHPSound);

                hpIcon.BlinkStart();
            }
        }
        else
        {
            moribundHP = false;

            hpIcon.BlinkStop();
        }

        hpText.text = GameManager.CurHP + " / " + GameManager.MaxHP;
    }

    private bool skillBlink = false;
    public void ChangeSkill()
    {
        skillFill.fillAmount = GameManager.Skill / 100f;
        if (Mathf.Approximately(skillFill.fillAmount, 1f)) // 충전율이 100프로가 되었을 경우
        {
            if (!skillBlink)
            {
                skillBlink = true;
                AudioManager.Inst.PlaySFX(skillChargedSound);

                skillIcon.BlinkStart();
            }
        }
        else
        {
            skillBlink = false;

            skillIcon.BlinkStop();
        }

        skillText.text = GameManager.Skill.ToString("F1") + "%";
    }
    public void ChangeLevel()
    {
        if (GameManager.Level >= GameManager.levelUpTable.Length) // 최대 레벨을 찍거나 넘었을 경우
        {
            levelText.text = "Lv.MAX";
            levelFill.fillAmount = 1f;

            levelIcon.Img.color = Color.yellow;
        }
        else
        {
            levelText.text = "Lv." + GameManager.Level.ToString();

            levelIcon.Img.color = Color.white;
        }
    }
    public void ChangeEXP()
    {
        if (GameManager.Level >= GameManager.levelUpTable.Length) // 최대 레벨을 찍거나 넘었을 경우
        {
            expText.text = "MAX";
            levelFill.fillAmount = 1f;
        }
        else
        {
            expText.text = GameManager.EXP.ToString();
            levelFill.fillAmount = (float)GameManager.EXP / GameManager.levelUpTable[GameManager.Level];
        }
    }
    public void ChangeWeapon()
    {
        Weapon info = GameManager.Inst.WeaponList[(int)GameManager.CurWeaponType];

        weaponImage.sprite = info.icon;
        weaponText.text = info.name;
    }


    public void BossAppear(string bossName = "보스")
    {
        print("boss");
        ChangeBossHP(1, 1);
        bossText.text = bossName;
        StartCoroutine("BossCor");
    }
    public void BossDisappear()
    {
        print("ssoq");
        bossPopup.PopupClose();
    }
    public const float BossWarningTime = 3f;
    private IEnumerator BossCor()
    {
        AudioManager.Inst.PlaySFX(warningSound);

        bossWarningPopup.PopupOpen();

        yield return YieldReturn.WaitForSeconds(BossWarningTime);

        bossWarningPopup.PopupClose();
        bossPopup.PopupOpen();
    }

    public void ChangeBossHP(float curHP, float maxHP)
    {
        bossHPFill.fillAmount = curHP / maxHP;
    }

    public void GameEnd()
    {
        Invoke("OpenGameEndPopup", gameEndPopupTerm);
    }
    private void OpenGameEndPopup()
    {
        gameEndPopup.PopupOpen();

        if (GameManager.GameStatus == GameStatus.GameClear)
        {
            AudioManager.Inst.PlaySFX(gameClearSound);

            gameEndText.text = "게임 클리어!!!";
        }
        else
        {
            AudioManager.Inst.PlaySFX(gameOverSound);

            gameEndText.text = "게임 오버...";
        }

        highScoreBG.color = GameManager.UseCheat ? Color.grey : Color.white;

        finalScoreText.text = "최종 스코어 : " + GameManager.Score.ToString();
        highScoreText.text = "최고 스코어 : " + GameManager.HighScore.ToString();

        cheatCheck.SetActive(GameManager.UseCheat);
    }

    public void Open_CloseTable()
    {
        if (levelTablePopup.Popup.blocksRaycasts) // blocksRaycasts가 true면 팝업이 열려 있다는 뜻이다.
        {
            levelTablePopup.PopupClose();
        }
        else
        {
            levelTablePopup.PopupOpen();
        }
    }
    #endregion
}

public enum SCENE
{
    Title,
    Play,

}