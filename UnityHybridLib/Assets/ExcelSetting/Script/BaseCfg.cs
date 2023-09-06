using System.Collections;
using System.Collections.Generic;
using System;


[Serializable]
public class BaseCfg 
{
    public string id;


    protected static List<string> parseArray(string origin)
    {
        if (string.IsNullOrEmpty(origin))
            return new List<string>();

        if(origin.Contains("|"))
        {
            return new List<string>(origin.Split("|"));
        }

        return new List<string>() { origin };
    }

}
