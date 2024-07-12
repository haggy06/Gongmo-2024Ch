using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;

using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameObject>(Path.Combine("Singleton", "GameManager"))).GetComponent<GameManager>(); // �̱��� ���ҽ� �������� ���ӸŴ����� ���� ���� �� ����
            }
            return instance;
        }
    }

    [SerializeField]
    private Sprite[] itemIconList = new Sprite[3];
    public Sprite[] ItemIconList => itemIconList;

    [SerializeField]
    private Weapon[] weaponList = new Weapon[7];
    public Weapon[] WeaponList  => weaponList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        highScore = PlayerPrefs.GetInt("HighScore");
        SceneManager.activeSceneChanged += SceneChanged;
    }

    #region _Scene Change Event_
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneChanged; // ������Ʈ �ı��� �� �̺�Ʈ ���� ����
    }
    private void SceneChanged(Scene replacedScene, Scene newScene)
    {
        bossCount = 0;
        GameStatus = GameStatus.Play;

        CameraResolutionLock.SetResolution(4f, 3f);

        GameEndEvent = (_) => { };
        BossEvent = (_) => { }; // BossEvent �ʱ�ȭ
        #region _Reset PlayData_
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
        #endregion
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
    /// <summary> ���� ������ ��� true, ������ ��� false </summary>
    public static event Action<bool> BossEvent = (_) => { };

    private static int bossCount = 0;
    private const int bossRequireKill = 3;
    public void BossAppear()
    {
        bossCount++;
        BossEvent.Invoke(true);
    }
    public void BossDisappear()
    {
        PopupManager.Inst.BossDisappear();

        if (bossCount >= bossRequireKill)
        {
            GameStatus = GameStatus.GameClear;
        }
        else
        {
            BossEvent.Invoke(false);
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
            if (Inst.gameStatus != GameStatus.Play) // �װų� Ŭ�������� ���
            {
                Debug.Log("������ ���� curHP�� ������� ����");
                return;
            }

            Inst.curHP = Mathf.Clamp(value, 0, Inst.maxHP); // hp�� ������ �ǰų� �ִ� ü���� ���� �ʰ� ����
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
            if (Inst.gameStatus != GameStatus.Play) // �װų� Ŭ�������� ���
            {
                Debug.Log("������ ���� maxHP�� ������� ����");
                return;
            }

            Inst.maxHP = value;
            CurHP = Inst.curHP; // CurHP ������Ƽ ����(ü�� Clamp �� UI ���ΰ�ħ �뵵)
        }
    }

    [SerializeField]
    private float damageScope = 1f;
    public static float DamageScope
    {
        get => Inst.damageScope;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // �װų� Ŭ�������� ���
            {
                Debug.Log("������ ���� damageScope�� ������� ����");
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
            if (Inst.gameStatus != GameStatus.Play) // �װų� Ŭ�������� ���
            {
                Debug.Log("������ ���� skill�� ������� ����");
                return;
            }

            Inst.skill = Mathf.Clamp(value, 0f, 100f);

            PopupManager.Inst.ChangeSkill();
        }
    }

    public static readonly int[] levelUpTable = { 0, 350, 700, 1200, 1500 };

    [SerializeField]
    private int exp = 0;
    public static int EXP
    {
        get => Inst.exp;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // �װų� Ŭ�������� ���
            {
                Debug.Log("������ ���� exp�� ������� ����");
                return;
            }

            Inst.exp = value;
            if (Inst.level < levelUpTable.Length) // �ִ� ������ ���� ������ ���
            {
                int levelUP = 0;
                while (Inst.exp - levelUpTable[Inst.level + levelUP] >= 0) // �������� ������ ���� �ݺ�
                {
                    Inst.exp -= levelUpTable[Inst.level + levelUP]; // �������� ���� ����ġ ����
                    levelUP++; // ���� ���� + 1

                    if (Inst.level + levelUP >= levelUpTable.Length) // �ִ� ������ ��ų� �Ѿ��� ���
                    {
                        Inst.exp = 0; // EXP�� 0���� ���� ��
                        break; // While �� Ż�� (���θ� �ε��� ���� ��)
                    }
                }

                if (levelUP != 0) // ���� ��ȭ�� �־��� ���
                {
                    Level += levelUP; // �ѹ��� ������ ���Խ�����
                }
                PopupManager.Inst.ChangeEXP();
            }
        }
    }

    public static event Action LevelUPEvent = () => { };
    [SerializeField]
    private int level = 1;
    public static int Level
    {
        get => Inst.level;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // �װų� Ŭ�������� ���
            {
                Debug.Log("������ ���� level�� ������� ����");
                return;
            }

            int levelDiff = value - Inst.level; // ������ ��ȭ���� ��Ÿ��

            if (levelDiff > 0) // ���� ��
            {
                if (Inst.level >= levelUpTable.Length) // �̹� �ִ� ������ ���
                {
                    Debug.Log("�ְ� �����Դϴ�.");
                    return;
                }

                MaxHP += 25;
                DamageScope += 0.1f;
                /*
                float rate = 1.1f * levelDiff;
                MaxHP = (int)Mathf.Round(Inst.maxHP * rate); // ü�� 10���� UP
                DamageScope = Inst.damageScope * rate; // ���ݷ� 10���� UP
                */
                CurHP = MaxHP;
                LevelUPEvent.Invoke();
            }
            else if (levelDiff < 0) // ���� �ٿ�
            {
                if (Inst.level <= 1) // ���� ������ ���
                {
                    Debug.Log("���� �����Դϴ�.");
                    return;
                }

                /*
                float rate = (10f / 11f) * Mathf.Abs(levelDiff);
                MaxHP = Mathf.Clamp((int)Mathf.Round(Inst.maxHP * rate), 200, int.MaxValue); // ü�� �ѹ�(���� ü�� ������ �������� ����)
                DamageScope = Mathf.Clamp(Inst.damageScope * rate, 1f, float.PositiveInfinity); // ���ݷ� �ѹ�(1�� ������ �������� ����)
                */
                MaxHP -= 25;
                DamageScope -= 0.1f;
            }

            Inst.level = value;

            PopupManager.Inst.ChangeLevel();
        }
    }

    public static readonly int[] stageUpTable = { 0, 2500, 6000 };

    [SerializeField]
    private int score = 0;
    public static int Score
    {

        get => Inst.score;
        set
        {
            if (Inst.gameStatus != GameStatus.Play) // �װų� Ŭ�������� ���
            {
                Debug.Log("������ ���� score�� ������� ����");
                return;
            }

            Inst.score = value;

            PopupManager.Inst.ChangeScore();

            if (Inst.stage < stageUpTable.Length) // ���� ���������� �ƴ� ���
            {
                if (stageUpTable[Inst.stage] <= Inst.score) // ���� ���������� �� �� ���� ��ŭ ���ھ �׿��� ���
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
            if (Inst.gameStatus != GameStatus.Play) // �װų� Ŭ�������� ���
            {
                Debug.Log("������ ���� curWeaponType�� ������� ����");
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
            if (true) // ���ο� ���� ���°� ������ ���
            {
                switch (Inst.gameStatus)
                {
                    case GameStatus.Play:

                        break;

                    case GameStatus.GameOver:
                        GameEnd();
                        break;

                    case GameStatus.GameClear:
                        GameEnd();
                        break;
                }

            }
        }
    }

    public static event Action<GameStatus> GameEndEvent = (_) => { };
    private static void GameEnd()
    {
        if (!UseCheat && Inst.highScore < Inst.score) // ġƮ�� �� ���� ����� ������� ���
        {
            HighScore = Inst.score;
        }

        GameEndEvent.Invoke(GameStatus);
        PopupManager.Inst.GameEnd();
    }
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
    Red3,

}