using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image primaryBar;   // instant bar (shows current HP)
    [SerializeField] private Image secondaryBar; // ghost/delayed bar

    [Header("Health")]
    [SerializeField] private float maxHealth; // total hp of enemy

    [Header("Damage / Drain")]
    [Range(0.05f, 2f)]
    [SerializeField] private float timeToDrain = 0.2f; // seconds for ghost to catch up

    public float _currentHealth;
    private float _ghostHealth; 
    private float _drainTimer; 

    private float _drainStartHealth;
    private float _drainTargetHealth;

    private enum State { Idle, Draining, Regenerating }
    private State _state = State.Idle;

    void Start()
    {
        // setting up default values
        _currentHealth = maxHealth;
        _ghostHealth = maxHealth;
        _drainTimer = 0f;

        if (primaryBar != null) primaryBar.fillAmount = 1f;
        if (secondaryBar != null) secondaryBar.fillAmount = 1f;
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
                secondaryBar.fillAmount = _ghostHealth / maxHealth;

                // keep primary showing the immediate value
                primaryBar.fillAmount = _currentHealth / maxHealth;

                if (drainTime >= 1f)
                {
                    // finished draining -> go to idle and start regen timer
                    _state = State.Idle;
                }
                break;

            case State.Idle:
                // both bars show current amounts
                primaryBar.fillAmount = _currentHealth / maxHealth;
                secondaryBar.fillAmount = _ghostHealth / maxHealth;
                break;
        }
    }
    
    public void TakeDamage(float damage)
    {
        // checks if is already dead
        if (_currentHealth <= 0f) return;

        // apply damage
        _currentHealth -= damage;

        // update primary immediately
        primaryBar.fillAmount = _currentHealth / maxHealth;

        // setup drain from current ghost value toward the new health
        _drainStartHealth = _ghostHealth;
        _drainTargetHealth = _currentHealth;
        _drainTimer = 0f;
        _state = State.Draining;

        // if died, notify parent
        if (_currentHealth <= 0f)
        {
            // TODO: insert death function
        }
    }
}
