namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_PlayerRank : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _nameText = null;
        [SerializeField] private TextMeshProUGUI _trophiesText = null;
        [SerializeField] private TextMeshProUGUI _rankText = null;
        [SerializeField] private TextMeshProUGUI _levelText = null;

        private Data.PlayerRank _clan = null;

        public void Initialize(Data.PlayerRank player)
        {
            _clan = player;
            _levelText.text = player.level.ToString();
            _trophiesText.text = player.trophies.ToString();
            _rankText.text = player.rank.ToString();
            _nameText.text = Data.DecodeString(player.name);
            _levelText.ForceMeshUpdate(true);
            _trophiesText.ForceMeshUpdate(true);
            _nameText.ForceMeshUpdate(true);
            _rankText.ForceMeshUpdate(true);
        }

    }
}