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
                instance = Instantiate(Resources.Load<GameObject>(Path.Combine("Singleton", "GameManager"))).GetComponent<GameManager>(); // 싱글톤 리소스 폴더에서 게임매니저를 꺼내 생성 후 저장
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
        SceneManager.activeSceneChanged -= SceneChanged; // 오브젝트 파괴될 때 이벤트 구독 해지
    }
    private void SceneChanged(Scene replacedScene, Scene newScene)
    {
        CameraResolutionLock.SetResolution(4f, 3f);

        if (newScene.buildIndex == (int)SCENE.Play) // 플레이 화면일 경우
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
            curHP = Mathf.Clamp(value, 0, maxHP); // hp가 음수가 되거나 최대 체력을 넘지 않게 제한
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
            CurHP = curHP; // CurHP 프로퍼티 실행(체력 Clamp 및 UI 새로고침 용도)
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
            if (level < levelUpTable.Length) // 최대 레벨을 찍지 못했을 경우
            {
                int levelUP = 0;
                while (exp - levelUpTable[level + levelUP] >= 0) // 레벨업이 가능한 동안 반복
                {
                    levelUP++; // 오른 레벨 + 1
                    exp -= levelUpTable[level + levelUP]; // 레벨업에 쓰인 경험치 빼줌

                    if (level + levelUP >= levelUpTable.Length) // 최대 레벨을 찍거나 넘었을 경우
                    {
                        Level += levelUP; // 레벨업을 하고
                        exp = 0; // EXP를 0으로 맞춘 뒤
                        break; // While 문 탈출 (냅두면 인덱스 오류 남)
                    }
                }

                Level += levelUP; // 한번에 레벨을 대입시켜줌
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
            int levelDiff = value - level; // 레벨의 변화량을 나타냄

            if (levelDiff > 0) // 레벨 업
            {
                float rate = 1.1f * levelDiff;
                MaxHP = (int)(maxHP * rate); // 체력 10프로 UP
                DamageScope = (int)(damageScope * rate); // 공격력 10프로 UP
                CurHP = MaxHP;
            }
            else if (levelDiff < 0) // 레벨 다운
            {
                float rate = (10f / 11f) * levelDiff;
                MaxHP = Mathf.Clamp((int)(maxHP * rate), 200, int.MaxValue); // 체력 롤백(시작 체력 밑으론 떨어지지 않음)
                DamageScope = Mathf.Clamp(damageScope * rate, 1f, float.PositiveInfinity); // 공격력 롤백(1배 밑으론 떨어지지 않음)
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

            if (stage < stageUpTable.Length) // 최종 스테이지가 아닐 경우
            {
                if (stageUpTable[stage] <= stage) // 다음 스테이지로 갈 수 있을 만큼 스코어가 쌓였을 경우
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