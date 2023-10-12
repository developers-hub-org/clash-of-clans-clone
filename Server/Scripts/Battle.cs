using AStarPathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevelopersHub.RealtimeNetworking.Server
{
    public class Battle
    {

        public long id = 0;
        public DateTime baseTime = DateTime.Now;
        public int frameCount = 0;
        public long defender = 0;
        public long attacker = 0;
        public List<Building> _buildings = new List<Building>();
        public List<Unit> _units = new List<Unit>();
        public List<Spell> _spells = new List<Spell>();
        public List<UnitToAdd> _unitsToAdd = new List<UnitToAdd>();
        public List<SpellToAdd> _spellsToAdd = new List<SpellToAdd>();
        private Grid grid = null;
        private Grid unlimitedGrid = null;
        private AStarSearch search = null;
        private AStarSearch unlimitedSearch = null;
        private List<Tile> blockedTiles = new List<Tile>();
        public List<Projectile> projectiles = new List<Projectile>();
        public double percentage = 0;
        public bool end = false;
        public bool surrender = false;
        public int surrenderFrame = 0;
        public float duration = 0;

        public int unitsDeployed = 0;
        public bool townhallDestroyed = false;
        public bool fiftyPercentDestroyed = false;
        public bool completelyDestroyed = false;

        public int winTrophies = 0;
        public int loseTrophies = 0;
        private int projectileCount = 0;

        public (int, int, int, int, int, int) GetlootedResources()
        {
            int totalGold = 0;
            int totalElixir = 0;
            int totalDark = 0;
            int lootedGold = 0;
            int lootedElixir = 0;
            int lootedDark = 0;
            for (int i = 0; i < _buildings.Count; i++)
            {
                switch (_buildings[i].building.id)
                {
                    case Data.BuildingID.townhall:
                        totalGold += _buildings[i].lootGoldStorage;
                        lootedGold += _buildings[i].lootedGold;
                        totalElixir += _buildings[i].lootElixirStorage;
                        lootedElixir += _buildings[i].lootedElixir;
                        totalDark += _buildings[i].lootDarkStorage;
                        lootedDark += _buildings[i].lootedDark;
                        break;
                    case Data.BuildingID.goldmine:
                    case Data.BuildingID.goldstorage:
                        totalGold += _buildings[i].lootGoldStorage;
                        lootedGold += _buildings[i].lootedGold;
                        break;
                    case Data.BuildingID.elixirmine:
                    case Data.BuildingID.elixirstorage:
                        totalElixir += _buildings[i].lootElixirStorage;
                        lootedElixir += _buildings[i].lootedElixir;
                        break;
                    case Data.BuildingID.darkelixirmine:
                    case Data.BuildingID.darkelixirstorage:
                        totalDark += _buildings[i].lootDarkStorage;
                        lootedDark += _buildings[i].lootedDark;
                        break;
                    case Data.BuildingID.clancastle:

                        break;
                }
            }
            return (lootedGold, lootedElixir, lootedDark, totalGold, totalElixir, totalDark);
        }

        public int stars { get { int s = 0; if (townhallDestroyed) { s++; } if (fiftyPercentDestroyed) { s++; } if (completelyDestroyed) { s++; } return s; } }

        public delegate void SpellSpawned(long databaseID, Data.SpellID id, BattleVector2 target, float radius);
        public delegate void Spawned(long id);
        public delegate void AttackCallback(long index, long target);
        public delegate void IndexCallback(long index);
        public delegate void FloatCallback(long index, float value);
        public delegate void DoubleCallback(long index, double value);
        public delegate void BlankCallback();
        public delegate void ProjectileCallback(int id, BattleVector2 current, BattleVector2 target);
        public ProjectileCallback projectileCallback = null;

        public int GetTrophies()
        {
            int s = stars;
            if (s > 0)
            {
                if (s >= 3)
                {
                    return winTrophies;
                }
                else
                {
                    int t = (int)Math.Floor((double)winTrophies / (double)s);
                    return t * s;
                }
            }
            else
            {
                return loseTrophies * -1;
            }
        }

        public class Projectile
        {
            public int id = 0;
            public int target = -1;
            public float damage = 0;
            public float splash = 0;
            public float timer = 0;
            public TargetType type = TargetType.unit;
            public bool heal = false;
            public bool follow = true;
            public BattleVector2 position = new BattleVector2();
        }

        public enum TargetType
        {
            unit, building
        }

        public class Tile
        {
            public Tile(Data.BuildingID id, BattleVector2Int position, int index = -1)
            {
                this.id = id;
                this.position = position;
                this.index = index;
            }
            public Data.BuildingID id;
            public BattleVector2Int position;
            public int index = -1;
        }

        public class Spell
        {
            public Data.Spell spell = null;
            public IndexCallback pulseCallback = null;
            public IndexCallback doneCallback = null;
            public BattleVector2 position;
            public bool done = false;
            public int palsesDone = 0;
            public double palsesTimer = 0;
            public BattleVector2 positionOnGrid { get { return new BattleVector2(position.x - Data.battleGridOffset, position.y - Data.battleGridOffset); } }
            public void Initialize(int x, int y)
            {
                if (spell == null) { return; }
                position = GridToWorldPosition(new BattleVector2Int(x, y));
            }
        }

        public class Unit
        {
            public Data.Unit unit = null;
            public float health = 0;
            public int target = -1;
            public int mainTarget = -1;
            public BattleVector2 position;
            public BattleVector2 positionOnGrid { get { return new BattleVector2(position.x - Data.battleGridOffset, position.y - Data.battleGridOffset); } }
            public Path path = null;
            public double pathTime = 0;
            public double pathTraveledTime = 0;
            public double attackTimer = 0;
            public bool moving = false;
            public Dictionary<int, float> resourceTargets = new Dictionary<int, float>();
            public Dictionary<int, float> defenceTargets = new Dictionary<int, float>();
            public Dictionary<int, float> otherTargets = new Dictionary<int, float>();
            public AttackCallback attackCallback = null;
            public IndexCallback dieCallback = null;
            public FloatCallback damageCallback = null;
            public FloatCallback healCallback = null;
            public IndexCallback targetCallback = null;
            public Dictionary<int, float> GetAllTargets()
            {
                Dictionary<int, float> temp = new Dictionary<int, float>();
                if (otherTargets.Count > 0)
                {
                    temp = temp.Concat(otherTargets).ToDictionary(x => x.Key, x => x.Value);
                }
                if (resourceTargets.Count > 0)
                {
                    temp = temp.Concat(resourceTargets).ToDictionary(x => x.Key, x => x.Value);
                }
                if (defenceTargets.Count > 0)
                {
                    temp = temp.Concat(defenceTargets).ToDictionary(x => x.Key, x => x.Value);
                }
                return temp;
            }
            public void AssignTarget(int target, Path path)
            {
                attackTimer = unit.attackSpeed;
                this.target = target;
                this.path = path;
                if (path != null)
                {
                    pathTraveledTime = 0;
                    pathTime = path.length / (unit.moveSpeed * Data.gridCellSize);
                }
                if (targetCallback != null)
                {
                    targetCallback.Invoke(unit.databaseID);
                }
            }
            public void AssignHealerTarget(int target, float distance)
            {
                attackTimer = unit.attackSpeed;
                this.target = target;
                pathTraveledTime = 0;
                pathTime = distance / (unit.moveSpeed * Data.gridCellSize);
            }
            public void TakeDamage(float damage)
            {
                if (health <= 0) { return; }
                health -= damage;
                if (damageCallback != null)
                {
                    damageCallback.Invoke(unit.databaseID, damage);
                }
                if (health < 0) { health = 0; }
                if (health <= 0)
                {
                    if (dieCallback != null)
                    {
                        dieCallback.Invoke(unit.databaseID);
                    }
                }
            }
            public void Heal(float amount)
            {
                if (amount <= 0 || health <= 0) { return; }
                health += amount;
                if (health > unit.health) { health = unit.health; }
                if (healCallback != null)
                {
                    healCallback.Invoke(unit.databaseID, amount);
                }
            }
            public void Initialize(int x, int y)
            {
                if (unit == null) { return; }
                position = GridToWorldPosition(new BattleVector2Int(x, y));
            }
        }

        public class Building
        {
            public Data.Building building = null;
            public float health = 0;
            public int target = -1;
            public double attackTimer = 0;
            public double percentage = 0;
            public BattleVector2 worldCenterPosition;
            public AttackCallback attackCallback = null;
            public DoubleCallback destroyCallback = null;
            public FloatCallback damageCallback = null;
            public BlankCallback starCallback = null;

            public int lootGoldStorage = 0;
            public int lootElixirStorage = 0;
            public int lootDarkStorage = 0;

            public int lootedGold = 0;
            public int lootedElixir = 0;
            public int lootedDark = 0;

            public void TakeDamage(float damage, ref Grid grid, ref List<Tile> blockedTiles, ref double percentage, ref bool fiftySatar, ref bool hallStar, ref bool allStar)
            {
                if (health <= 0) { return; }
                health -= damage;
                if (damageCallback != null)
                {
                    damageCallback.Invoke(building.databaseID, damage);
                }
                if (health < 0) { health = 0; }

                double loot = 1d - ((double)health / (double)building.health);
                if (lootGoldStorage > 0) { lootedGold = (int)Math.Floor(lootGoldStorage * loot); }
                if (lootElixirStorage > 0) { lootedElixir = (int)Math.Floor(lootElixirStorage * loot); }
                if (lootDarkStorage > 0) { lootedDark = (int)Math.Floor(lootDarkStorage * loot); }

                if (health <= 0)
                {
                    for (int x = building.x; x < building.x + building.columns; x++)
                    {
                        for (int y = building.y; y < building.y + building.rows; y++)
                        {
                            grid[x, y].Blocked = false;
                            for (int i = 0; i < blockedTiles.Count; i++)
                            {
                                if (blockedTiles[i].position.x == x && blockedTiles[i].position.y == y)
                                {
                                    blockedTiles.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }
                    if (this.percentage > 0)
                    {
                        percentage += this.percentage;
                    }
                    if (destroyCallback != null)
                    {
                        destroyCallback.Invoke(building.databaseID, this.percentage);
                    }
                    if (building.id == Data.BuildingID.townhall && !hallStar)
                    {
                        hallStar = true;
                        if (starCallback != null)
                        {
                            starCallback.Invoke();
                        }
                    }
                    int p = (int)Math.Floor(percentage * 100d);
                    if (p >= 50 && !fiftySatar)
                    {
                        fiftySatar = true;
                        if (starCallback != null)
                        {
                            starCallback.Invoke();
                        }
                    }
                    if (p >= 100 && !allStar)
                    {
                        allStar = true;
                        if (starCallback != null)
                        {
                            starCallback.Invoke();
                        }
                    }
                }
            }
            public void Initialize()
            {
                health = building.health;
                percentage = building.percentage;
                lootedGold = 0;
                lootedElixir = 0;
                lootedDark = 0;
            }
        }

        public class UnitToAdd
        {
            public Unit unit = null;
            public int x;
            public int y;
            public Spawned callback = null;
            public AttackCallback attackCallback = null;
            public IndexCallback dieCallback = null;
            public FloatCallback damageCallback = null;
            public FloatCallback healCallback = null;
            public IndexCallback targetCallback = null;
        }

        public class SpellToAdd
        {
            public Spell spell = null;
            public int x;
            public int y;
            public SpellSpawned callback = null;
        }

        public void Initialize(List<Building> buildings, DateTime time, AttackCallback attackCallback = null, DoubleCallback destroyCallback = null, FloatCallback damageCallback = null, BlankCallback starGained = null, ProjectileCallback projectileCallback = null)
        {
            baseTime = time;
            duration = Data.battleDuration;
            frameCount = 0;
            percentage = 0;
            unitsDeployed = 0;
            fiftyPercentDestroyed = false;
            townhallDestroyed = false;
            completelyDestroyed = false;
            end = false;
            projectileCount = 0;
            surrender = false;
            this.projectileCallback = projectileCallback;
            _buildings = buildings;
            grid = new Grid(Data.gridSize + (Data.battleGridOffset * 2), Data.gridSize + (Data.battleGridOffset * 2));
            unlimitedGrid = new Grid(Data.gridSize + (Data.battleGridOffset * 2), Data.gridSize + (Data.battleGridOffset * 2));
            search = new AStarSearch(grid);
            unlimitedSearch = new AStarSearch(unlimitedGrid);
            for (int i = 0; i < _buildings.Count; i++)
            {
                _buildings[i].attackCallback = attackCallback;
                _buildings[i].destroyCallback = destroyCallback;
                _buildings[i].damageCallback = damageCallback;
                _buildings[i].starCallback = starGained;

                _buildings[i].Initialize();
                _buildings[i].worldCenterPosition = new BattleVector2((_buildings[i].building.x + (_buildings[i].building.columns / 2f)) * Data.gridCellSize, (_buildings[i].building.y + (_buildings[i].building.rows / 2f)) * Data.gridCellSize);

                int startX = _buildings[i].building.x;
                int endX = _buildings[i].building.x + _buildings[i].building.columns;

                int startY = _buildings[i].building.y;
                int endY = _buildings[i].building.y + _buildings[i].building.rows;

                if (_buildings[i].building.id != Data.BuildingID.wall && _buildings[i].building.columns > 1 && _buildings[i].building.rows > 1)
                {
                    startX++;
                    startY++;
                    endX--;
                    endY--;
                    if (endX <= startX || endY <= startY)
                    {
                        continue;
                    }
                }

                for (int x = startX; x < endX; x++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        grid[x, y].Blocked = true;
                        blockedTiles.Add(new Tile(_buildings[i].building.id, new BattleVector2Int(x, y), i));
                    }
                }
            }
        }

        public bool IsAliveUnitsOnGrid()
        {
            for (int i = 0; i < _units.Count; i++)
            {
                if (_units[i].health > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanBattleGoOn()
        {
            if (Math.Abs(percentage - 1d) > 0.0001d && IsAliveUnitsOnGrid())
            {
                double time = (float)frameCount * Data.battleFrameRate;
                if (time < duration)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanAddUnit(int x, int y)
        {
            x += Data.battleGridOffset;
            y += Data.battleGridOffset;

            for (int i = 0; i < _buildings.Count; i++)
            {
                if (_buildings[i].health <= 0)
                {
                    continue;
                }

                int startX = _buildings[i].building.x;
                int endX = _buildings[i].building.x + _buildings[i].building.columns;

                int startY = _buildings[i].building.y;
                int endY = _buildings[i].building.y + _buildings[i].building.rows;

                for (int x2 = startX; x2 < endX; x2++)
                {
                    for (int y2 = startY; y2 < endY; y2++)
                    {
                        if (x == x2 && y == y2)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool CanAddSpell(int x, int y)
        {
            return true;
        }

        public void AddUnit(Data.Unit unit, int x, int y, Spawned callback = null, AttackCallback attackCallback = null, IndexCallback dieCallback = null, FloatCallback damageCallback = null, FloatCallback healCallback = null, IndexCallback targetCallback = null)
        {
            if (end)
            {
                return;
            }
            x += Data.battleGridOffset;
            y += Data.battleGridOffset;
            UnitToAdd unitToAdd = new UnitToAdd();
            unitToAdd.callback = callback;
            Unit battleUnit = new Unit();
            battleUnit.attackCallback = attackCallback;
            battleUnit.dieCallback = dieCallback;
            battleUnit.damageCallback = damageCallback;
            battleUnit.healCallback = healCallback;
            battleUnit.targetCallback = targetCallback;
            battleUnit.unit = unit;
            battleUnit.Initialize(x, y);
            battleUnit.health = unit.health;
            unitToAdd.unit = battleUnit;
            unitToAdd.x = x;
            unitToAdd.y = y;
            _unitsToAdd.Add(unitToAdd);
        }

        public void AddSpell(Data.Spell spell, int x, int y, SpellSpawned callback = null, IndexCallback pulseCallback = null, IndexCallback doneCallback = null)
        {
            if (end)
            {
                return;
            }
            x += Data.battleGridOffset;
            y += Data.battleGridOffset;
            SpellToAdd spellToAdd = new SpellToAdd();
            spellToAdd.callback = callback;
            Spell battleSpell = new Spell();
            battleSpell.doneCallback = doneCallback;
            battleSpell.pulseCallback = pulseCallback;
            battleSpell.spell = spell;
            battleSpell.Initialize(x, y);
            spellToAdd.spell = battleSpell;
            spellToAdd.x = x;
            spellToAdd.y = y;
            _spellsToAdd.Add(spellToAdd);
        }

        public void ExecuteFrame()
        {
            int addIndex = _units.Count;
            for (int i = _unitsToAdd.Count - 1; i >= 0; i--)
            {
                /*
                if (CanAddUnit(_unitsToAdd[i].x, _unitsToAdd[i].y))
                {
                    
                }*/
                unitsDeployed += _unitsToAdd[i].unit.unit.hosing;
                _unitsToAdd[i].x += Data.battleGridOffset;
                _unitsToAdd[i].y += Data.battleGridOffset;
                _units.Insert(addIndex, _unitsToAdd[i].unit);

                addIndex++;
                if (_unitsToAdd[i].callback != null)
                {
                    _unitsToAdd[i].callback.Invoke(_unitsToAdd[i].unit.unit.databaseID);
                }
                _unitsToAdd.RemoveAt(i);
            }

            addIndex = _spells.Count;
            for (int i = _spellsToAdd.Count - 1; i >= 0; i--)
            {
                _spellsToAdd[i].x += Data.battleGridOffset;
                _spellsToAdd[i].y += Data.battleGridOffset;
                _spells.Insert(addIndex, _spellsToAdd[i].spell);
                if (_spellsToAdd[i].callback != null)
                {
                    _spellsToAdd[i].callback.Invoke(_spellsToAdd[i].spell.spell.databaseID, _spellsToAdd[i].spell.spell.id, _spells[addIndex].positionOnGrid, _spellsToAdd[i].spell.spell.server.radius);
                }
                addIndex++;
                _spellsToAdd.RemoveAt(i);
            }

            for (int i = 0; i < _buildings.Count; i++)
            {
                if (_buildings[i].building.targetType != Data.BuildingTargetType.none && _buildings[i].health > 0)
                {
                    HandleBuilding(i, Data.battleFrameRate);
                }
            }

            for (int i = 0; i < _units.Count; i++)
            {
                if (_units[i].health > 0)
                {
                    HandleUnit(i, Data.battleFrameRate);
                }
            }

            for (int i = 0; i < _spells.Count; i++)
            {
                if (_spells[i].done == false)
                {
                    HandleSpell(i, Data.battleFrameRate);
                }
            }

            if (projectiles.Count > 0)
            {
                for (int i = projectiles.Count - 1; i >= 0; i--)
                {
                    projectiles[i].timer -= Data.battleFrameRate;
                    if (projectiles[i].timer <= 0)
                    {
                        if (projectiles[i].type == TargetType.unit)
                        {
                            if (projectiles[i].heal)
                            {
                                _units[projectiles[i].target].Heal(projectiles[i].damage);
                                for (int j = 0; j < _units.Count; j++)
                                {
                                    if (_units[j].health <= 0 || j == projectiles[i].target || _units[j].unit.movement == Data.UnitMoveType.fly)
                                    {
                                        continue;
                                    }
                                    float distance = BattleVector2.Distance(_units[j].position, _units[projectiles[i].target].position);
                                    if (distance < projectiles[i].splash * Data.gridCellSize)
                                    {
                                        _units[j].Heal(projectiles[i].damage * (1f - (distance / projectiles[i].splash * Data.gridCellSize)));
                                    }
                                }
                            }
                            else
                            {
                                _units[projectiles[i].target].TakeDamage(projectiles[i].damage);
                                if (projectiles[i].splash > 0)
                                {
                                    for (int j = 0; j < _units.Count; j++)
                                    {
                                        if (j != projectiles[i].target)
                                        {
                                            float distance = BattleVector2.Distance(_units[j].position, _units[projectiles[i].target].position);
                                            if (distance < projectiles[i].splash * Data.gridCellSize)
                                            {
                                                _units[j].TakeDamage(projectiles[i].damage * (1f - (distance / projectiles[i].splash * Data.gridCellSize)));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            _buildings[projectiles[i].target].TakeDamage(projectiles[i].damage, ref grid, ref blockedTiles, ref percentage, ref fiftyPercentDestroyed, ref townhallDestroyed, ref completelyDestroyed);
                        }
                        projectiles.RemoveAt(i);
                    }
                }
            }

            frameCount++;
        }

        private void HandleBuilding(int index, double deltaTime)
        {
            if (_buildings[index].target >= 0)
            {
                if (_units[_buildings[index].target].health <= 0 || !IsUnitInRange(_buildings[index].target, index) || (_units[_buildings[index].target].unit.movement == Data.UnitMoveType.underground && _units[_buildings[index].target].path != null))
                {
                    // If the building's target is dead or not in range then remove it as target
                    _buildings[index].target = -1;
                }
                else
                {
                    // Building has a target
                    bool freeze = false;
                    for (int i = 0; i < _spells.Count; i++)
                    {
                        if (_spells[i].done) { continue; }
                        if (_spells[i].spell.id == Data.SpellID.freeze)
                        {
                            double p = GetBuildingInSpellRangePercentage(i, index);
                            if (p > 0)
                            {
                                freeze = true;
                                break;
                            }
                        }
                    }
                    if (!freeze)
                    {
                        if (IsUnitCanBeSeen(_buildings[index].target, index))
                        {
                            _buildings[index].attackTimer += deltaTime;
                            int attacksCount = (int)Math.Floor(_buildings[index].attackTimer / _buildings[index].building.speed);
                            if (attacksCount > 0)
                            {
                                _buildings[index].attackTimer -= (attacksCount * _buildings[index].building.speed);
                                for (int i = 1; i <= attacksCount; i++)
                                {
                                    if (_buildings[index].building.radius > 0 && _buildings[index].building.rangedSpeed > 0)
                                    {
                                        float distance = BattleVector2.Distance(_units[_buildings[index].target].position, _buildings[index].worldCenterPosition);
                                        Projectile projectile = new Projectile();
                                        projectile.type = TargetType.unit;
                                        projectile.target = _buildings[index].target;
                                        projectile.timer = distance / (_buildings[index].building.rangedSpeed * Data.gridCellSize);
                                        projectile.damage = _buildings[index].building.damage;
                                        projectile.splash = _buildings[index].building.splashRange;
                                        projectile.follow = true;
                                        projectile.position = _buildings[index].worldCenterPosition;
                                        projectileCount++;
                                        projectile.id = projectileCount;
                                        projectiles.Add(projectile);
                                        if (projectileCallback != null)
                                        {
                                            projectileCallback.Invoke(projectile.id, _buildings[index].worldCenterPosition, _units[_buildings[index].target].position);
                                        }
                                    }
                                    else
                                    {
                                        _units[_buildings[index].target].TakeDamage(_buildings[index].building.damage);
                                        if (_buildings[index].building.splashRange > 0)
                                        {
                                            for (int j = 0; j < _units.Count; j++)
                                            {
                                                if (j != _buildings[index].target)
                                                {
                                                    float distance = BattleVector2.Distance(_units[j].position, _units[_buildings[index].target].position);
                                                    if (distance < _buildings[index].building.splashRange * Data.gridCellSize)
                                                    {
                                                        _units[j].TakeDamage(_buildings[index].building.damage * (1f - (distance / _buildings[index].building.splashRange * Data.gridCellSize)));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (_buildings[index].attackCallback != null)
                                    {
                                        _buildings[index].attackCallback.Invoke(_buildings[index].building.databaseID, _units[_buildings[index].target].unit.databaseID);
                                    }
                                }
                            }
                        }
                        else
                        {
                            _buildings[index].target = -1;
                        }
                    }
                }
            }
            if (_buildings[index].target < 0)
            {
                // Find a new target for this building
                if (FindTargetForBuilding(index))
                {
                    HandleBuilding(index, deltaTime);
                }
            }
        }

        private bool FindTargetForBuilding(int index)
        {
            for (int i = 0; i < _units.Count; i++)
            {
                if (_units[i].health <= 0 || _units[i].unit.movement == Data.UnitMoveType.underground && _units[i].path != null)
                {
                    continue;
                }

                if (_buildings[index].building.targetType == Data.BuildingTargetType.ground && _units[i].unit.movement == Data.UnitMoveType.fly)
                {
                    continue;
                }

                if (_buildings[index].building.targetType == Data.BuildingTargetType.air && _units[i].unit.movement != Data.UnitMoveType.fly)
                {
                    continue;
                }

                if (IsUnitInRange(i, index) && IsUnitCanBeSeen(i, index))
                {
                    _buildings[index].attackTimer = _buildings[index].building.speed;
                    _buildings[index].target = i;
                    return true;
                }
            }
            return false;
        }

        private bool IsUnitInRange(int unitIndex, int buildingIndex)
        {
            float distance = BattleVector2.Distance(_buildings[buildingIndex].worldCenterPosition, _units[unitIndex].position);
            if (distance <= (_buildings[buildingIndex].building.radius * Data.gridCellSize))
            {
                if (_buildings[buildingIndex].building.blindRange > 0 && distance <= _buildings[buildingIndex].building.blindRange)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool IsUnitCanBeSeen(int unitIndex, int buildingIndex)
        {
            for (int i = 0; i < _spells.Count; i++)
            {
                if (_spells[i].done) { continue; }
                if (_spells[i].spell.id == Data.SpellID.invisibility)
                {
                    float distance = BattleVector2.Distance(_units[unitIndex].position, _spells[i].position);
                    if (distance <= (_spells[i].spell.server.radius * Data.gridCellSize))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void HandleUnit(int index, double deltaTime)
        {
            if (_units[index].unit.id == Data.UnitID.healer)
            {
                if (_units[index].target >= 0 && (_units[_units[index].target].health <= 0 || _units[_units[index].target].health >= _units[_units[index].target].unit.health))
                {
                    _units[index].moving = false;
                    _units[index].target = -1;
                }
                if (_units[index].target < 0)
                {
                    _units[index].moving = false;
                    FindHealerTargets(index);
                }
                if (_units[index].target >= 0)
                {
                    _units[index].moving = false;
                    float distance = BattleVector2.Distance(_units[index].position, _units[_units[index].target].position);
                    if (distance + Data.gridCellSize <= _units[index].unit.attackRange)
                    {
                        _units[index].attackTimer += deltaTime;
                        if (_units[index].attackTimer >= _units[index].unit.attackSpeed)
                        {
                            _units[index].attackTimer -= _units[index].unit.attackSpeed;
                            if (_units[index].unit.attackRange > 0 && _units[index].unit.rangedSpeed > 0)
                            {
                                Projectile projectile = new Projectile();
                                projectile.type = TargetType.unit;
                                projectile.target = _units[index].target;
                                projectile.timer = distance / (_units[index].unit.rangedSpeed * Data.gridCellSize);
                                projectile.damage = GetUnitDamage(index);
                                projectile.follow = true;
                                projectile.position = _units[index].position;
                                projectile.heal = true;
                                projectileCount++;
                                projectile.id = projectileCount;
                                projectiles.Add(projectile);
                                if (projectileCallback != null)
                                {
                                    projectileCallback.Invoke(projectile.id, _units[index].position, _units[_units[index].target].position);
                                }
                            }
                            else
                            {
                                float baseHeal = GetUnitDamage(index);
                                _units[_units[index].target].Heal(baseHeal);
                                for (int i = 0; i < _units.Count; i++)
                                {
                                    if (_units[i].health <= 0 || i == index || i == _units[index].target)
                                    {
                                        continue;
                                    }
                                    float d = BattleVector2.Distance(_units[i].position, _units[_units[index].target].position);
                                    if (d < _units[i].unit.splashRange * Data.gridCellSize)
                                    {
                                        float amount = baseHeal * (1f - (d / _units[i].unit.splashRange * Data.gridCellSize));
                                        _units[i].Heal(amount);
                                    }
                                }
                            }
                            if (_units[index].attackCallback != null)
                            {
                                _units[index].attackCallback.Invoke(_units[index].unit.databaseID, 0);
                            }
                        }
                    }
                    else
                    {
                        // Move the healer
                        _units[index].moving = true;
                        float d = (float)deltaTime * GetUnitMoveSpeed(index) * Data.gridCellSize;
                        _units[index].position = BattleVector2.LerpUnclamped(_units[index].position, _units[_units[index].target].position, d / distance);
                        return;
                    }
                }
            }
            else
            {
                if (_units[index].path != null)
                {
                    if (_units[index].target < 0 || (_units[index].target >= 0 && _buildings[_units[index].target].health <= 0))
                    {
                        _units[index].moving = false;
                        _units[index].path = null;
                        _units[index].target = -1;
                    }
                    else
                    {
                        _units[index].moving = true;
                        /*
                        if(_units[index].unit.movement == Data.UnitMoveType.ground)
                        {
                            bool inJumpRange = false;
                            for (int i = 0; i < _spells.Count; i++)
                            {
                                if (_spells[i].done) { continue; }
                                if (_spells[i].spell.id == Data.SpellID.jump)
                                {
                                    float distance = BattleVector2.Distance(_units[index].position, _spells[i].position);
                                    if (distance <= (_spells[i].spell.server.radius * Data.gridCellSize))
                                    {
                                        inJumpRange = true;
                                        break;
                                    }
                                }
                            }
                            if (inJumpRange)
                            {

                            }
                        }
                        */

                        double remainedTime = _units[index].pathTime - _units[index].pathTraveledTime;
                        if (remainedTime >= deltaTime)
                        {
                            double moveExtra = 1;
                            double s = GetUnitMoveSpeed(index);
                            if (s != _units[index].unit.moveSpeed)
                            {
                                moveExtra = s / _units[index].unit.moveSpeed;
                            }
                            _units[index].pathTraveledTime += (deltaTime * moveExtra);
                            if (_units[index].pathTraveledTime > _units[index].pathTime)
                            {
                                _units[index].pathTraveledTime = _units[index].pathTime;
                            }
                            if (_units[index].pathTraveledTime < 0)
                            {
                                _units[index].pathTraveledTime = 0;
                            }
                            deltaTime = 0;
                        }
                        else
                        {
                            _units[index].pathTraveledTime = _units[index].pathTime;
                            deltaTime -= remainedTime;
                        }

                        // Update unit's position based on path
                        _units[index].position = GetPathPosition(_units[index].path.points, (float)(_units[index].pathTraveledTime / _units[index].pathTime));

                        // Check if target is in range
                        if (_units[index].unit.attackRange > 0 && IsBuildingInRange(index, _units[index].target))
                        {
                            _units[index].path = null;
                        }
                        else
                        {
                            // check if unit reached the end of the path
                            BattleVector2 targetPosition = GridToWorldPosition(new BattleVector2Int(_units[index].path.points.Last().Location.X, _units[index].path.points.Last().Location.Y));
                            float distance = BattleVector2.Distance(_units[index].position, targetPosition);
                            if (distance <= Data.gridCellSize * 0.05f)
                            {
                                _units[index].position = targetPosition;
                                _units[index].path = null;
                                _units[index].moving = false;
                            }
                        }
                    }
                }

                if (_units[index].target >= 0)
                {
                    if (_buildings[_units[index].target].health > 0)
                    {
                        if (_buildings[_units[index].target].building.id == Data.BuildingID.wall && _units[index].mainTarget >= 0 && _buildings[_units[index].mainTarget].health <= 0)
                        {
                            _units[index].moving = false;
                            _units[index].target = -1;
                        }
                        else
                        {
                            if (_units[index].path == null)
                            {
                                // Attack the target
                                _units[index].moving = false;
                                _units[index].attackTimer += deltaTime;
                                if (_units[index].attackTimer >= _units[index].unit.attackSpeed)
                                {
                                    float multiplier = 1;
                                    if (_units[index].unit.priority != Data.TargetPriority.all || _units[index].unit.priority != Data.TargetPriority.none)
                                    {
                                        switch (_buildings[_units[index].target].building.id)
                                        {
                                            case Data.BuildingID.townhall:
                                            case Data.BuildingID.goldmine:
                                            case Data.BuildingID.goldstorage:
                                            case Data.BuildingID.elixirmine:
                                            case Data.BuildingID.elixirstorage:
                                            case Data.BuildingID.darkelixirmine:
                                            case Data.BuildingID.darkelixirstorage:
                                                if (_units[index].unit.priority == Data.TargetPriority.resources)
                                                {
                                                    multiplier = _units[index].unit.priorityMultiplier;
                                                }
                                                break;
                                            case Data.BuildingID.wall:
                                                if (_units[index].unit.priority == Data.TargetPriority.walls)
                                                {
                                                    multiplier = _units[index].unit.priorityMultiplier;
                                                }
                                                break;
                                            case Data.BuildingID.cannon:
                                            case Data.BuildingID.archertower:
                                            case Data.BuildingID.mortor:
                                            case Data.BuildingID.airdefense:
                                            case Data.BuildingID.wizardtower:
                                            case Data.BuildingID.hiddentesla:
                                            case Data.BuildingID.bombtower:
                                            case Data.BuildingID.xbow:
                                            case Data.BuildingID.infernotower:
                                                if (_units[index].unit.priority == Data.TargetPriority.defenses)
                                                {
                                                    multiplier = _units[index].unit.priorityMultiplier;
                                                }
                                                break;
                                        }
                                    }

                                    float distance = BattleVector2.Distance(_units[index].position, _buildings[_units[index].target].worldCenterPosition);
                                    if (_units[index].unit.attackRange > 0 && _units[index].unit.rangedSpeed > 0)
                                    {
                                        Projectile projectile = new Projectile();
                                        projectile.type = TargetType.building;
                                        projectile.target = _units[index].target;
                                        projectile.timer = distance / (_units[index].unit.rangedSpeed * Data.gridCellSize);
                                        projectile.damage = GetUnitDamage(index) * multiplier;
                                        projectile.follow = true;
                                        projectile.position = _units[index].position;
                                        projectileCount++;
                                        projectile.id = projectileCount;
                                        projectiles.Add(projectile);
                                        if (projectileCallback != null)
                                        {
                                            projectileCallback.Invoke(projectile.id, _units[index].position, _buildings[_units[index].target].worldCenterPosition);
                                        }
                                    }
                                    else
                                    {
                                        _buildings[_units[index].target].TakeDamage(GetUnitDamage(index) * multiplier, ref grid, ref blockedTiles, ref percentage, ref fiftyPercentDestroyed, ref townhallDestroyed, ref completelyDestroyed);
                                    }
                                    _units[index].attackTimer -= _units[index].unit.attackSpeed;
                                    if (_units[index].attackCallback != null)
                                    {
                                        _units[index].attackCallback.Invoke(_units[index].unit.databaseID, _buildings[_units[index].target].building.databaseID);
                                    }
                                    if (_units[index].unit.id == Data.UnitID.wallbreaker)
                                    {
                                        _units[index].TakeDamage(_units[index].health);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _units[index].moving = false;
                        _units[index].target = -1;
                    }
                }

                if (_units[index].target < 0)
                {
                    // Find a target and path
                    _units[index].moving = false;
                    FindTargets(index, _units[index].unit.priority);
                    if (deltaTime > 0 && _units[index].target >= 0)
                    {
                        HandleUnit(index, deltaTime);
                    }
                }
            }
        }

        private void FindHealerTargets(int index)
        {
            int target = -1;
            float distance = 99999;
            // TODO: Larger mass of units is priority
            for (int i = 0; i < _units.Count; i++)
            {
                if (_units[i].health <= 0 || i == index || _units[i].health >= _units[i].unit.health || _units[i].unit.movement == Data.UnitMoveType.fly)
                {
                    continue;
                }
                float d = BattleVector2.Distance(_units[i].position, _units[index].position);
                if (d < distance)
                {
                    target = i;
                    distance = d;
                }
            }
            if (target >= 0)
            {
                _units[index].AssignHealerTarget(target, distance + Data.gridCellSize);
            }
        }

        private void ListUnitTargets(int index, Data.TargetPriority priority)
        {
            _units[index].resourceTargets.Clear();
            _units[index].defenceTargets.Clear();
            _units[index].otherTargets.Clear();
            if (priority == Data.TargetPriority.walls)
            {
                priority = Data.TargetPriority.all;
            }
            for (int i = 0; i < _buildings.Count; i++)
            {
                if (_buildings[i].health <= 0 || _buildings[i].building.id == Data.BuildingID.wall || !IsBuildingCanBeAttacked(_buildings[i].building.id))
                {
                    continue;
                }
                float distance = BattleVector2.Distance(_buildings[i].worldCenterPosition, _units[index].position);
                switch (_buildings[i].building.id)
                {
                    case Data.BuildingID.townhall:
                    case Data.BuildingID.elixirmine:
                    case Data.BuildingID.elixirstorage:
                    case Data.BuildingID.darkelixirmine:
                    case Data.BuildingID.darkelixirstorage:
                    case Data.BuildingID.goldmine:
                    case Data.BuildingID.goldstorage:
                        _units[index].resourceTargets.Add(i, distance);
                        break;
                    case Data.BuildingID.cannon:
                    case Data.BuildingID.archertower:
                    case Data.BuildingID.mortor:
                    case Data.BuildingID.airdefense:
                    case Data.BuildingID.wizardtower:
                    case Data.BuildingID.hiddentesla:
                    case Data.BuildingID.bombtower:
                    case Data.BuildingID.xbow:
                    case Data.BuildingID.infernotower:
                        _units[index].defenceTargets.Add(i, distance);
                        break;
                    case Data.BuildingID.wall:
                        // Don't include
                        break;
                    default:
                        _units[index].otherTargets.Add(i, distance);
                        break;
                }
            }
        }

        private void FindTargets(int index, Data.TargetPriority priority)
        {
            ListUnitTargets(index, priority);
            if (priority == Data.TargetPriority.defenses)
            {
                if (_units[index].defenceTargets.Count > 0)
                {
                    AssignTarget(index, ref _units[index].defenceTargets);
                }
                else
                {
                    FindTargets(index, Data.TargetPriority.all);
                    return;
                }
            }
            else if (priority == Data.TargetPriority.resources)
            {
                if (_units[index].resourceTargets.Count > 0)
                {
                    AssignTarget(index, ref _units[index].resourceTargets);
                }
                else
                {
                    FindTargets(index, Data.TargetPriority.all);
                    return;
                }
            }
            else if (priority == Data.TargetPriority.all || priority == Data.TargetPriority.walls)
            {
                Dictionary<int, float> temp = _units[index].GetAllTargets();
                if (temp.Count > 0)
                {
                    AssignTarget(index, ref temp, priority == Data.TargetPriority.walls);
                }
            }
        }

        private void AssignTarget(int index, ref Dictionary<int, float> targets, bool wallsPriority = false)
        {
            if (wallsPriority)
            {
                var wallPath = GetPathToWall(index, ref targets);
                if (wallPath.Item1 >= 0)
                {
                    _units[index].AssignTarget(wallPath.Item1, wallPath.Item2);
                    return;
                }
            }

            int min = targets.Aggregate((a, b) => a.Value < b.Value ? a : b).Key;
            var path = GetPathToBuilding(min, index);
            if (path.Item1 >= 0)
            {
                _units[index].AssignTarget(path.Item1, path.Item2);
            }
        }

        private (int, Path) GetPathToWall(int unitIndex, ref Dictionary<int, float> targets)
        {
            BattleVector2Int unitGridPosition = WorldToGridPosition(_units[unitIndex].position);
            List<Path> tiles = new List<Path>();
            foreach (var target in (targets.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value)))
            {
                List<Cell> points = search.Find(new Vector2Int(_buildings[target.Key].building.x, _buildings[target.Key].building.y), new Vector2Int(unitGridPosition.x, unitGridPosition.y)).ToList();
                if (Path.IsValid(ref points, new Vector2Int(_buildings[target.Key].building.x, _buildings[target.Key].building.y), new Vector2Int(unitGridPosition.x, unitGridPosition.y)))
                {
                    continue;
                }
                else
                {
                    for (int i = 0; i < _units.Count; i++)
                    {
                        if (_units[i].health <= 0 || _units[i].unit.movement != Data.UnitMoveType.ground || i != unitIndex || _units[i].target < 0 || _units[i].mainTarget != target.Key || _units[i].mainTarget < 0 || _buildings[_units[i].mainTarget].building.id != Data.BuildingID.wall || _buildings[_units[i].mainTarget].health <= 0)
                        {
                            continue;
                        }
                        BattleVector2Int pos = WorldToGridPosition(_units[i].position);
                        List<Cell> pts = search.Find(new Vector2Int(pos.x, pos.y), new Vector2Int(unitGridPosition.x, unitGridPosition.y)).ToList();
                        if (Path.IsValid(ref pts, new Vector2Int(pos.x, pos.y), new Vector2Int(unitGridPosition.x, unitGridPosition.y)))
                        {
                            float dis = GetPathLength(pts, false);
                            if (id <= Data.battleGroupWallAttackRadius)
                            {
                                Vector2Int end = _units[i].path.points.Last().Location;
                                Path p = new Path();
                                if (p.Create(ref search, pos, new BattleVector2Int(end.X, end.Y)))
                                {
                                    _units[unitIndex].mainTarget = target.Key;
                                    p.blocks = _units[i].path.blocks;
                                    p.length = GetPathLength(p.points);
                                    return (_units[i].target, p);
                                }
                            }
                        }
                    }
                    Path path = new Path();
                    if (path.Create(ref unlimitedSearch, unitGridPosition, new BattleVector2Int(_buildings[target.Key].building.x, _buildings[target.Key].building.y)))
                    {
                        path.length = GetPathLength(path.points);
                        for (int i = 0; i < path.points.Count; i++)
                        {
                            for (int j = 0; j < blockedTiles.Count; j++)
                            {
                                if (blockedTiles[j].position.x == path.points[i].Location.X && blockedTiles[j].position.y == path.points[i].Location.Y)
                                {
                                    if (blockedTiles[j].id == Data.BuildingID.wall && _buildings[blockedTiles[j].index].health > 0)
                                    {
                                        int t = blockedTiles[j].index;
                                        for (int k = path.points.Count - 1; k >= j; k--)
                                        {
                                            path.points.RemoveAt(k);
                                        }
                                        path.length = GetPathLength(path.points);
                                        return (t, path);
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return (-1, null);
        }

        private (int, Path) GetPathToBuilding(int buildingIndex, int unitIndex)
        {
            if (_buildings[buildingIndex].building.id == Data.BuildingID.wall || _buildings[buildingIndex].building.id == Data.BuildingID.decoration || _buildings[buildingIndex].building.id == Data.BuildingID.obstacle)
            {
                return (-1, null);
            }

            BattleVector2Int unitGridPosition = WorldToGridPosition(_units[unitIndex].position);

            // Get the x and y list of the building's surrounding tiles
            List<int> columns = new List<int>();
            List<int> rows = new List<int>();
            int startX = _buildings[buildingIndex].building.x;
            int endX = _buildings[buildingIndex].building.x + _buildings[buildingIndex].building.columns - 1;
            int startY = _buildings[buildingIndex].building.y;
            int endY = _buildings[buildingIndex].building.y + _buildings[buildingIndex].building.rows - 1;
            if (_units[unitIndex].unit.movement == Data.UnitMoveType.ground && _buildings[buildingIndex].building.id == Data.BuildingID.wall)
            {
                startX--;
                startY--;
                endX++;
                endY++;
            }
            columns.Add(startX);
            columns.Add(endX);
            rows.Add(startY);
            rows.Add(endY);

            // Get the list of building's available surrounding tiles
            List<Path> tiles = new List<Path>();
            if (_units[unitIndex].unit.movement == Data.UnitMoveType.ground)
            {
                #region With Walls Effect
                int closest = -1;
                float distance = 99999;
                int blocks = 999;
                for (int x = 0; x < columns.Count; x++)
                {
                    for (int y = 0; y < rows.Count; y++)
                    {
                        if (x >= 0 && y >= 0 && x < Data.gridSize && y < Data.gridSize)
                        {
                            Path path1 = new Path();
                            Path path2 = new Path();
                            path1.Create(ref search, new BattleVector2Int(columns[x], rows[y]), unitGridPosition);
                            path2.Create(ref unlimitedSearch, new BattleVector2Int(columns[x], rows[y]), unitGridPosition);
                            if (path1.points != null && path1.points.Count > 0)
                            {
                                path1.length = GetPathLength(path1.points);
                                int lengthToBlocks = (int)Math.Floor(path1.length / (Data.battleTilesWorthOfOneWall * Data.gridCellSize));
                                if (path1.length < distance && lengthToBlocks <= blocks)
                                {
                                    closest = tiles.Count;
                                    distance = path1.length;
                                    blocks = lengthToBlocks;
                                }
                                tiles.Add(path1);
                            }
                            if (path2.points != null && path2.points.Count > 0)
                            {
                                path2.length = GetPathLength(path2.points);
                                for (int i = 0; i < path2.points.Count; i++)
                                {
                                    for (int j = 0; j < blockedTiles.Count; j++)
                                    {
                                        if (blockedTiles[j].position.x == path2.points[i].Location.X && blockedTiles[j].position.y == path2.points[i].Location.Y)
                                        {
                                            if (blockedTiles[j].id == Data.BuildingID.wall && _buildings[blockedTiles[j].index].health > 0)
                                            {
                                                path2.blocks.Add(blockedTiles[j]);
                                                // path2.blocksHealth += _buildings[blockedTiles[j].index].health;
                                            }
                                            break;
                                        }
                                    }
                                }
                                if (path2.length < distance && path2.blocks.Count <= blocks)
                                {
                                    closest = tiles.Count;
                                    distance = path1.length;
                                    blocks = path2.blocks.Count;
                                }
                                tiles.Add(path2);
                            }
                        }
                    }
                }
                tiles[closest].points.Reverse();
                if (tiles[closest].blocks.Count > 0)
                {
                    for (int i = 0; i < _units.Count; i++)
                    {
                        if (_units[i].health <= 0 || _units[i].unit.movement != Data.UnitMoveType.ground || i != unitIndex || _units[i].target < 0 || _units[i].mainTarget != buildingIndex || _units[i].mainTarget < 0 || _buildings[_units[i].mainTarget].building.id != Data.BuildingID.wall || _buildings[_units[i].mainTarget].health <= 0)
                        {
                            continue;
                        }
                        BattleVector2Int pos = WorldToGridPosition(_units[i].position);
                        List<Cell> points = search.Find(new Vector2Int(pos.x, pos.y), new Vector2Int(unitGridPosition.x, unitGridPosition.y)).ToList();
                        if (!Path.IsValid(ref points, new Vector2Int(pos.x, pos.y), new Vector2Int(unitGridPosition.x, unitGridPosition.y)))
                        {
                            continue;
                        }
                        // float dis = GetPathLength(points, false);
                        if (id <= Data.battleGroupWallAttackRadius)
                        {
                            Vector2Int end = _units[i].path.points.Last().Location;
                            Path path = new Path();
                            if (path.Create(ref search, pos, new BattleVector2Int(end.X, end.Y)))
                            {
                                _units[unitIndex].mainTarget = buildingIndex;
                                path.blocks = _units[i].path.blocks;
                                path.length = GetPathLength(path.points);
                                return (_units[i].target, path);
                            }
                        }
                    }

                    Tile last = tiles[closest].blocks.Last();
                    for (int i = tiles[closest].points.Count - 1; i >= 0; i--)
                    {
                        int x = tiles[closest].points[i].Location.X;
                        int y = tiles[closest].points[i].Location.Y;
                        tiles[closest].points.RemoveAt(i);
                        if (x == last.position.x && y == last.position.y)
                        {
                            break;
                        }
                    }
                    _units[unitIndex].mainTarget = buildingIndex;
                    return (last.index, tiles[closest]);
                }
                else
                {
                    return (buildingIndex, tiles[closest]);
                }
                #endregion
            }
            else
            {
                #region Without Walls Effect
                int closest = -1;
                float distance = 99999;
                for (int x = 0; x < columns.Count; x++)
                {
                    for (int y = 0; y < rows.Count; y++)
                    {
                        if (columns[x] >= 0 && rows[y] >= 0 && columns[x] < Data.gridSize && rows[y] < Data.gridSize)
                        {
                            Path path = new Path();
                            if (path.Create(ref unlimitedSearch, new BattleVector2Int(columns[x], rows[y]), unitGridPosition))
                            {
                                path.length = GetPathLength(path.points);
                                if (path.length < distance)
                                {
                                    closest = tiles.Count;
                                    distance = path.length;
                                }
                                tiles.Add(path);
                            }
                        }
                    }
                }
                if (closest >= 0)
                {
                    tiles[closest].points.Reverse();
                    return (buildingIndex, tiles[closest]);
                }
                #endregion
            }
            return (-1, null);
        }

        private bool IsBuildingInRange(int unitIndex, int buildingIndex)
        {
            for (int x = _buildings[buildingIndex].building.x; x < _buildings[buildingIndex].building.x + _buildings[buildingIndex].building.columns; x++)
            {
                for (int y = _buildings[buildingIndex].building.y; y < _buildings[buildingIndex].building.y + _buildings[buildingIndex].building.columns; y++)
                {
                    float distance = BattleVector2.Distance(GridToWorldPosition(new BattleVector2Int(x, y)), _units[unitIndex].position);
                    if (distance <= _units[unitIndex].unit.attackRange)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static float GetPathLength(IList<Cell> path, bool includeCellSize = true)
        {
            float length = 0;
            if (path != null && path.Count > 1)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    length += BattleVector2.Distance(new BattleVector2(path[i - 1].Location.X, path[i - 1].Location.Y), new BattleVector2(path[i].Location.X, path[i].Location.Y));
                }
            }
            if (includeCellSize)
            {
                length *= Data.gridCellSize;
            }
            return length;
        }

        private bool HandleSpell(int index, double deltaTime)
        {
            bool end = false;
            _spells[index].palsesTimer += deltaTime;
            if (_spells[index].palsesTimer >= _spells[index].spell.server.pulsesDuration)
            {
                _spells[index].palsesTimer -= _spells[index].spell.server.pulsesDuration;
                _spells[index].palsesDone += 1;
                switch (_spells[index].spell.id)
                {
                    case Data.SpellID.lightning:
                        for (int i = 0; i < _buildings.Count; i++)
                        {
                            if (_buildings[i].health <= 0 || !IsBuildingCanBeAttacked(_buildings[i].building.id)) { continue; }
                            float damage = (float)Math.Ceiling(GetBuildingInSpellRangePercentage(index, i) * _spells[index].spell.server.pulsesValue);
                            if (damage <= 0) { continue; }
                            _buildings[i].TakeDamage(damage, ref grid, ref blockedTiles, ref percentage, ref fiftyPercentDestroyed, ref townhallDestroyed, ref completelyDestroyed);
                        }
                        break;
                    case Data.SpellID.healing:
                        for (int i = 0; i < _units.Count; i++)
                        {
                            if (_units[i].health <= 0) { continue; }
                            float distance = BattleVector2.Distance(_units[i].position, _spells[index].position);
                            if (distance > _spells[index].spell.server.radius * Data.gridCellSize) { continue; }
                            _units[i].Heal(_spells[index].spell.server.pulsesValue);
                        }
                        break;
                    case Data.SpellID.rage:

                        break;
                    case Data.SpellID.jump:

                        break;
                    case Data.SpellID.freeze:

                        break;
                    case Data.SpellID.invisibility:

                        break;
                    case Data.SpellID.earthquake:

                        break;
                    case Data.SpellID.haste:

                        break;
                    case Data.SpellID.skeleton:

                        break;
                    case Data.SpellID.bat:

                        break;
                }
                if (_spells[index].pulseCallback != null)
                {
                    _spells[index].pulseCallback.Invoke(_spells[index].spell.databaseID);
                }
            }
            if (_spells[index].palsesDone >= _spells[index].spell.server.pulsesCount)
            {
                _spells[index].done = true;
                if (_spells[index].doneCallback != null)
                {
                    _spells[index].doneCallback.Invoke(_spells[index].spell.databaseID);
                }
                end = true;
            }
            return end;
        }

        private double GetBuildingInSpellRangePercentage(int spellIndex, int buildingIndex)
        {
            double percentage = 0;
            float distance = BattleVector2.Distance(_buildings[buildingIndex].worldCenterPosition, _spells[spellIndex].position);
            float radius = Math.Max(_buildings[buildingIndex].building.columns, _buildings[buildingIndex].building.rows) * Data.gridCellSize / 2f;
            float delta = (_spells[spellIndex].spell.server.radius * Data.gridCellSize) - (distance + radius);
            if (delta >= 0)
            {
                percentage = 1;
            }
            else
            {
                delta = Math.Abs(delta);
                if (delta < radius * 2f)
                {
                    percentage = delta / (radius * 2f);
                }
            }
            return percentage;
        }

        private float GetUnitDamage(int index)
        {
            float damage = _units[index].unit.damage;
            for (int i = 0; i < _spells.Count; i++)
            {
                if (_spells[i].done) { continue; }
                if (_spells[i].spell.id == Data.SpellID.rage)
                {
                    damage += (_units[index].unit.damage * _spells[i].spell.server.pulsesValue);
                }
            }
            return damage;
        }

        private float GetUnitMoveSpeed(int index)
        {
            float speed = _units[index].unit.moveSpeed;
            for (int i = 0; i < _spells.Count; i++)
            {
                if (_spells[i].done) { continue; }
                if (_spells[i].spell.id == Data.SpellID.rage)
                {
                    speed += _spells[i].spell.server.pulsesValue2;
                }
                else if (_spells[i].spell.id == Data.SpellID.haste)
                {
                    speed += _spells[i].spell.server.pulsesValue;
                }
            }
            return speed;
        }

        public class Path
        {
            public Path()
            {
                length = 0;
                points = null;
                blocks = new List<Tile>();
            }
            public bool Create(ref AStarSearch search, BattleVector2Int start, BattleVector2Int end)
            {
                points = search.Find(new Vector2Int(start.x, start.y), new Vector2Int(end.x, end.y)).ToList();
                if (!IsValid(ref points, new Vector2Int(start.x, start.y), new Vector2Int(end.x, end.y)))
                {
                    points = null;
                    return false;
                }
                else
                {
                    this.start.x = start.x;
                    this.start.y = start.y;
                    this.end.x = end.x;
                    this.end.y = end.y;
                    return true;
                }
            }
            public static bool IsValid(ref List<Cell> points, Vector2Int start, Vector2Int end)
            {
                if (points == null || !points.Any() || !points.Last().Location.Equals(end) || !points.First().Location.Equals(start))
                {
                    return false;
                }
                return true;
            }
            public BattleVector2Int start;
            public BattleVector2Int end;
            public List<Cell> points = null;
            public float length = 0;
            public List<Tile> blocks = null;
            // public float blocksHealth = 0;
        }

        private static BattleVector2 GetPathPosition(IList<Cell> path, float t)
        {
            if (t < 0) { t = 0; }
            if (t > 1) { t = 1; }
            float totalLength = GetPathLength(path);
            float length = 0;
            if (path != null && path.Count > 1)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    BattleVector2Int a = new BattleVector2Int(path[i - 1].Location.X, path[i - 1].Location.Y);
                    BattleVector2Int b = new BattleVector2Int(path[i].Location.X, path[i].Location.Y);
                    float l = BattleVector2.Distance(a, b) * Data.gridCellSize;
                    float p = (length + l) / totalLength;
                    if (p >= t)
                    {
                        t = (t - (length / totalLength)) / (p - (length / totalLength));
                        return BattleVector2.LerpUnclamped(GridToWorldPosition(a), GridToWorldPosition(b), t);
                    }
                    length += l;
                }
            }
            return GridToWorldPosition(new BattleVector2Int(path[0].Location.X, path[0].Location.Y));
        }

        private static BattleVector2 GridToWorldPosition(BattleVector2Int position)
        {
            return new BattleVector2(position.x * Data.gridCellSize + Data.gridCellSize / 2f, position.y * Data.gridCellSize + Data.gridCellSize / 2f);
        }

        private static BattleVector2Int WorldToGridPosition(BattleVector2 position)
        {
            return new BattleVector2Int((int)Math.Floor(position.x / Data.gridCellSize), (int)Math.Floor(position.y / Data.gridCellSize));
        }

        public struct BattleVector2
        {
            public float x;
            public float y;

            public BattleVector2(float x, float y) { this.x = x; this.y = y; }

            public static BattleVector2 LerpUnclamped(BattleVector2 a, BattleVector2 b, float t)
            {
                return new BattleVector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
            }

            public static float Distance(BattleVector2 a, BattleVector2 b)
            {
                float diff_x = a.x - b.x;
                float diff_y = a.y - b.y;
                return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
            }

            public static float Distance(BattleVector2Int a, BattleVector2Int b)
            {
                return Distance(new BattleVector2(a.x, a.y), new BattleVector2(b.x, b.y));
            }

            /// <summary>
            /// Smootly moves a vector2 to another vector2 with desired speed.
            /// </summary>
            /// <param name="source">Position which you want to move from.</param>
            /// <param name="target">Position which you want to reach.</param>
            /// <param name="speed">Move distance per second. Note: Do not multiply delta time to speed.</param>
            public static BattleVector2 LerpStatic(BattleVector2 source, BattleVector2 target, float deltaTime, float speed)
            {
                if ((source.x == target.x && source.y == target.y) || speed <= 0) { return source; }
                float distance = Distance(source, target);
                float t = speed * deltaTime;
                if (t > distance) { t = distance; }
                return LerpUnclamped(source, target, distance == 0 ? 1 : t / distance);
            }
        }

        public struct BattleVector2Int
        {
            public int x;
            public int y;

            public BattleVector2Int(int x, int y) { this.x = x; this.y = y; }
        }

        public static bool IsBuildingCanBeAttacked(Data.BuildingID id)
        {
            switch (id)
            {
                case Data.BuildingID.obstacle:
                case Data.BuildingID.decoration:
                case Data.BuildingID.boomb:
                case Data.BuildingID.springtrap:
                case Data.BuildingID.airbomb:
                case Data.BuildingID.giantbomb:
                case Data.BuildingID.seekingairmine:
                case Data.BuildingID.skeletontrap:
                    return false;
            }
            return true;
        }

    }
}