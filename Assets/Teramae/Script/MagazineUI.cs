using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineUI : MonoBehaviour
{
    [SerializeField, Header("�}�K�W���C���[�W")]
    List<GameObject> magazineImg = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < GameObject.Find("Player").GetComponent<Player>().GetMagazineNum(); i++)
            DrawMagazine(i);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �}�K�W���\��
    /// </summary>
    /// <param name="magazine"></param>
    public void DrawMagazine(int magazine)
    {
        magazineImg[magazine].SetActive(true);
    }

    /// <summary>
    /// �}�K�W����\��
    /// </summary>
    /// <param name="magazine"></param>
    public void HiddenMagazine(int magazine)
    {
        magazineImg[magazine].SetActive(false);
    }
}
