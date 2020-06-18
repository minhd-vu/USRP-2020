using System;
using static System.Math;

[Serializable]
public class Genes
{
    const float mutationChance = .2f;
    const float maxMutationAmount = .3f;

    static readonly System.Random prng = new System.Random();

    public bool isMale;
    public float[] values;

    public Genes(float[] values)
    {
        isMale = RandomValue() < 0.5f;
        this.values = values;
    }

    public static Genes RandomGenes(int num)
    {
        float[] values = new float[num];
        for (int i = 0; i < num; i++)
        {
            values[i] = RandomValue();
        }
        return new Genes(values);
    }

    public static Genes InheritedGenes(Genes mother, Genes father)
    {
        float[] values = new float[mother.values.Length];

        for (int i = 0; i < values.Length; ++i)
        {
            values[i] = (mother.values[i] + father.values[i]) / 2.0f + RandomValue() < mutationChance ? maxMutationAmount * RandomValue() : 0;
        }

        return new Genes(values);
    }

    static float RandomValue()
    {
        return (float)prng.NextDouble();
    }

    static float RandomGaussian()
    {
        double u1 = 1 - prng.NextDouble();
        double u2 = 1 - prng.NextDouble();
        double randStdNormal = Sqrt(-2 * Log(u1)) * Sin(2 * PI * u2);
        return (float)randStdNormal;
    }
}