using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;

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

        SceneManager.activeSceneChanged += SceneChanged;
    }

    #region _Scene Change Event_
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneChanged; // ������Ʈ �ı��� �� �̺�Ʈ ���� ����
    }
    private void SceneChanged(Scene replacedScene, Scene newScene)
    {
        CameraResolutionLock.SetResolution(4f, 3f);

        if (newScene.buildIndex == (int)SCENE.Play) // �÷��� ȭ���� ���
        {
            MaxHP = 200;
            CurHP = MaxHP;

            DamageScope = 1f;

            Skill = 0f;

            EXP = 0;
            Level = 1;

            Score = 0;
            Stage = 1;

            CurWeaponType = WeaponType.Normal;
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

    public void MoveScene(SCENE targetScene)
    {

    }

    #region _Play Stat_
    private static PoolObject curBullet;
    public static PoolObject CurBullet => curBullet;

    private static int curHP = 200;
    public static int CurHP
    {
        get => curHP;
        set
        {
            curHP = Mathf.Clamp(value, 0, maxHP); // hp�� ������ �ǰų� �ִ� ü���� ���� �ʰ� ����
            PopupManager.Inst.ChangeHP();
        }
    }

    private static int maxHP = 200;
    public static int MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
            CurHP = curHP; // CurHP ������Ƽ ����(ü�� Clamp �� UI ���ΰ�ħ �뵵)
        }
    }

    private static float damageScope = 1f;
    public static float DamageScope
    {
        get => damageScope;
        set
        {
            damageScope = Mathf.Clamp(value, 1f, float.PositiveInfinity);
        }
    }

    private static float skill = 0f;
    public static float Skill
    {
        get => skill;
        set
        {
            skill = Mathf.Clamp(value, 0f, 100f);

            PopupManager.Inst.ChangeSkill();
        }
    }

    public static readonly int[] levelUpTable = { 0, 50, 100, 200, 400, 800, 1600 };
    private static int exp = 0;
    public static int EXP
    {
        get => exp;
        set
        {
            exp = value;
            if (level < levelUpTable.Length) // �ִ� ������ ���� ������ ���
            {
                int levelUP = 0;
                while (exp - levelUpTable[level + levelUP] >= 0) // �������� ������ ���� �ݺ�
                {
                    levelUP++; // ���� ���� + 1
                    exp -= levelUpTable[level + levelUP]; // �������� ���� ����ġ ����

                    if (level + levelUP >= levelUpTable.Length) // �ִ� ������ ��ų� �Ѿ��� ���
                    {
                        Level += levelUP; // �������� �ϰ�
                        exp = 0; // EXP�� 0���� ���� ��
                        break; // While �� Ż�� (���θ� �ε��� ���� ��)
                    }
                }

                Level += levelUP; // �ѹ��� ������ ���Խ�����
                PopupManager.Inst.ChangeEXP();
            }
        }
    }

    private static int level = 1;
    public static int Level
    {
        get => level;
        set
        {
            int levelDiff = value - level; // ������ ��ȭ���� ��Ÿ��

            if (levelDiff > 0) // ���� ��
            {
                float rate = 1.1f * levelDiff;
                MaxHP = (int)(maxHP * rate); // ü�� 10���� UP
                DamageScope = (int)(damageScope * rate); // ���ݷ� 10���� UP
                CurHP = MaxHP;
            }
            else if (levelDiff < 0) // ���� �ٿ�
            {
                float rate = (10f / 11f) * levelDiff;
                MaxHP = Mathf.Clamp((int)(maxHP * rate), 200, int.MaxValue); // ü�� �ѹ�(���� ü�� ������ �������� ����)
                DamageScope = Mathf.Clamp(damageScope * rate, 1f, float.PositiveInfinity); // ���ݷ� �ѹ�(1�� ������ �������� ����)
            }

            level = value;

            PopupManager.Inst.ChangeLevel();
        }
    }

    public static readonly int[] stageUpTable = { 0, 2000, 5000 };
    private static int score = 0;
    public static int Score
    {
        get => score;
        set
        {
            score = value;

            PopupManager.Inst.ChangeScore();

            if (stage < stageUpTable.Length) // ���� ���������� �ƴ� ���
            {
                if (stageUpTable[stage] <= stage) // ���� ���������� �� �� ���� ��ŭ ���ھ �׿��� ���
                {
                    Stage++;
                }
            }
        }
    }

    private static int stage = 1;
    public static int Stage
    {
        get => stage;
        set
        {
            stage = value;
            PopupManager.Inst.ChangeStage();
        }
    }

    private static WeaponType curWeaponType = WeaponType.Normal;
    public static WeaponType CurWeaponType
    {
        get => curWeaponType;
        set
        {
            curWeaponType = value;
            curBullet = Inst.weaponList[(int)value].bullet;
            PopupManager.Inst.ChangeWeapon();
        }
    }
    #endregion
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