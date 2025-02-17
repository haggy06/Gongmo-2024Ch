using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;

using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private Weapon[] weaponList = new Weapon[7];
    public Weapon[] WeaponList => weaponList;

    protected override void Awake()
    {
        base.Awake();

        highScore = PlayerPrefs.GetInt("HighScore");
    }

    #region _Scene Change Event_
    protected override void SceneChanged(Scene replacedScene, Scene newScene)
    {
        isBossExist = false;
        bossCount = 0;
        GameStatus = GameStatus.Play;

        CameraResolutionLock.SetResolution(4f, 3f);

        StageChangeEvent = () => { };
        LevelUPEvent = (_) => { };
        GameEndEvent = (_) => { };
        BossEvent = (_) => { }; // BossEvent 초기화

        if (newScene.buildIndex == (int)SCENE.Play)
        {
            MaxHP = 250;
            CurHP = MaxHP;

            DamageScope = 1f;

            Skill = 0f;

            EXP = 0;
            Level = 1;

            Score = 0;
            Stage = 1;

            CurWeaponType = WeaponType.Normal;

            UseCheat = false;
        }
        else if (newScene.buildIndex == (int)SCENE.Title)
        {
            AudioManager.Inst.ChangeBGM(ResourceLoader.AudioLoad(FolderName.BGM, "TItle"));
        }
    }
    #endregion
    private void FixedUpdate()
    {
        if (!CameraResolutionLock.CheckResolution())
        {
            CameraResolutionLock.SetResolution(4f, 3f);
        }
    }

    public static event Action StageChangeEvent = () => { };
    /// <summary> 보스 등장의 경우 true, 퇴장일 경우 false </summary>
    public static event Action<bool> BossEvent = (_) => { };

    private static int bossCount = 0;
    private const int bossRequireKill = 5;

    private bool isBossExist = false;
    public void BossAppear()
    {
        isBossExist = true;
        bossCount++;
        AudioManager.Inst.ChangeBGM(ResourceLoader.AudioLoad(FolderName.BGM, "Boss"));

        BossEvent.Invoke(true);
    }
    public void BossDisappear()
    {
        BossEvent.Invoke(false);

        isBossExist = false;
        PopupManager.Inst.BossDisappear();

        if (bossCount >= bossRequireKill)
        {
            GameStatus = GameStatus.GameClear;
        }
        else
        {
            AudioManager.Inst.ChangeBGM(ResourceLoader.AudioLoad(FolderName.BGM, "Stage" + Inst.stage));
        }
    }


    [SerializeField]
    private int highScore = 0;
    public static int HighScore
    {
        get => Inst.highScore;
        set
        {
            Inst.highScore = value;
            PlayerPrefs.SetInt("HighScore", Inst.highScore);
        }
    }
    #region _Play Status_
    [Header("Play Status")]

    [SerializeField]
    private bool useCheat = false;
    public static bool UseCheat
    {
        get => Inst.useCheat;
        set
        {
            Inst.useCheat = value;


        }
    }

    private static Weapon curWeapon;
    public static Weapon CurWeapon => curWeapon;

    [SerializeField]
    private int curHP = 200;
    public static int CurHP
    {
        get => Inst.curHP;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // 죽거나 클리어했을 경우
            {
                Debug.Log("게임이 끝나 curHP가 변경되지 않음");
                return;
            }

            Inst.curHP = Mathf.Clamp(value, 0, Inst.maxHP); // hp가 음수가 되거나 최대 체력을 넘지 않게 제한
            PopupManager.Inst.ChangeHP();
        }
    }

    [SerializeField]
    private int maxHP = 250;
    public static int MaxHP
    {
        get => Inst.maxHP;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // 죽거나 클리어했을 경우
            {
                Debug.Log("게임이 끝나 maxHP가 변경되지 않음");
                return;
            }

            Inst.maxHP = value;
            CurHP = Inst.curHP; // CurHP 프로퍼티 실행(체력 Clamp 및 UI 새로고침 용도)
        }
    }

    [SerializeField]
    private float damageScope = 1f;
    public static float DamageScope
    {
        get => Inst.damageScope;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // 죽거나 클리어했을 경우
            {
                Debug.Log("게임이 끝나 damageScope가 변경되지 않음");
                return;
            }

            Inst.damageScope = Mathf.Clamp(value, 1f, float.PositiveInfinity);
        }
    }

    [SerializeField]
    private float skill = 0f;
    public static float Skill
    {
        get => Inst.skill;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // 죽거나 클리어했을 경우
            {
                Debug.Log("게임이 끝나 skill이 변경되지 않음");
                return;
            }

            Inst.skill = Mathf.Clamp(value, 0f, 100f);

            PopupManager.Inst.ChangeSkill();
        }
    }

    public static readonly int[] levelUpTable = { 0, 1000, 1600, 2500, 4000 };

    [SerializeField]
    private int exp = 0;
    public static int EXP
    {
        get => Inst.exp;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // 죽거나 클리어했을 경우
            {
                Debug.Log("게임이 끝나 exp가 변경되지 않음");
                return;
            }

            Inst.exp = value;
            if (Inst.level < levelUpTable.Length) // 최대 레벨을 찍지 못했을 경우
            {
                int levelUP = 0;
                while (Inst.exp - levelUpTable[Inst.level + levelUP] >= 0) // 레벨업이 가능한 동안 반복
                {
                    Inst.exp -= levelUpTable[Inst.level + levelUP]; // 레벨업에 쓰인 경험치 빼줌
                    levelUP++; // 오른 레벨 + 1

                    if (Inst.level + levelUP >= levelUpTable.Length) // 최대 레벨을 찍거나 넘었을 경우
                    {
                        Inst.exp = 0; // EXP를 0으로 맞춘 뒤
                        break; // While 문 탈출 (냅두면 인덱스 오류 남)
                    }
                }

                if (levelUP != 0) // 레벨 변화가 있었을 경우
                {
                    Level += levelUP; // 한번에 레벨을 대입시켜줌
                }
                PopupManager.Inst.ChangeEXP();
            }
        }
    }

    /// <summary> 레벨 업일 경우 true, 다운일 경우 false </summary>
    public static event Action<bool> LevelUPEvent = (_) => { };
    [SerializeField]
    private int level = 1;
    public static int Level
    {
        get => Inst.level;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // 죽거나 클리어했을 경우
            {
                Debug.Log("게임이 끝나 level이 변경되지 않음");
                return;
            }

            int levelDiff = value - Inst.level; // 레벨의 변화량을 나타냄

            if (levelDiff > 0) // 레벨 업
            {
                if (Inst.level >= levelUpTable.Length) // 이미 최대 레벨일 경우
                {
                    Debug.Log("최고 레벨입니다.");
                    return;
                }
                Inst.level = value;

                MaxHP += 25;
                DamageScope += 0.1f;

                CurHP = MaxHP;
                LevelUPEvent.Invoke(true);
            }
            else if (levelDiff < 0) // 레벨 다운
            {
                if (Inst.level <= 1) // 최저 레벨일 경우
                {
                    Debug.Log("최저 레벨입니다.");
                    return;
                }
                Inst.level = value;

                MaxHP -= 25;
                DamageScope -= 0.1f;

                LevelUPEvent.Invoke(false);
            }

            PopupManager.Inst.ChangeLevel();
        }
    }

    public static readonly int[] stageUpTable = { 0, 6500, 13500 };

    [SerializeField]
    private int score = 0;
    public static int Score
    {

        get => Inst.score;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // 죽거나 클리어했을 경우
            {
                Debug.Log("게임이 끝나 score가 변경되지 않음");
                return;
            }

            Inst.score = value;

            PopupManager.Inst.ChangeScore();

            if (Inst.stage < stageUpTable.Length) // 최종 스테이지가 아닐 경우
            {
                if (stageUpTable[Inst.stage] <= Inst.score) // 다음 스테이지로 갈 수 있을 만큼 스코어가 쌓였을 경우
                {
                    Stage++;
                }
            }
        }
    }

    [SerializeField]
    private int stage = 1;
    public static int Stage
    {
        get => Inst.stage;
        set
        {
            Inst.stage = value;

            if (!Inst.isBossExist)
                AudioManager.Inst.ChangeBGM(ResourceLoader.AudioLoad(FolderName.BGM, "Stage" + Inst.stage));
            PopupManager.Inst.ChangeStage();

            StageChangeEvent.Invoke();
        }
    }

    [SerializeField]
    private WeaponType curWeaponType = WeaponType.Normal;
    public static WeaponType CurWeaponType
    {
        get => Inst.curWeaponType;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // 죽거나 클리어했을 경우
            {
                Debug.Log("게임이 끝나 curWeaponType이 변경되지 않음");
                return;
            }

            Inst.curWeaponType = value;
            curWeapon = Inst.weaponList[(int)value];
            PopupManager.Inst.ChangeWeapon();
        }
    }
    #endregion
    [SerializeField]
    private GameStatus gameStatus = GameStatus.Play;
    public static GameStatus GameStatus
    {
        get => Inst.gameStatus;
        set
        {
            Inst.gameStatus = value;
            if (true) // 새로운 게임 상태가 들어왔을 경우
            {
                switch (Inst.gameStatus)
                {
                    case GameStatus.Play:

                        break;

                    case GameStatus.GameOver:
                        AudioManager.Inst.ChangeBGM(ResourceLoader.AudioLoad(FolderName.BGM, "GameOver"));
                        GameEnd();
                        break;

                    case GameStatus.GameClear:
                        AudioManager.Inst.ChangeBGM(ResourceLoader.AudioLoad(FolderName.BGM, "GameClear"));
                        GameEnd();
                        break;
                }

            }
        }
    }

    public static event Action<GameStatus> GameEndEvent = (_) => { };
    private static void GameEnd()
    {
        if (!UseCheat) // 치트를 안 썼을 경우
        {
            if (Inst.gameStatus == GameStatus.GameClear) // 클리어했을 경우
                PlayerPrefs.SetInt("Clear", Mathf.Clamp(Inst.stage, PlayerPrefs.GetInt("Clear", 0), 3));

            if (Inst.highScore < Inst.score) // 기록을 경신했을 경우
            {
                HighScore = Inst.score;
            }
        }        

        GameEndEvent.Invoke(GameStatus);
        PopupManager.Inst.GameEnd();
    }
}
[Serializable]
public struct Weapon
{
    public string name;

    public PoolObject bullet;
    public PoolObject skill;

    public float coolDown;
    public int skillGauge;
}

public enum GameStatus
{
    Play,
    GameOver,
    GameClear,

}

public enum WeaponType
{
    Normal,

    Green1,
    Green2,
    Green3,

    Red1,
    Red2,
    Red3
}