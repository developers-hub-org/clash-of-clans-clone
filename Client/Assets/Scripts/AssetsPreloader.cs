namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetsPreloader : MonoBehaviour
    {

        [SerializeField] private GameObject[] assets = null;

        private void Awake()
        {
            if (assets != null && assets.Length > 0)
            {
                Vector3 position = Camera.main.transform.position + Camera.main.transform.forward.normalized * 10f;
                for (int i = 0; i < assets.Length; i++)
                {
                    if(assets[i] != null)
                    {
                        GameObject asset = Instantiate(assets[i], position, Quaternion.identity);
                        Destroy(asset, 1f);
                    }
                }
            }
            Destroy(gameObject);
        }

    }
}