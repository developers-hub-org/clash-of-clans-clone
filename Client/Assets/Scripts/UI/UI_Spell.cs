namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Spell : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private TextMeshProUGUI _housingText = null;
        [SerializeField] private UI_SpellBrewing _brewPrefab = null;
        [SerializeField] private RectTransform _brewingGridRoot = null;
        [SerializeField] private RectTransform _brewGrid = null;
        [SerializeField] private RectTransform _listRoot = null;
        [SerializeField] private GridLayoutGroup _listLayout = null;
        [SerializeField] private UI_SpellItem _spellPrefab = null;
        [SerializeField] private Data.SpellID[] _spellsAvailable = null; public Data.SpellID[] spellsList { get { return _spellsAvailable; } }

        private static UI_Spell _instance = null; public static UI_Spell instanse { get { return _instance; } }

        private List<UI_SpellBrewing> brewingItems = new List<UI_SpellBrewing>();
        private List<UI_SpellItem> uiSpells = new List<UI_SpellItem>();

        private float brewingItemHeight = 1;
        private int _occupiedHousing = 0;
        private int _availableHousing = 0; public int freeHousing { get { return _availableHousing - _occupiedHousing; } }

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        [HideInInspector] public bool isOpen = false;

        private void ClearBrewingItems()
        {
            for (int i = 0; i < brewingItems.Count; i++)
            {
                if (brewingItems[i])
                {
                    Destroy(brewingItems[i].gameObject);
                }
            }
            brewingItems.Clear();
        }

        public void Initialize()
        {
            for (int i = 0; i < uiSpells.Count; i++)
            {
                for (int j = 0; j < Player.instanse.initializationData.serverSpells.Count; j++)
                {
                    if (uiSpells[i].id == Player.instanse.initializationData.serverSpells[j].id)
                    {
                        uiSpells[i].Initialize(Player.instanse.initializationData.serverSpells[j]);
                        break;
                    }
                }
            }
        }

        private void Start()
        {
            if (_spellsAvailable != null)
            {
                Data.SpellID[] spellsAvailable = _spellsAvailable.Distinct().ToArray();
                for (int i = 0; i < spellsAvailable.Length; i++)
                {
                    UI_SpellItem spell = Instantiate(_spellPrefab, _listLayout.transform);
                    spell.id = spellsAvailable[i];
                    uiSpells.Add(spell);
                }
            }
            _closeButton.onClick.AddListener(Close);
            float space = Screen.height * 0.01f;
            float height = ((Screen.height * _listRoot.anchorMax.y) - (space * 3f)) / 2f;
            _listLayout.spacing = new Vector2(space, space);
            _listLayout.cellSize = new Vector2(height * 0.9f, height);
            brewingItemHeight = (_brewingGridRoot.anchorMax.y - _brewingGridRoot.anchorMin.y) * Screen.height;
        }

        public void SetStatus(bool status)
        {
            ClearBrewingItems();
            if (status)
            {
                UI_Main.instanse.SetStatus(false);
                Initialize();
                Sync();
            }
            _elements.SetActive(status);
            isOpen = status;
        }

        private void UpdateBrewingList()
        {
            for (int i = 0; i < Player.instanse.data.spells.Count; i++)
            {
                if (Player.instanse.data.spells[i].ready == false)
                {
                    int x1 = -1;
                    int x2 = -1;
                    for (int j = 0; j < brewingItems.Count; j++)
                    {
                        if (brewingItems[j] && Player.instanse.data.spells[i].databaseID == brewingItems[j].databaseID)
                        {
                            x1 = j;
                            break;
                        }
                        if (brewingItems[j] && brewingItems[j].databaseID <= 0 && Player.instanse.data.spells[i].id == brewingItems[j].id)
                        {
                            x2 = j;
                        }
                    }
                    if (x1 >= 0)
                    {

                    }
                    else
                    {
                        if (x2 >= 0)
                        {
                            brewingItems[x2].Initialize(Player.instanse.data.spells[i]);
                        }
                        else
                        {
                            UI_SpellBrewing spell = Instantiate(_brewPrefab, _brewGrid.transform);
                            spell.Initialize(Player.instanse.data.spells[i]);
                            RectTransform rect = spell.GetComponent<RectTransform>();
                            rect.sizeDelta = new Vector2(brewingItemHeight, brewingItemHeight);
                            brewingItems.Add(spell);
                        }
                    }
                }
            }
            ResetBrewingItemsIndex();
        }

        public void StartBrewingSpell(Data.SpellID id)
        {
            UI_SpellBrewing spell = Instantiate(_brewPrefab, _brewGrid.transform);
            spell.Initialize(id);
            RectTransform rect = spell.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(brewingItemHeight, brewingItemHeight);
            brewingItems.Add(spell);
        }

        private void Close()
        {
            SetStatus(false);
            UI_Main.instanse.SetStatus(true);
        }

        public void Sync()
        {
            int occupied = 0;
            int available = 0;
            for (int i = 0; i < uiSpells.Count; i++)
            {
                uiSpells[i].Sync();
                occupied += uiSpells[i].housing;
            }

            for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
            {
                if (Player.instanse.data.buildings[i].id != Data.BuildingID.spellfactory) { continue; }
                available += Player.instanse.data.buildings[i].capacity;
                break;
            }

            _availableHousing = available;
            _occupiedHousing = occupied;

            _housingText.text = occupied.ToString() + "/" + available.ToString();

            for (int i = brewingItems.Count - 1; i >= 0; i--)
            {
                if (brewingItems[i])
                {
                    int x = -1;
                    for (int j = 0; j < Player.instanse.data.spells.Count; j++)
                    {
                        if (Player.instanse.data.spells[j].databaseID == brewingItems[i].databaseID)
                        {
                            x = j;
                            break;
                        }
                    }
                    if (x >= 0)
                    {
                        if (Player.instanse.data.spells[x].ready)
                        {
                            RemoveTrainingItem(i);
                        }
                    }
                    else
                    {
                        if (brewingItems[i].remove)
                        {
                            RemoveTrainingItem(i);
                        }
                    }
                }
                else
                {
                    RemoveTrainingItem(i);
                }
            }
            UpdateBrewingList();
        }

        public void RemoveTrainingItem(int i)
        {
            if (i < 0 || i >= brewingItems.Count)
            {
                return;
            }
            if (brewingItems[i])
            {
                Destroy(brewingItems[i].gameObject);
            }
            brewingItems.RemoveAt(i);
            ResetBrewingItemsIndex();
        }

        public void ResetBrewingItemsIndex()
        {
            for (int j = 0; j < brewingItems.Count; j++)
            {
                if (brewingItems[j])
                {
                    brewingItems[j].index = j;
                }
            }
        }

    }
}