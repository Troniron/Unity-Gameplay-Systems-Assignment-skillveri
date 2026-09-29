using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace skillveri_Assignment
{
    public class DynamicTable : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField rowInput;
        [SerializeField] private TMP_InputField columnInput;
        [SerializeField] private Button generateButton;
        [SerializeField] private Toggle headerToggle;

        [Header("Prefab")]
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform spawnPoint;

        [Header("Grid Settings")]
        [SerializeField] private float spacingX = 4f;
        [SerializeField] private float spacingY = 4f;

        [Header("Pool Settings")]
        [SerializeField] private int initialPoolSize = 20;
        [SerializeField] private GameObject FewSecondObj;
        private readonly Queue<GameObject> objectPool = new();
        private readonly List<GameObject> activeObjects = new();

        private void Awake()
        {
            PrewarmPool();
        }
        void TurnOfLater()
        {
            FewSecondObj?.SetActive(false);
        }

        private void OnEnable()
        {
            generateButton.onClick.AddListener(GenerateGrid);
            FewSecondObj.SetActive(true);
            Invoke(nameof(TurnOfLater), 1f);
        }

        private void OnDisable()
        {
            generateButton.onClick.RemoveListener(GenerateGrid);
            TurnOfLater();
        }

        private void Start()
        {
            //  GenerateGrid();
        }

        private void PrewarmPool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = Instantiate(cellPrefab, spawnPoint);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }

        private GameObject GetObjectFromPool()
        {
            if (objectPool.Count > 0)
            {
                return objectPool.Dequeue();
            }

            return Instantiate(cellPrefab, spawnPoint);
        }

        private void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(spawnPoint, false);
            objectPool.Enqueue(obj);
        }

        private void ReturnAllToPool()
        {
            foreach (GameObject obj in activeObjects)
            {
                ReturnToPool(obj);
            }

            activeObjects.Clear();
        }

        public void GenerateGrid()
        {
            ReturnAllToPool();

            int rows = GetInputValue(rowInput, 5);
            int columns = GetInputValue(columnInput, 4);

            bool hasHeader = headerToggle != null && headerToggle.isOn;
            int totalRows = rows + (hasHeader ? 1 : 0);

            SpawnGrid(totalRows, columns);
        }

        private int GetInputValue(TMP_InputField input, int defaultValue)
        {
            if (input != null && int.TryParse(input.text, out int value))
            {
                return Mathf.Clamp(value, 1, 100);
            }

            return defaultValue;
        }

        private void SpawnGrid(int rows, int columns)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    GameObject obj = GetObjectFromPool();

                    obj.transform.SetParent(spawnPoint, false);

                    obj.transform.localPosition = new Vector3(
                        column * spacingX,
                        -row * spacingY,
                        0f
                    );

                    obj.transform.localRotation = Quaternion.identity;
                    obj.SetActive(true);

                    activeObjects.Add(obj);
                }
            }
        }
    }
}