// FishingGameManager.cs
using UnityEngine;


public class FishingGameManager : MonoBehaviour
{
    public enum GameState
    {
        Idle,
        Waiting,
        Bite,
        Reeling
    }

    public GameState currentState;

    [Header("References")]
    public FishingUIManager ui;
    public FishSpawner spawner;
    public Animator anim;

    [Header("Animated Object")]
    public Transform animatedObject;

    [Header("Fish Database")]
    public FishData[] fishes;

    FishData currentFish;

    int score;

    float waitTimer;
    float biteTimer;
    float tension;
    float reelTimer;

    bool roundFinished = false;

    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        if (animatedObject != null)
        {
            startPos = animatedObject.position;
            startRot = animatedObject.rotation;
        }

        score = 0;
        ui.UpdateScore(score);

        ResetAnimatorCompletely();
        EnterIdle();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Idle: IdleState(); break;
            case GameState.Waiting: WaitingState(); break;
            case GameState.Bite: BiteState(); break;
            case GameState.Reeling: ReelingState(); break;
        }
    }

    void IdleState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartWaiting();
    }

    void WaitingState()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
            StartBite();
    }

    void BiteState()
    {
        biteTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            StartReeling();

        if (biteTimer <= 0f)
            FailCatch();
    }

    void ReelingState()
    {
        if (roundFinished)
            return;

        tension -= Time.deltaTime * GetDifficultySpeed();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tension += 15f;
            anim.ResetTrigger("Caught");
            anim.ResetTrigger("Lose");
            anim.SetTrigger("Reeling");
            anim.ResetTrigger("Cast");
            anim.ResetTrigger("Idle");
        }

        tension = Mathf.Clamp(tension, 0f, 100f);

        ui.UpdateSlider(tension);

        reelTimer += Time.deltaTime;

        if (tension <= 30f || tension >= 70f)
        {
            roundFinished = true;
            FailCatch();
            return;
        }

        if (reelTimer >= 5f)
        {
            roundFinished = true;
            CatchFish();
        }
    }

    void EnterIdle()
    {
        currentState = GameState.Idle;
        roundFinished = false;

        ResetAnimatedObject();
        ResetAnimatorCompletely();

        ui.ShowIdle();
        spawner.ClearFish();
    }

    void StartWaiting()
    {
        currentState = GameState.Waiting;
        waitTimer = Random.Range(2f, 5f);

        ui.ShowWaiting();

        anim.SetTrigger("Cast");
    }

    void StartBite()
    {
        currentFish = GetRandomFish();

        currentState = GameState.Bite;
        biteTimer = 1.5f;

        ui.ShowBite();

        anim.SetTrigger("Cast");
    }

    void StartReeling()
    {
        currentState = GameState.Reeling;

        tension = 50f;
        reelTimer = 0f;
        roundFinished = false;

        ui.ShowReeling();

        anim.SetTrigger("Reeling");
    }

    void CatchFish()
    {
        currentState = GameState.Idle;

        anim.ResetTrigger("Lose");
        anim.SetTrigger("Caught");

        int addScore = GetFishScore();

        score += addScore;
        ui.UpdateScore(score);

        spawner.SpawnFish(currentFish);

        ui.ShowResult(currentFish, addScore, score);

        Invoke(nameof(NextRound), 2f);
    }

    void FailCatch()
    {
        currentState = GameState.Idle;
        roundFinished = true;

        anim.ResetTrigger("Caught");


        ui.ShowGameOver(score);

        Time.timeScale = 0f;
    }

    void NextRound()
    {
        ui.HideResult();
        EnterIdle();
    }



    void ResetAnimatedObject()
    {
        if (animatedObject == null)
            return;

        animatedObject.position = startPos;
        animatedObject.rotation = startRot;
    }

    void ResetAnimatorCompletely()
    {
        if (anim == null)
            return;

        anim.ResetTrigger("Cast");
        anim.ResetTrigger("Bite");
        anim.ResetTrigger("Reeling");
        anim.ResetTrigger("Caught");


        anim.Rebind();
        anim.Update(0f);
        anim.Play("Idle", 0, 0f);
    }

    FishData GetRandomFish()
    {
        return fishes[Random.Range(0, fishes.Length)];
    }

    int GetFishScore()
    {
        if (currentFish.rarity == FishRarity.Common) return 10;
        if (currentFish.rarity == FishRarity.Uncommon) return 25;
        return 50;
    }

    float GetDifficultySpeed()
    {
        if (currentFish.difficulty == FishDifficulty.Easy) return 10f;
        if (currentFish.difficulty == FishDifficulty.Medium) return 18f;
        return 28f;
    }
}