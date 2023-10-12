namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Research : MonoBehaviour
    {

        [SerializeField] private GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private RectTransform _listRoot = null;
        [SerializeField] private GridLayoutGroup _listLayout = null;
        [SerializeField] private UI_ResearchUnit _unitPrefab = null;
        [SerializeField] private UI_ResearchSpell _spellPrefab = null;
        private static UI_Research _instance = null; public static UI_Research instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }

        private List<UI_ResearchUnit> units = new List<UI_ResearchUnit>();
        private List<UI_ResearchSpell> spells = new List<UI_ResearchSpell>();

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
            float space = Screen.height * 0.01f;
            float height = ((Screen.height * _listRoot.anchorMax.y) - (space * 4f)) / 3f;
            _listLayout.spacing = new Vector2(space, space);
            _listLayout.cellSize = new Vector2(height * 0.9f, height);
            for (int i = 0; i < UI_Train.instanse.unitsList.Length; i++)
            {
                UI_ResearchUnit unit = Instantiate(_unitPrefab, _listLayout.transform);
                unit.id = UI_Train.instanse.unitsList[i];
                units.Add(unit);
            }
            for (int i = 0; i < UI_Spell.instanse.spellsList.Length; i++)
            {
                UI_ResearchSpell spell = Instantiate(_spellPrefab, _listLayout.transform);
                spell.id = UI_Spell.instanse.spellsList[i];
                spells.Add(spell);
            }
        }

        public void SetStatus(bool status)
        {
            if (status)
            {
                for (int i = 0; i < units.Count; i++)
                {
                    units[i].Initialize();
                }
                for (int i = 0; i < spells.Count; i++)
                {
                    spells[i].Initialize();
                }
            }
            _elements.SetActive(status);
            _active = status;
        }

        public void ResearchResponse(int response, Data.Research research)
        {
            if (isActive)
            {
                if (response == 1)
                {
                    if(research.type == Data.ResearchType.unit)
                    {
                        for (int i = 0; i < units.Count; i++)
                        {
                            if(units[i].id.ToString() == research.globalID)
                            {
                                units[i].Initialize();
                                break;
                            }
                        }
                    }
                    else if (research.type == Data.ResearchType.spell)
                    {
                        for (int i = 0; i < spells.Count; i++)
                        {
                            if (spells[i].id.ToString() == research.globalID)
                            {
                                spells[i].Initialize();
                                break;
                            }
                        }
                    }
                }
                else if (response == 2)
                {

                }
                else
                {

                }
            }
        }

        private void Close()
        {
            SetStatus(false);
            UI_Main.instanse.SetStatus(true);
        }

    }
}