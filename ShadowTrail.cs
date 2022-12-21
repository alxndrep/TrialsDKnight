using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShadowTrail : MonoBehaviour
{
    [SerializeField] public GameObject Sombra;
    public List<GameObject> pool = new List<GameObject>();
    private float cronometro;
    [SerializeField] public float velocidad;
    [SerializeField] public Color _Color;
    [SerializeField] private string nombreLayer;
    [SerializeField] private int orderLayer;
    private void Start()
    {
        pool = new List<GameObject>();
    }
    public GameObject GetShadows()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if(pool[i] != null)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive(true);
                    pool[i].transform.position = transform.position;
                    pool[i].transform.rotation = transform.rotation;
                    pool[i].GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                    pool[i].GetComponent<SolidTrail>()._Color = _Color;
                    return pool[i];
                }
            }
        }
        GameObject obj = Instantiate(Sombra, transform.position, transform.rotation) as GameObject;
        obj.GetComponent<SpriteRenderer>().sortingLayerName = nombreLayer;
        obj.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;
        obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        obj.GetComponent<SolidTrail>()._Color = _Color;
        pool.Add(obj);
        return obj;
    }

    public void Sombras_Skill()
    {
        cronometro += velocidad * Time.deltaTime;
        if (cronometro > 1)
        {
            GetShadows();
            cronometro = 0;
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject shadow in pool)
        {
            Destroy(shadow.gameObject);
        }
    }
}
