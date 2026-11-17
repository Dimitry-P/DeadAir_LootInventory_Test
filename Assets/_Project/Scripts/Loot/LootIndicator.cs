using UnityEngine;

public class LootIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject indicatorVisual;
    [SerializeField] private Transform targetCamera;

    [Header("Settings")]
    [SerializeField] private float detectionRadius = 2.5f;

    private void Start()
    {
        if (indicatorVisual != null)
        {
            indicatorVisual.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null || indicatorVisual == null)
        {
            if (targetCamera != null)
            {
                indicatorVisual.transform.rotation = Quaternion.LookRotation(
                    indicatorVisual.transform.position - targetCamera.position
                );
            }
        }
        
        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        indicatorVisual.SetActive(distance <= detectionRadius);
    }
}