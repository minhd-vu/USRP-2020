using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plant : LivingEntity
{
    const float consumeSpeed = 8;

    float reproduction;
    float timer;

    public override void Init(Coord coord)
    {
        base.Init(coord);
        reproduction = Random.Range(1, 200);
        timer = Time.time + Random.Range(0, reproduction);
    }

    private void Update()
    {
        if (Time.time - timer > reproduction)
        {
            Coord c = Environment.GetNextTileRandom(coord);
            var entities = Environment.speciesMaps[Species.Plant].GetEntities(c, 1);
            if (entities != null && entities.Count == 0)
            {
                var entity = Instantiate(prefab);
                entity.Init(c);
                Environment.speciesMaps[Species.Plant].Add(entity, c);
            }

            timer = Time.time;
        }
    }

    public float Consume(float amount)
    {
        float amountConsumed = Mathf.Max(0, Mathf.Min(AmountRemaining, amount));
        AmountRemaining -= amount * consumeSpeed;

        transform.localScale = Vector3.one * AmountRemaining;

        if (AmountRemaining <= 0)
        {
            Die(CauseOfDeath.Eaten);
        }

        return amountConsumed;
    }

    public float AmountRemaining { get; private set; } = 1;
}