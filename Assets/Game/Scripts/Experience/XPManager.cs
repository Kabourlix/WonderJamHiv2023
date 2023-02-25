using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public int currentXP, targetXP, level;

    public static XPManager instance;

    [SerializeField] private MeshFilter currentModel;
    [SerializeField] private Mesh targetModelLVL1, targetModelLVL2;

    private void Awake()
    {
        if (instance == null)
            instance = this; 
        else
            Destroy(gameObject);
    } 

    void Start()
    {
        level = 0;
        currentXP = 0;
        targetXP = 30;
    }

    void Update()
    {
            if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("allo");
            XPManager.instance.AddXP(10);
        }
    }
    
    public void AddXP(int xp)
    {
        currentXP += xp;

        if (currentXP >= targetXP)
        {
            currentXP = targetXP - currentXP;
            level++;
            LevelUp();
        }
    }

        void LevelUp()
    {
        switch (level)
        {
            
            case 1:
                currentModel.mesh = targetModelLVL1;
                break;

            case 2:
                currentModel.mesh = targetModelLVL2;
                break;
        }
    }
}
