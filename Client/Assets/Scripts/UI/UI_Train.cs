namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Train : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private TextMeshProUGUI _housingText = null;
        [SerializeField] private UI_UnitsTraining _trainPrefab = null;
        [SerializeField] private RectTransform _trainingGridRoot = null;
        [SerializeField] private RectTransform _trainGrid = null;
        [SerializeField] private RectTransform _listRoot = null;
        [SerializeField] private GridLayoutGroup _listLayout = null;
        [SerializeField] private UI_Unit _unitPrefab = null;
        [SerializeField] private Data.UnitID[] _unitsAvailable = null; public Data.UnitID[] unitsList { get { return _unitsAvailable; } }

        private static UI_Train _instance = null; public static UI_Train instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }
        private List<UI_UnitsTraining> trainigItems = new List<UI_UnitsTraining>(); public int trainigItemsCount { get { return trainigItems.Count; } }
        private List<UI_Unit> uiUnits = new List<UI_Unit>();

        private float trainingItemHeight = 1;
        private int _occupiedHousing = 0;
        private int _availableHousing = 0; public int freeHousing { get { return _availableHousing - _occupiedHousing; } }
        private int _targetIndex = 0; public int targetIndex { get { return _targetIndex; } set { _targetIndex = value; }}
        
        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void ClearTrainingItems()
        {
            for (int i = 0; i < trainigItems.Count; i++)
            {
                if (trainigItems[i])
                {
                    Destroy(trainigItems[i].gameObject);
                }
            }
            trainigItems.Clear();
        }

        public void Initialize()
        {
            for (int i = 0; i < uiUnits.Count; i++)
            {
                for (int j = 0; j < Player.instanse.initializationData.serverUnits.Count; j++)
                {
                    if (uiUnits[i].id == Player.instanse.initializationData.serverUnits[j].id)
                    {
                        uiUnits[i].Initialize(Player.instanse.initializationData.serverUnits[j]);
                        break;
                    }
                }
            }
        }

        private void Start()
        {
            if(_unitsAvailable != null)
            {
                Data.UnitID[] unitsAvailable = _unitsAvailable.Distinct().ToArray();
                for (int i = 0; i < unitsAvailable.Length; i++)
                {
                    UI_Unit unit = Instantiate(_unitPrefab, _listLayout.transform);
                    unit.id = unitsAvailable[i];
                    uiUnits.Add(unit);
                }
            }
            _closeButton.onClick.AddListener(Close);
            float space = Screen.height * 0.01f;
            float height = ((Screen.height * _listRoot.anchorMax.y) - (space * 3f)) / 2f;
            _listLayout.spacing = new Vector2(space, space);
            _listLayout.cellSize = new Vector2(height * 0.9f, height);
            trainingItemHeight = (_trainingGridRoot.anchorMax.y - _trainingGridRoot.anchorMin.y) * Screen.height;
        }

        public void Open()
        {
            ClearTrainingItems();
            UI_Main.instanse.SetStatus(false);
            Initialize();
            Sync();
            _elements.SetActive(true);
            _active = true;
            Player.instanse.RushSyncRequest();
        }

        private void UpdateTrainingList()
        {
            for (int i = 0; i < Player.instanse.data.units.Count; i++)
            {
                if (Player.instanse.data.units[i].ready == false)
                {
                    int x1 = -1;
                    int x2 = -1;
                    for (int j = 0; j < trainigItems.Count; j++)
                    {
                        if (trainigItems[j] && Player.instanse.data.units[i].databaseID == trainigItems[j].databaseID)
                        {
                            x1 = j;
                            break;
                        }
                        if (trainigItems[j] && trainigItems[j].databaseID <= 0 && Player.instanse.data.units[i].id == trainigItems[j].id)
                        {
                            x2 = j;
                        }
                    }
                    if (x1 >= 0)
                    {
                        trainigItems[x1].Initialize(Player.instanse.data.units[i]);
                    }
                    else
                    {
                        if (x2 >= 0)
                        {
                            trainigItems[x2].Initialize(Player.instanse.data.units[i]);
                        }
                        else
                        {
                            UI_UnitsTraining unit = Instantiate(_trainPrefab, _trainGrid.transform);
                            unit.Initialize(Player.instanse.data.units[i]);
                            RectTransform rect = unit.GetComponent<RectTransform>();
                            rect.sizeDelta = new Vector2(trainingItemHeight, trainingItemHeight);
                            trainigItems.Add(unit);
                        }
                    }
                }
            }
        }

        public void StartTrainUnit(Data.UnitID id)
        {
            UI_UnitsTraining unit = Instantiate(_trainPrefab, _trainGrid.transform);
            unit.Initialize(id);
            RectTransform rect = unit.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(trainingItemHeight, trainingItemHeight);
            trainigItems.Add(unit);
        }

        public void Close()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            ClearTrainingItems();
            _elements.SetActive(false);
            _active = false;
            UI_Main.instanse.SetStatus(true);
        }

        public void Sync()
        {
            int occupied = 0;
            int available = 0;
            for (int i = 0; i < uiUnits.Count; i++)
            {
                uiUnits[i].Sync();
                occupied += uiUnits[i].housing;
            }

            for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
            {
                if(Player.instanse.data.buildings[i].id != Data.BuildingID.armycamp) { continue; }
                available += Player.instanse.data.buildings[i].capacity;
            }

            _availableHousing = available;
            _occupiedHousing = occupied;

            _housingText.text = occupied.ToString() + "/" + available.ToString();

            RemoveTrainedItems();
            UpdateTrainingList();
            
            _housingText.ForceMeshUpdate(true);
        }

        private void RemoveTrainedItems()
        {
            for (int i = trainigItems.Count - 1; i >= 0; i--)
            {
                for (int j = Player.instanse.data.units.Count - 1; j >= 0; j--)
                {
                    if (Player.instanse.data.units[j].databaseID == trainigItems[i].databaseID)
                    {
                        RemoveTrainingItem(trainigItems[i]);
                        break;
                    }
                }
            }
        }
        
        private void Update()
        {
            for (int i = 0; i < trainigItems.Count; i++)
            {
                if (trainigItems[i].done)
                {
                    /*
                    if (trainigItems[i].isTrained)
                    {
                        trainigItems[i].gameObject.SetActive(false);
                    }
                    */
                    continue;
                }
                trainigItems[i].UpdateStatus(Time.deltaTime);
                break;
            }
        }

        public void RemoveTrainingItem(UI_UnitsTraining item)
        {
            if (item == null)
            {
                return;
            }
            trainigItems.Remove(item);
            Destroy(item.gameObject);
        }
        
    }
}