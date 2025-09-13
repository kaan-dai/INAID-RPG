using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.HealthSystemCM {
    public class HealthBarUI : MonoBehaviour {
        [Tooltip("Optional; Either assign a reference in the Editor (that implements IGetHealthSystem) or manually call SetHealthSystem()")]
        [SerializeField] private GameObject getHealthSystemGameObject;

        [Tooltip("Image to show the Health Bar, should be set as Fill, the script modifies fillAmount")]
        [SerializeField] private Image image;

        private HealthSystem healthSystem;

        private void Start() {
            if (getHealthSystemGameObject == null) {
              
                return;
            }

            if (HealthSystem.TryGetHealthSystem(getHealthSystemGameObject, out HealthSystem healthSystem)) {
                SetHealthSystem(healthSystem);
            } else {
                
            }
        }

        private void Update() {
            UpdateHealthBar();
        }

        public void SetHealthSystem(HealthSystem healthSystem) {
            if (this.healthSystem != null) {
                this.healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
            }
            this.healthSystem = healthSystem;

            UpdateHealthBar();

            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }

        private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e) {
            UpdateHealthBar();
        }

        private void UpdateHealthBar() {
            if (healthSystem != null) {
                image.fillAmount = healthSystem.GetHealthNormalized();
            }
        }

        private void OnDestroy() {
            if (healthSystem != null) {
                healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
            }
        }
    }
}
