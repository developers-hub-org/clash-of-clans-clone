namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class UI_SelectedBuilding : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        [SerializeField] public RectTransform _buildingName = null;
        [SerializeField] public TextMeshProUGUI _buildingNameText = null;

        [SerializeField] private float buildingNameHeight = 0.06f; 
        [SerializeField] private float buildingNameAspect = 6f;
        private Vector2 buildingNameSize = Vector2.one;

        private static UI_SelectedBuilding _instance = null; public static UI_SelectedBuilding instance { get { return _instance; } }
        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _buildingName.anchorMin = Vector3.zero;
            _buildingName.anchorMax = Vector3.zero;
            buildingNameSize = new Vector2(Screen.height * buildingNameHeight * buildingNameAspect, Screen.height * buildingNameHeight);
            _buildingName.sizeDelta = buildingNameSize * CameraController.instanse.zoomScale;
        }

        private void Update()
        {
            if (Building.selectedInstanse != null)
            {
                _buildingName.sizeDelta = buildingNameSize / CameraController.instanse.zoomScale;

                Vector3 end = UI_Main.instanse._grid.GetEndPosition(Building.selectedInstanse);

                Vector3 planDownLeft = CameraController.instanse.planDownLeft;
                Vector3 planTopRight = CameraController.instanse.planTopRight;

                float w = planTopRight.x - planDownLeft.x;
                float h = planTopRight.y - planDownLeft.y;

                float endW = end.x - planDownLeft.x;
                float endH = end.y - planDownLeft.y;

                Vector2 screenPoint = new Vector2(endW / w * Screen.width, endH / h * Screen.height);

                Vector2 data = screenPoint;
                data.y += (_buildingName.rect.height / 2f);
                _buildingName.anchoredPosition = data;
            }

        }

    }
}