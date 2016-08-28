using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShipData {
  public string identifier, displayName;
  public Mesh mesh;
  public float angularThrustMod = 1.0f;
  public float thrustMod = 1.0f;
  public int maxAmmo, maxShield, maxHealth;
  public float fireRate, shieldRate, ammoRate;
  public Vector3[] shotSpawns;
}
