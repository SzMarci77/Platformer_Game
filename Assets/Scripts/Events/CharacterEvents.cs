using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public class CharacterEvents
{

    //Character damaged and damage value
    public static UnityAction<GameObject, int> characterDamaged;

    //Character heald and heal value
    public static UnityAction<GameObject, int> characterHealed;
}
