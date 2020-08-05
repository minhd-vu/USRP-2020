using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class Animal : LivingEntity
{
    public const int maxViewDistance = 10;

    [EnumFlags]
    public Species diet;

    public CreatureAction currentAction;
    [SerializeField]
    public Genes genes;
    public Color maleColour;
    public Color femaleColour;

    // Settings
    float timeBetweenActionChoices = 1;
    public float moveSpeed = 1.0f;

    public float hungerTimeFactor = 300;
    public float thirstTimeFactor = 200;
    public float staminaTimeFactor = 150;
    public float desireTimeFactor = 500;
    public float lifespanTimeFactor = 600;

    float drinkDuration = 6;
    float eatDuration = 10;
    float restDuration = 14;

    // Visual settings
    float moveArcHeight = .2f;

    [Header("State")]
    public float hunger;
    public float thirst;
    public float stamina;
    public float desire;
    public float lifespan;

    bool canReproduce;
    private enum GeneNames
    {
        Speed = 0,
        Hunger,
        Thirst,
        Stamina,
        Desire,
        Lifespan
    }

    // Used for targeting movement
    protected LivingEntity foodTarget;
    protected Coord waterTarget;
    protected Animal mateTarget;

    // Movement data
    bool animatingMovement;
    Coord moveFromCoord;
    Coord moveTargetCoord;
    Vector3 moveStartPos;
    Vector3 moveTargetPos;
    float moveTime;
    float moveSpeedFactor;
    float moveArcHeightFactor;
    Coord[] path;
    int pathIndex;

    // Other
    float lastActionChooseTime;
    const float sqrtTwo = 1.4142f;
    const float oneOverSqrtTwo = 1 / sqrtTwo;

    public override void Init(Coord coord)
    {
        base.Init(coord);
        moveFromCoord = coord;

        genes = Genes.RandomGenes(Enum.GetNames(typeof(GeneNames)).Length);
        moveSpeed += 0.1f * genes.values[(int)GeneNames.Speed];
        hungerTimeFactor *= genes.values[(int)GeneNames.Hunger];
        thirstTimeFactor *= genes.values[(int)GeneNames.Thirst];
        staminaTimeFactor *= genes.values[(int)GeneNames.Stamina];
        desireTimeFactor *= genes.values[(int)GeneNames.Desire];
        lifespanTimeFactor *= genes.values[(int)GeneNames.Lifespan];
        material.color = (genes.isMale) ? maleColour : femaleColour;
        canReproduce = true;

        ChooseNextAction();
    }

    protected virtual void Update()
    {
        // Increase stats over time; these influence what the animals does.
        hunger += Time.deltaTime / hungerTimeFactor;
        thirst += Time.deltaTime / thirstTimeFactor;
        stamina += Time.deltaTime / staminaTimeFactor;
        if (canReproduce && lifespan > 0.4)
        {
            desire += Time.deltaTime / desireTimeFactor;
        }
        lifespan += Time.deltaTime / lifespanTimeFactor;

        // Animate movement. After moving a single tile, the animal will be able to choose its next action.
        if (animatingMovement)
        {
            AnimateMove();
        }
        else
        {
            // Handle interactions with external things, like food, water, mates.
            HandleInteractions();

            if (Time.time - lastActionChooseTime > timeBetweenActionChoices)
            {
                ChooseNextAction();
            }
        }

        if (hunger >= 1)
        {
            Die(CauseOfDeath.Hunger);
        }
        else if (thirst >= 1)
        {
            Die(CauseOfDeath.Thirst);
        }
        else if (stamina >= 1)
        {
            Die(CauseOfDeath.Exhaustion);
        }
        else if (lifespan >= 1)
        {
            Die(CauseOfDeath.Age);
        }
    }

    // Animals choose their next action after each movement step (1 tile),
    // or, when not moving (e.g interacting with food etc), at a fixed time interval.
    protected virtual void ChooseNextAction()
    {
        lastActionChooseTime = Time.time;

        float[] states = { hunger, thirst, stamina, desire };
        float max = states.Max();

        if (max == desire && canReproduce && lifespan > 0.5)
        {
            FindPotentialMates();
        }
        else if (max == stamina)
        {
            currentAction = CreatureAction.Resting;
        }
        else if (max == hunger)
        {
            FindFood();
        }
        else if (max == thirst)
        {
            FindWater();
        }

        Act();
    }

    protected virtual void FindFood()
    {
        LivingEntity foodSource = Environment.SenseFood(coord, this, FoodPreferencePenalty);
        if (foodSource)
        {
            currentAction = CreatureAction.GoingToFood;
            foodTarget = foodSource;
            CreatePath(foodTarget.coord);

        }
        else
        {
            currentAction = CreatureAction.Exploring;
        }
    }

    protected virtual void FindWater()
    {
        Coord waterTile = Environment.SenseWater(coord);
        if (waterTile != Coord.invalid)
        {
            currentAction = CreatureAction.GoingToWater;
            waterTarget = waterTile;
            CreatePath(waterTarget);

        }
        else
        {
            currentAction = CreatureAction.Exploring;
        }
    }

    protected virtual void FindPotentialMates()
    {
        List<Animal> potentialMates = Environment.SensePotentialMates(coord, this);
        if (potentialMates.Count > 0)
        {
            currentAction = CreatureAction.SearchingForMate;
            mateTarget = potentialMates[UnityEngine.Random.Range(0, potentialMates.Count)];
            CreatePath(mateTarget.coord);
        }
        else
        {
            currentAction = CreatureAction.Exploring;
        }
    }

    // When choosing from multiple food sources, the one with the lowest penalty will be selected.
    protected virtual int FoodPreferencePenalty(LivingEntity self, LivingEntity food)
    {
        return Coord.SqrDistance(self.coord, food.coord);
    }

    protected void Act()
    {
        switch (currentAction)
        {
            case CreatureAction.Exploring:
                StartMoveToCoord(Environment.GetNextTileWeighted(coord, moveFromCoord));
                break;
            case CreatureAction.GoingToFood:
                if (Coord.AreNeighbours(coord, foodTarget.coord))
                {
                    LookAt(foodTarget.coord);
                    currentAction = CreatureAction.Eating;
                }
                else
                {
                    StartMoveToCoord(path[pathIndex]);
                    pathIndex++;
                }
                break;
            case CreatureAction.GoingToWater:
                if (Coord.AreNeighbours(coord, waterTarget))
                {
                    LookAt(waterTarget);
                    currentAction = CreatureAction.Drinking;
                }
                else
                {
                    StartMoveToCoord(path[pathIndex]);
                    pathIndex++;
                }
                break;
            case CreatureAction.SearchingForMate:
                if (Coord.AreNeighbours(coord, mateTarget.coord))
                {
                    LookAt(mateTarget.coord);
                    if (mateTarget && Math.Abs(mateTarget.lifespan - lifespan) < 0.1 && desire > 0 && mateTarget.desire > 0)
                    {
                        desire = 0;
                        mateTarget.desire = 0;
                        //mateTarget.canReproduce = false;
                        //canReproduce = false;

                        Animal entity = (Animal)Instantiate(prefab);
                        entity.Init(coord);
                        entity.genes = Genes.InheritedGenes(genes, mateTarget.genes);
                        Environment.speciesMaps[entity.species].Add(entity, coord);
                    }
                }
                else
                {
                    StartMoveToCoord(path[pathIndex]);
                    pathIndex++;
                }
                break;
        }
    }

    protected void CreatePath(Coord target)
    {
        // Create new path if current is not already going to target.
        if (path == null || pathIndex >= path.Length || (path[path.Length - 1] != target || path[pathIndex - 1] != moveTargetCoord))
        {
            path = EnvironmentUtility.GetPath(coord.x, coord.y, target.x, target.y);
            pathIndex = 0;
        }
    }

    protected void StartMoveToCoord(Coord target)
    {
        moveFromCoord = coord;
        moveTargetCoord = target;
        moveStartPos = transform.position;
        moveTargetPos = Environment.tileCentres[moveTargetCoord.x, moveTargetCoord.y];
        animatingMovement = true;

        bool diagonalMove = Coord.SqrDistance(moveFromCoord, moveTargetCoord) > 1;
        moveArcHeightFactor = (diagonalMove) ? sqrtTwo : 1;
        moveSpeedFactor = (diagonalMove) ? oneOverSqrtTwo : 1;

        LookAt(moveTargetCoord);
    }

    protected void LookAt(Coord target)
    {
        if (target != coord)
        {
            Coord offset = target - coord;
            transform.eulerAngles = Vector3.up * Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg;
        }
    }

    void HandleInteractions()
    {
        switch (currentAction)
        {
            case CreatureAction.Eating:
                if (foodTarget && hunger > 0)
                {
                    float eatAmount = Mathf.Min(hunger, Time.deltaTime / eatDuration);

                    if (foodTarget is Plant)
                    {
                        eatAmount = ((Plant)foodTarget).Consume(eatAmount);
                    }
                    else if (foodTarget is Animal)
                    {
                        eatAmount = hunger;
                        ((Animal)foodTarget).Die(CauseOfDeath.Eaten);
                    }

                    hunger -= eatAmount;
                }
                break;
            case CreatureAction.Drinking:
                if (thirst > 0)
                {
                    thirst -= Time.deltaTime / drinkDuration;
                    thirst = Mathf.Clamp01(thirst);
                }
                break;
            case CreatureAction.Resting:
                if (stamina > 0)
                {
                    stamina -= Time.deltaTime / restDuration;
                    stamina = Mathf.Clamp01(stamina);
                }
                break;
        }
    }

    void AnimateMove()
    {
        // Move in an arc from start to end tile.
        moveTime = Mathf.Min(1, moveTime + Time.deltaTime * moveSpeed * moveSpeedFactor);
        float height = (1 - 4 * (moveTime - .5f) * (moveTime - .5f)) * moveArcHeight * moveArcHeightFactor;
        transform.position = Vector3.Lerp(moveStartPos, moveTargetPos, moveTime) + Vector3.up * height;

        // Finished moving.
        if (moveTime >= 1)
        {
            Environment.RegisterMove(this, coord, moveTargetCoord);
            coord = moveTargetCoord;

            animatingMovement = false;
            moveTime = 0;
            ChooseNextAction();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            var surroundings = Environment.Sense(coord);
            Gizmos.color = Color.white;
            if (surroundings.nearestFoodSource != null)
            {
                Gizmos.DrawLine(transform.position, surroundings.nearestFoodSource.transform.position);
            }
            if (surroundings.nearestWaterTile != Coord.invalid)
            {
                Gizmos.DrawLine(transform.position, Environment.tileCentres[surroundings.nearestWaterTile.x, surroundings.nearestWaterTile.y]);
            }

            if (currentAction == CreatureAction.GoingToFood)
            {
                var path = EnvironmentUtility.GetPath(coord.x, coord.y, foodTarget.coord.x, foodTarget.coord.y);
                Gizmos.color = Color.black;
                for (int i = 0; i < path.Length; i++)
                {
                    Gizmos.DrawSphere(Environment.tileCentres[path[i].x, path[i].y], .2f);
                }
            }
        }
    }

}