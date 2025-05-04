using System.Collections.Generic;
using UnityEngine;

public class Harpoon : Weapon
{
    [SerializeField] private int m_WiresNumber = 1;
    [SerializeField] private Wire m_Wire;

    private Queue<Wire> m_Wires = new();

    public void RewindWire(Wire i_Wire)
    {
        m_Wires.Enqueue(i_Wire);
    }

    public override void OnPick(GameObject i_Player)
    {
        base.OnPick(i_Player);

        // Instantiate wires
        Transform gameArea = i_Player.transform.parent;
        for (int i = 0; i < m_WiresNumber; ++i)
        {
            // Create a new wire in the game area and store it
            Wire wire = Instantiate(m_Wire, gameArea);
            wire.gameObject.SetActive(false);
            wire.SetHarpoon(this);
            m_Wires.Enqueue(wire);
        }
    }

    public override void Destroy()
    {
        StartCoroutine(DestroyWires());
    }

    private IEnumerator<WaitForSeconds> DestroyWires()
    {
        // Schedule wires destruction for when they will be safely done winding
        yield return new WaitForSeconds(1f);
        foreach (Wire wire in m_Wires)
        {
            Destroy(wire.gameObject);
        }
        // Finally, destroy the harpoon itself
        base.Destroy();
    }

    public override bool CanShoot()
    {
        return m_Wires.Count > 0;
    }

    protected override void OnShoot()
    {
        if (m_Wires.Count <= 0) return;

        // Get the first available wire
        Wire wire = m_Wires.Dequeue();

        // Move wire at player's feet
        wire.transform.position = transform.parent.position;
        wire.Shoot();
    }

    protected override void Awake()
    {
        base.Awake();
        if (m_Wire == null) Debug.LogError("Harpoon wire is not set");
    }
}
