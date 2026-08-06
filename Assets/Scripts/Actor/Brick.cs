using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Actor
{
    public class Brick : MonoBehaviour
    {
        [SerializeField] private TextMeshPro hpText;

        [Header("Health & Score")]
        [SerializeField] private int maxHealth = 1;
        [SerializeField] private int scoreValue = 100;

        private int currentHealth;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            
            Initialize();
            UpdateVisuals();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                TakeDamage(1);
            }
        }

        private void Initialize()
        {
            currentHealth = maxHealth;
        }

        private void TakeDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                DestroyBrick();
            }
            else
            {
                UpdateVisuals();
            }
        }

        private void UpdateVisuals()
        {
            if (hpText != null)
            {
                hpText.text = currentHealth.ToString();
            }
        }

        private void DestroyBrick()
        {
            Destroy(gameObject);
        }
    }
}