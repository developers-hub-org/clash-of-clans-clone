namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_BuildingOptions : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;

        private static UI_BuildingOptions _instance = null; public static UI_BuildingOptions instanse { get { return _instance; } }

        public RectTransform infoPanel = null;
        public RectTransform upgradePanel = null;
        public RectTransform instantPanel = null;
        public RectTransform trainPanel = null;
        public RectTransform clanPanel = null;
        public RectTransform spellPanel = null;
        public RectTransform researchPanel = null;
        public RectTransform removePanel = null;
        public RectTransform boostPanel = null;

        public Button infoButton = null;
        public Button upgradeButton = null;
        public Button instantButton = null;
        public Button trainButton = null;
        public Button clanButton = null;
        public Button spellButton = null;
        public Button researchButton = null;
        public Button removeButton = null;
        public Button boostButton = null;

        public TextMeshProUGUI instantCost = null;
        public TextMeshProUGUI removeCost = null;
        public TextMeshProUGUI boostCost = null;
        public Image removeCostIcon = null;
        [HideInInspector] public bool canDo = false;

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        public void SetStatus(bool status)
        {
            if(status && Building.selectedInstanse != null)
            {
                bool isChainging = Building.selectedInstanse.lastChange >= Player.instanse.lastUpdateSent;
                int instantGemCost = 0;
                infoPanel.gameObject.SetActive(UI_Main.instanse.isActive);
                upgradePanel.gameObject.SetActive(!isChainging && Building.selectedInstanse.data.isConstructing == false && UI_Main.instanse.isActive && Building.selectedInstanse.data.id != Data.BuildingID.buildershut && Building.selectedInstanse.data.id != Data.BuildingID.obstacle);
                if (Building.selectedInstanse.data.isConstructing)
                {
                    instantGemCost = Data.GetInstantBuildRequiredGems((int)(Building.selectedInstanse.data.constructionTime - Player.instanse.data.nowTime).TotalSeconds);
                    instantCost.text = instantGemCost.ToString();
                    if(instantGemCost > Player.instanse.data.gems)
                    {
                        instantCost.color = Color.red;
                    }
                    else
                    {
                        instantCost.color = Color.white;
                    }
                    instantCost.ForceMeshUpdate(true);
                }
                instantPanel.gameObject.SetActive(!isChainging && Building.selectedInstanse.data.isConstructing == true && UI_Main.instanse.isActive && instantGemCost > 0);
                if (Building.selectedInstanse.data.id == Data.BuildingID.obstacle && UI_Main.instanse.isActive && Building.selectedInstanse.data.level > 0 && Building.selectedInstanse.data.isConstructing == false)
                {
                    canDo = true;
                    int index = -1;
                    for (int i = 0; i < Player.instanse.initializationData.serverBuildings.Count; i++)
                    {
                        if(Player.instanse.initializationData.serverBuildings[i].id != Data.BuildingID.obstacle.ToString() || Player.instanse.initializationData.serverBuildings[i].level != Building.selectedInstanse.data.level)
                        {
                            continue;
                        }
                        index = i;
                        break;
                    }
                    if(index >= 0)
                    {
                        if(Player.instanse.initializationData.serverBuildings[index].requiredGold > 0)
                        {
                            removeCostIcon.sprite = AssetsBank.instanse.goldIcon;
                            removeCost.text = Player.instanse.initializationData.serverBuildings[index].requiredGold.ToString();
                            if(Player.instanse.gold >= Player.instanse.initializationData.serverBuildings[index].requiredGold)
                            {
                                removeCost.color = Color.white;
                            }
                            else
                            {
                                canDo = false;
                                removeCost.color = Color.red;
                            }
                        }
                        else if (Player.instanse.initializationData.serverBuildings[index].requiredElixir > 0)
                        {
                            removeCostIcon.sprite = AssetsBank.instanse.elixirIcon;
                            removeCost.text = Player.instanse.initializationData.serverBuildings[index].requiredElixir.ToString();
                            if (Player.instanse.elixir >= Player.instanse.initializationData.serverBuildings[index].requiredElixir)
                            {
                                removeCost.color = Color.white;
                            }
                            else
                            {
                                canDo = false;
                                removeCost.color = Color.red;
                            }
                        }
                        else if (Player.instanse.initializationData.serverBuildings[index].requiredDarkElixir > 0)
                        {
                            removeCostIcon.sprite = AssetsBank.instanse.darkIcon;
                            removeCost.text = Player.instanse.initializationData.serverBuildings[index].requiredDarkElixir.ToString();
                            if (Player.instanse.darkElixir >= Player.instanse.initializationData.serverBuildings[index].requiredDarkElixir)
                            {
                                removeCost.color = Color.white;
                            }
                            else
                            {
                                canDo = false;
                                removeCost.color = Color.red;
                            }
                        }
                        else
                        {
                            removeCostIcon.sprite = AssetsBank.instanse.gemsIcon;
                            removeCost.text = Player.instanse.initializationData.serverBuildings[index].requiredGems.ToString();
                            if (Player.instanse.data.gems >= Player.instanse.initializationData.serverBuildings[index].requiredGems)
                            {
                                removeCost.color = Color.white;
                            }
                            else
                            {
                                canDo = false;
                                removeCost.color = Color.red;
                            }
                        }
                    }
                    removePanel.gameObject.SetActive(true);
                    removeCost.ForceMeshUpdate(true);
                }
                else
                {
                    removePanel.gameObject.SetActive(false);
                }
                if ((Building.selectedInstanse.data.id == Data.BuildingID.goldmine || Building.selectedInstanse.data.id == Data.BuildingID.elixirmine || Building.selectedInstanse.data.id == Data.BuildingID.darkelixirmine) && Building.selectedInstanse.data.level > 0)
                {
                    canDo = true;
                    int cost = Data.GetBoostResourcesCost(Building.selectedInstanse.data.id, Building.selectedInstanse.data.level);
                    boostCost.text = cost.ToString();
                    if (Player.instanse.data.gems >= cost)
                    {
                        boostCost.color = Color.white;
                    }
                    else
                    {
                        canDo = false;
                        boostCost.color = Color.red;
                    }
                    boostPanel.gameObject.SetActive(Building.selectedInstanse.data.boost < Player.instanse.data.nowTime);
                    boostCost.ForceMeshUpdate(true);
                }
                else
                {
                    boostPanel.gameObject.SetActive(false);
                }
                trainPanel.gameObject.SetActive(!isChainging && (Building.selectedInstanse.data.id == Data.BuildingID.armycamp || Building.selectedInstanse.data.id == Data.BuildingID.barracks) && UI_Main.instanse.isActive && Building.selectedInstanse.data.level > 0);
                clanPanel.gameObject.SetActive(Building.selectedInstanse.data.id == Data.BuildingID.clancastle && UI_Main.instanse.isActive && Building.selectedInstanse.data.level > 0);
                spellPanel.gameObject.SetActive(Building.selectedInstanse.data.id == Data.BuildingID.spellfactory && UI_Main.instanse.isActive && Building.selectedInstanse.data.level > 0);
                researchPanel.gameObject.SetActive(Building.selectedInstanse.data.id == Data.BuildingID.laboratory && UI_Main.instanse.isActive && Building.selectedInstanse.data.level > 0);
            }
            _elements.SetActive(status);
        }

    }
}