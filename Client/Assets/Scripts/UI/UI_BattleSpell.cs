namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_BattleSpell : MonoBehaviour
    {

        [SerializeField] private Image _icon = null;
        [SerializeField] private TextMeshProUGUI _haveText = null;
        [SerializeField] private Button _buttonSelect = null;
        [SerializeField] private GameObject _selectEffects = null;

        [HideInInspector] public Data.SpellID id = Data.SpellID.lightning;
        public int count { get { return spells.Count; } }
        private List<long> spells = new List<long>();

        public void Initialize(Data.SpellID id)
        {
            Sprite icon = AssetsBank.GetSpellIcon(id);
            if (icon != null)
            {
                _icon.sprite = icon;
            }
            this.id = id;
        }

        public void Add(long id)
        {
            spells.Add(id);
            _haveText.text = spells.Count.ToString();
            _haveText.ForceMeshUpdate(true);
        }

        public long Get()
        {
            long value = -1;
            if (spells.Count > 0)
            {
                value = spells[0];
                spells.RemoveAt(0);
            }
            _haveText.text = "x" + spells.Count.ToString();
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
            UI_Battle.instanse.SpellSelected(id);
        }

        public void Deselect()
        {
            _selectEffects.SetActive(false);
        }

    }
}