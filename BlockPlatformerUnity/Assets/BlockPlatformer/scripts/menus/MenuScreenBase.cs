using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
    
    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
}
