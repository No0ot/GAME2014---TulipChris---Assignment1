using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum DetailsPanelSetting
{
    BUILD,
    UPGRADE
}

public class DetailsPanel : MonoBehaviour
{
    public Image portraitRef;
    public TMP_Text resourceCost;
    public Image resource;

    public RectTransform damageBar;
    public RectTransform speedBar;
    public RectTransform rangeBar;

    float damageMax = 25;
    float speedMax = 3;
    float rangeMax = 10;

    float damageRef;
    float speedRef;
    float rangeRef;

    public GameObject targetReference;
    TowerScript towerReference;

    public Sprite goldImage;
    public Sprite ironImage;
    public Sprite steelImage;

    public GameObject buildPanel;
    public GameObject upgradePanel;

    public GameObject basicTowerPrefab;
    public GameObject rapidTowerPrefab;
    public GameObject quakeTowerPrefab;
    public GameObject missleTowerPrefab;

    public DetailsPanelSetting setting;

    public TMP_Text towerLevel;
    public TMP_Text towerKills;

    public void UpdateTargetReference(GameObject target, DetailsPanelSetting new_Setting)
    {
        targetReference = target;
        towerReference = targetReference.GetComponent<TowerScript>();
        setting = new_Setting;
        Refresh();
    }

    public void UpdateTargetReference(TowerType target, DetailsPanelSetting new_Setting)
    {
        switch(target)
        {
            case TowerType.BASIC:
                targetReference = basicTowerPrefab;
                break;
            case TowerType.RAPID:
                targetReference = rapidTowerPrefab;
                break;
            case TowerType.QUAKE:
                targetReference = quakeTowerPrefab;
                break;
            case TowerType.MISSLE:
                targetReference = missleTowerPrefab;
                break;
        }

        towerReference = targetReference.GetComponent<TowerScript>();
        setting = new_Setting;
        Refresh();
    }

    void UpdateBars()
    {
        damageBar.offsetMin = new Vector2(260 - (260 * ((float)towerReference.damage / (float)damageMax)), damageBar.offsetMax.y);
        speedBar.offsetMin = new Vector2(260 * ((float)towerReference.fireRate / (float)speedMax), speedBar.offsetMax.y);
        rangeBar.offsetMin = new Vector2(260 - (260 * ((float)towerReference.range / (float)rangeMax)), speedBar.offsetMax.y);
    }

    void UpdateTowerStats()
    {
        towerLevel.SetText(towerReference.level.ToString());
        towerKills.SetText(towerReference.kills.ToString());
    }

    void Refresh()
    {
        switch (setting)
        {
            case DetailsPanelSetting.BUILD:
                buildPanel.SetActive(true);
                upgradePanel.SetActive(false);
                UpdateBars();
                break;
            case DetailsPanelSetting.UPGRADE:
                buildPanel.SetActive(false);
                upgradePanel.SetActive(true);
                UpdateTowerStats();
                break;
        }
        UpdatePortrait();
        UpdateCost();
    }

    void UpdatePortrait()
    {
        portraitRef.sprite = targetReference.GetComponent<SpriteRenderer>().sprite;
    }

    void UpdateCost()
    {
        switch(setting)
        {
            case DetailsPanelSetting.BUILD:
                resource.sprite = goldImage;
                resourceCost.SetText(towerReference.goldCost.ToString());
                break;
            case DetailsPanelSetting.UPGRADE:
              switch(towerReference.level)
              {
                  case 1:
                      resource.sprite = ironImage;
                      resourceCost.SetText(towerReference.ironCost.ToString());
                      break;
                  case 2:
                      resource.sprite = steelImage;
                      resourceCost.SetText(towerReference.steelCost.ToString());
                      break;
                  case 3:
                      // find something to do here
                      break;
              }
                break;
        }
    }

    public void UpgradeButton()
    {
        if(setting == DetailsPanelSetting.UPGRADE)
        {
            switch (towerReference.level)
            {
                case 1:
                    if (PlayerStats.Instance.iron >= towerReference.ironCost)
                    {
                        towerReference.UpgradeTower();
                        Refresh();
                    }
                    else
                        GameplayUIManager.Instance.DisplayErrorText("Need more iron!");
                    break;
                case 2:
                    if (PlayerStats.Instance.steel >= towerReference.steelCost)
                    {
                        towerReference.UpgradeTower();
                        Refresh();
                    }
                    else
                        GameplayUIManager.Instance.DisplayErrorText("Need more steel!");
                    break;
                case 3:
                    // find something to do here
                    break;
            }
        }
    }
}
