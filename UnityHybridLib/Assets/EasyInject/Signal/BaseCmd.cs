using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseCmd 
{
    public object SIGNAL { get; set; }

    public virtual void Exec() { }
}
