using UnityEngine;

public class Bomb : MonoBehaviour
{
    public bool isVR;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            if (isVR)
            {
                GameManagerVR.Instance.Explode();
            }
            else
            {
                GameManager.Instance.Explode();
            }
        }
    }

}
