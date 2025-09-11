using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathButton : MonoBehaviour
{
    public PlayerHealth health;
    // Start is called before the first frame update
  public void Death() { health.currentHP = 0; }
}
