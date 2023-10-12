namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UI_WarMember : MonoBehaviour
    {

        [SerializeField] private Image _image = null;
        [SerializeField] private TextMeshProUGUI _name = null;
        public GameObject selectedEffects = null;
        [SerializeField] private RectTransform _selectedCircle = null;
        [SerializeField] private GameObject bestAttack = null;
        [SerializeField] private GameObject bestAttackStar1 = null;
        [SerializeField] private GameObject bestAttackStar2 = null;
        [SerializeField] private GameObject bestAttackStar3 = null;
        [SerializeField] private RectTransform attack1 = null;
        [SerializeField] private RectTransform attack2 = null;

        [HideInInspector] public Data.ClanMember _data = null;

        [HideInInspector] public int remainedAttacks = 0;

        private void Start()
        {
            Button button = GetComponentInChildren<Button>();
            if (button)
            {
                button.onClick.AddListener(Select);
            }
        }

        public void Initialize(Data.ClanMember data, int attacksDone, Data.ClanWarAttack bestEnemyAttack, bool isEnemy)
        {
            Sprite icon = AssetsBank.GetBuildingIcon(Data.BuildingID.townhall);
            if(icon != null)
            {
                _image.sprite = icon;
            }

            float size = Mathf.RoundToInt(Screen.height * 0.2f);

            RectTransform rect = GetComponent<RectTransform>();
            rect.anchorMin = Vector2.one * 0.5f;
            rect.anchorMax = Vector2.one * 0.5f;
            rect.sizeDelta = new Vector2(size, size);
            rect.anchoredPosition = Vector2.zero;
            _selectedCircle.sizeDelta = new Vector2(size, size) * 1.25f;
            if (isEnemy)
            {
                Vector3 scale = _image.transform.localScale;
                scale.x = -scale.x;
                _image.transform.localScale = scale;
            }
           
            remainedAttacks = Data.clanWarAttacksPerPlayer - attacksDone;
            selectedEffects.SetActive(false);

            if(data != null)
            {
                _data = data;
                _name.text = (_data.warPos + 1).ToString() + ". " + Data.DecodeString(data.name);
            }

            float s = Screen.height * 0.02f;
            attack1.sizeDelta = new Vector2(s, s * 2f);
            attack2.sizeDelta = new Vector2(s, s * 2f);
            attack1.anchoredPosition = new Vector2(0, 0);
            attack2.anchoredPosition = new Vector2(-s, 0);

            attack1.gameObject.SetActive(remainedAttacks > 0);
            attack2.gameObject.SetActive(remainedAttacks > 1);

            if (bestEnemyAttack != null)
            {
                bestAttackStar1.SetActive(bestEnemyAttack.stars > 0);
                bestAttackStar2.SetActive(bestEnemyAttack.stars > 1);
                bestAttackStar3.SetActive(bestEnemyAttack.stars > 2);
                bestAttack.SetActive(true);
            }
            else
            {
                bestAttack.SetActive(false);
            }
        }

        private void Select()
        {
            UI_Clan.instanse.SelectWarMember(this);
        }

    }
}