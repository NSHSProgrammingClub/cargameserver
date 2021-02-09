using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Ammunition
{
    public int current;
    public int total;
    public int generic;

    public Ammunition(int generic, int total)
    {
        this.current = generic;
        this.generic = generic;
        this.total = total;
    }
}
