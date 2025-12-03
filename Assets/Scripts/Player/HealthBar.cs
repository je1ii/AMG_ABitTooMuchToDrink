using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance { get; private set; }
    
    [Header("UI")]
    [SerializeField] private Image primaryBar;   // instant bar (shows current HP)
    [SerializeField] private Image secondaryBar; // ghost/delayed bar

    [Header("Health")]
    [SerializeField] private float maxHealth; // total hp of enemy
    [SerializeField] private bool isInvulnerable;

    [Header("Damage / Drain")]
    [Range(0.05f, 2f)]
    [SerializeField] private float timeToDrain = 0.2f; // seconds for ghost to catch up

    public float _currentHealth;
    private float _ghostHealth; 
    private float _drainTimer; 

    private float _drainStartHealth;
    private float _drainTargetHealth;

    private enum State { Idle, Draining}
    private State _state = State.Idle;
    
    public void SetInvulnerable(bool value) => isInvulnerable = value;

    public bool GetInvulnerableValue()
    {
        return isInvulnerable;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    void Start()
    {
        // setting up default values
        _currentHealth = maxHealth;
        _ghostHealth = maxHealth;
        _drainTimer = 0f;

        isInvulnerable = false;

        if (primaryBar != null && secondaryBar != null) UpdateUI();
    }

    void Update()
    {
        if (primaryBar == null || secondaryBar == null) return;

        var dt = Time.deltaTime;

        switch (_state)
        {
            case State.Draining: 
                _drainTimer += dt;
                var drainTime = Mathf.Clamp01(_drainTimer / Mathf.Max(0.0001f, timeToDrain));
                _ghostHealth = Mathf.Lerp(_drainStartHealth, _drainTargetHealth, drainTime);
                UpdateUI();

                if (drainTime >= 1f)
                {
                    _state = State.Idle;
                }
                break;

            case State.Idle:
                // both bars show current amounts
                UpdateUI();
                break;
        }

        if (isInvulnerable)
        {
            IntoxicationBar.Instance.ResetIntoxication();
        }
    }
    
    private void UpdateUI()
    {
        primaryBar.fillAmount = _currentHealth / maxHealth;
        secondaryBar.fillAmount = _ghostHealth / maxHealth;
    }

    private IEnumerator InvulnerableTimer(float time)
    {
        yield return new WaitForSeconds(time);
        isInvulnerable = false;
    }

    public void StartTimer(float time)
    {
        StartCoroutine(InvulnerableTimer(time));
    }

    private IEnumerator PlayerDied()
    {
        Debug.Log("Player died.");
        yield return new WaitForSeconds(1.5f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void KillPlayer()
    {
        StartCoroutine(PlayerDied());
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void TakeDamage(float damage)
    {
        // checks if is already dead
        if (_currentHealth <= 0f) return;

        if (isInvulnerable) return;

        // apply damage
        _currentHealth -= damage;

        // setup drain from current ghost value toward the new health
        _drainStartHealth = _ghostHealth;
        _drainTargetHealth = _currentHealth;
        _drainTimer = 0f;
        _state = State.Draining;

        if (_currentHealth <= 0f)
        {
            StartCoroutine(PlayerDied());
        }
    }

    public bool Heal(float amount)
    {
        if (_currentHealth <= 0f || _currentHealth >= maxHealth) return false;
        
        _currentHealth += amount;
        
        return true;
    }

    public void TestDamage()
    {
        TakeDamage(1f);
    }
}
