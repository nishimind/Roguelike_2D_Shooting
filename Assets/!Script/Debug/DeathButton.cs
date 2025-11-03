using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathButton : MonoBehaviour
{
    
     //Start is called before the first frame update
  public void Death() { PlayerStatus.Instance.currentHP = 0; }
}
