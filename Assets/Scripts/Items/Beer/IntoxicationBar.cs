using UnityEngine;
using UnityEngine.UI;

public class IntoxicationBar : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private Image primaryBar;
    [SerializeField] private Image secondaryBar;
    
    [Header("Intoxication")] 
    [SerializeField] private float currentIntoxication;
    private float maxIntoxication = 100f;
    private float ghostIntoxication;
    
    [Header("Sober Rate")]
    [SerializeField] private float soberDrunkRate = 0.8f;
    [SerializeField] private float soberDizzyRate = 0.5f;
    [SerializeField] private float soberNormalRate = 0.1f;
    [SerializeField] private float soberDelay = 1f;
    
    [Header("Reference")]
    [SerializeField] private PlayerMovement movement;
    
    private float soberTimer;
    private float soberRate;
    
    private float drainTimer; 
    private float timeToDrain = 0.2f;
    private float drainStart;
    private float drainTarget;

    enum BarState { Idle, Draining, Sobering }
    private BarState barState = BarState.Idle;

    void Start()
    {
        currentIntoxication = 0f;
        ghostIntoxication = currentIntoxication;
        drainTimer = 0f;
        soberTimer = 0f;
        
        UpdateUI();
    }

    void Update()
    {
        if (primaryBar == null || secondaryBar == null) return;
        
        float dt = Time.deltaTime;
        UpdateBarStatus(dt);

        if (currentIntoxication >= 70)
        {
            soberRate = soberDrunkRate;
            movement.SetPlayerState(true, false);
        }
        else if (currentIntoxication >= 40)
        {
            soberRate = soberDizzyRate;
            movement.SetPlayerState(false, true);
        }
        else
        {
            soberRate = soberNormalRate;
            movement.SetPlayerState(false, false);
        }
    }

    private void UpdateBarStatus(float dt)
    {
        switch (barState)
        {
            case BarState.Draining: 
                drainTimer += dt;
                var drainTime = Mathf.Clamp01(drainTimer / Mathf.Max(0.0001f, timeToDrain));
                ghostIntoxication = Mathf.Lerp(drainStart, drainTarget, drainTime);
                UpdateUI();

                if (drainTime >= 1f)
                {
                    barState = BarState.Idle;
                    soberRate = 0f;
                }
                break;

            case BarState.Idle:
                UpdateUI();

                soberTimer += dt;
                if (soberTimer >= soberDelay && currentIntoxication > 0)
                {
                    barState = BarState.Sobering;
                }
                break;

            case BarState.Sobering:
                currentIntoxication -= soberRate * dt;
                currentIntoxication = Mathf.Max(currentIntoxication, 0f);
                
                ghostIntoxication = currentIntoxication; 
                UpdateUI();
                
                if (currentIntoxication <= 0f)
                {
                    barState = BarState.Idle;
                    soberTimer = 0f;
                }
                break;
        }
    }
    
    private void UpdateUI()
    {
        primaryBar.fillAmount = currentIntoxication / maxIntoxication;
        secondaryBar.fillAmount = ghostIntoxication / maxIntoxication;
    }

    public void DoneConsuming(float intoxicationValue)
    {
        currentIntoxication += intoxicationValue;
        UpdateUI();

        drainStart = ghostIntoxication;
        drainTarget = currentIntoxication;
        drainTimer = 0f;
        barState = BarState.Draining;
        
        soberTimer = 0f;
        
        if(currentIntoxication >= maxIntoxication) currentIntoxication = maxIntoxication;
    }
}
