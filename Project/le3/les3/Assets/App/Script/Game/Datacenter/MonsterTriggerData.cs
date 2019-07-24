using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MonsterTriggerData : Framework.IRecord
{
    public int ID()
    {
        return id;
    }

    public int id;

    public float time;

    public int repeat;

    public int monsterid;
}

