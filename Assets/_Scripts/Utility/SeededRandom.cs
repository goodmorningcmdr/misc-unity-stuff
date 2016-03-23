using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeededRandom
{
	protected System.Random _random;
	protected int _seed;

	public int seed
	{
		get
		{
			return _seed;
		}
	}

	public void Reseed()
	{
		SetSeed(Range(0, int.MaxValue));
	}

	public SeededRandom(int seed)
	{
		SetSeed(seed);
	}

	public SeededRandom()
	{
		SetSeed((int)System.DateTime.Now.Ticks & 0x0000FFFF);
	}

	public void SetSeed(int seed)
	{
		_seed = seed;
		_random = new System.Random(seed);
	}

	public float Range(float min, float max)
	{
		if (min > max)
		{
			float temp = max;
			max = min;
			min = temp;
		}

		return Mathf.Lerp(min, max, value);
	}

	public float value
	{
		get
		{
			return (float)_random.Next() / (int.MaxValue - 1);
		}
	}

	public int Range(int min, int max)
	{
		if (min > max)
		{
			int temp = max;
			max = min;
			min = temp;
		}

		return _random.Next(min, max);
	}

	public T GetRandom<T>(List<T> list)
	{
		if (list == null || list.Count == 0)
		{
			return default(T);
		}
		
		return list[Range(0, list.Count)];
	}
}