namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_BattleUnit : MonoBehaviour
    {

        [SerializeField] private Image _icon = null;
        [SerializeField] private TextMeshProUGUI _haveText = null;
        [SerializeField] private Button _buttonSelect = null;
        [SerializeField] private GameObject _selectEffects = null;

        [HideInInspector] public Data.UnitID id = Data.UnitID.barbarian;
        public int count { get { return units.Count; } }
        private List<long> units = new List<long>();

        public void Initialize(Data.UnitID id)
        {
            Sprite icon = AssetsBank.GetUnitIcon(id);
            if (icon != null)
            {
                _icon.sprite = icon;
            }
            this.id = id;
        }

        public void Add(long id)
        {
            units.Add(id);
            _haveText.text = "x" + units.Count.ToString();
            _haveText.ForceMeshUpdate(true);
        }

        public long Get()
        {
            long value = -1;
            if (units.Count > 0)
            {
                value = units[0];
                units.RemoveAt(0);
            }
            _haveText.text = units.Count.ToString();
            _haveText.ForceMeshUpdate(true);
            return value;
        }

        private void Start()
        {
            _selectEffects.SetActive(false);
            _buttonSelect.onClick.AddListener(Select);
        }

        public void Select()
        {
            _selectEffects.SetActive(true);
            UI_Battle.instanse.UnitSelected(id);
        }

        public void Deselect()
        {
            _selectEffects.SetActive(false);
        }

    }
}