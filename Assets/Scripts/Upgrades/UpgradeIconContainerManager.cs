using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeIconContainerManager : MonoBehaviour
{
    public int curIdx;
    
    private void Start()
    {
        SetCurIdx();
    }
    
    private void SetCurIdx()
    {
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                if (parentTransform.GetChild(i+2) == transform)
                {
                    curIdx = i;
                    break;
                }
            }
        }
    }
    
    public void ClickHandler()
    {
        if (transform.parent.GetComponent<UpgradeManager>().canSelect[curIdx] == false)
        {
            transform.GetComponent<UIClickFeedback>().OnClick();
            transform.parent.GetChild(5).gameObject.SetActive(false);
            return;
        }
        bool currentValue = transform.parent.GetComponent<UpgradeManager>().isSelected[curIdx];
        transform.parent.GetComponent<UpgradeManager>().isSelected[curIdx] = !currentValue;
        
        if (!currentValue)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.parent.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
