using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Name", menuName = "Enemy", order = 51)]
public class Enemy_Setup : ScriptableObject
{
    private const int MAX_OBSTACLE_VALUE = 16;
    private const int MAX_COIN_VALUE = 32;

    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Difficulty _difficulty;
    [SerializeField] private Mode[] _modes;
    [SerializeField] private bool _randomObstacles;
    [SerializeField] private bool _randomRangeObstacles;
    [SerializeField] private int _minRangeObstacles;
    [SerializeField] private int _maxRangeObstacles;
    [SerializeField] private int[] _desiredObstaclesIndexes = new int[MAX_OBSTACLE_VALUE];
    [SerializeField] private bool _randomCoins;
    [SerializeField] private bool _randomRangeCoins;
    [SerializeField] private int _minRangeCoins;
    [SerializeField] private int _maxRangeCoins;
    [SerializeField] private int[] _desiredCoinsIndexes = new int[MAX_COIN_VALUE];
    [SerializeField] private bool _randomBaseRotationSpeed;
    [SerializeField] [Range(0, 10)] private int _minRangeBaseRotationSpeed = 0;
    [SerializeField] [Range(0, 10)] private int _baseRotationSpeed = 3;
    [SerializeField] [Range(0, 10)] private int _minSpeed = 0;
    [SerializeField] [Range(0, 10)] private int _maxSpeed = 0;
    [SerializeField] [Range(0, 10)] private int _minTimeToSwitch = 0;
    [SerializeField] [Range(0, 10)] private int _maxTimeToSwitch = 0;
    [SerializeField] private bool _randomRangeHealth;
    [SerializeField] private int _minRangeHealth;
    [SerializeField] private int _maxRangeHealth;
    [SerializeField] private float _health = 10f; // Прописать слайдер для фикс значений после тестов
    [SerializeField] [HideInInspector] private float _takeDamage;

    [Space]
    [Header("FOR DEBUGING")]
    [SerializeField] private bool _oneShotOneKill;
    [SerializeField] private bool _reset;
    [SerializeField] private bool _generateOnlyObjects;
    [SerializeField] private bool _appleEdit;
    [SerializeField] private bool _saveObjects;

    public Difficulty Difficulty1 { get => _difficulty; set => _difficulty = value; }
    public Mode[] Modes { get => _modes; set => _modes = value; }
    public float BaseRotateSpeed { get => _baseRotationSpeed; set => _baseRotationSpeed = (int)value; }
    public float Health { get => _health; set => _health = value; }
    public float TakeDamage { get => _takeDamage = 1f / _health; } // 1 (100% от прогресс бара) / на здоровье = количество ударов до уничтожения 
    public int MinTimeToSwitch { get => _minTimeToSwitch; set => _minTimeToSwitch = value; }
    public int MaxTimeToSwitch { get => _maxTimeToSwitch; set => _maxTimeToSwitch = value; }
    public Sprite Sprite { get => _sprite; set => _sprite = value; }
    public int[] DesiredObstacleIndex { get => _desiredObstaclesIndexes; set => _desiredObstaclesIndexes = value; }
    public int[] DesiredCoinsIndex { get => _desiredCoinsIndexes; set => _desiredCoinsIndexes = value; }
    public bool RandomObstacles { get => _randomObstacles; set => _randomObstacles = value; }
    public bool RandomCoins { get => _randomCoins; set => _randomCoins = value; }
    public int MinSpeed { get => _minSpeed; set => _minSpeed = value; }
    public int MaxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    public bool AppleEdit { get => _appleEdit; set => _appleEdit = value; }
    public bool Reset { get => _reset; set => _reset = value; }
    public bool GenerateObjects { get => _generateOnlyObjects; set => _generateOnlyObjects = value; }
    public bool SaveObjects { get => _saveObjects; set => _saveObjects = value; }
    public bool OneShotOneKill { get => _oneShotOneKill; set => _oneShotOneKill = value; }
    public bool RandomRangeObstacles { get => _randomRangeObstacles; set => _randomRangeObstacles = value; }
    public bool RandomRangeCoins { get => _randomRangeCoins; set => _randomRangeCoins = value; }
    public int MinRangeObstacles { get => _minRangeObstacles; set => _minRangeObstacles = value; }
    public int MinRangeCoins { get => _minRangeCoins; set => _minRangeCoins = value; }
    public int MinRangeBaseRotationSpeed { get => _minRangeBaseRotationSpeed; set => _minRangeBaseRotationSpeed = value; }
    public bool RandomBaseRotationSpeed { get => _randomBaseRotationSpeed; set => _randomBaseRotationSpeed = value; }
    public bool RandomRangeHealth { get => _randomRangeHealth; set => _randomRangeHealth = value; }
    public int MinRangeHealth { get => _minRangeHealth; set => _minRangeHealth = value; }
    public Sprite[] Sprites { get => _sprites; set => _sprites = value; }
    public int MaxRangeObstacles { get => _maxRangeObstacles; set => _maxRangeObstacles = value; }
    public int MaxRangeCoins { get => _maxRangeCoins; set => _maxRangeCoins = value; }
    public int MaxRangeHealth { get => _maxRangeHealth; set => _maxRangeHealth = value; }

    public enum Mode
    {
        Default, // EN1
        SpeedChanging, // Меняет скорость // EN2
        RotateChanging, // Меняет вращение // EN3
        Breaker, // Резкий тормоз // EN4
        Universal // Сочитает всё
    }
    public enum Difficulty
    {
        Default,
        Eazy,
        Medium,
        Hard,
        Boss
    }
}
