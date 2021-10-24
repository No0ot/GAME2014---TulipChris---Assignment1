//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 24, 2021
//      File            : DetailsPanel.cs
//      Description     : This script contains methods used for the details panel in the gameplay scene.
//      History         :   v0.7 - Added Script and an enum to determine what setting the details panel should display along with getting and updating the window based on the target reference.
//                          v0.75 - Added functionality for the build portion of the details panel
//                          v0.9 - Added Functionality for the upgrade portion of the panel.
//
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
