using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plant : LivingEntity
{
    float amountRemaining = 1;
    const float consumeSpeed = 8;

    float reproduction;
    float timer;

    public override void Init(Coord coord)
    {
        base.Init(coord);
        reproduction = Random.Range(1, 400);
        timer = Time.time;
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
        float amountConsumed = Mathf.Max(0, Mathf.Min(amountRemaining, amount));
        amountRemaining -= amount * consumeSpeed;

        transform.localScale = Vector3.one * amountRemaining;

        if (amountRemaining <= 0)
        {
            Die(CauseOfDeath.Eaten);
        }

        return amountConsumed;
    }

    public float AmountRemaining
    {
        get
        {
            return amountRemaining;
        }
    }
}