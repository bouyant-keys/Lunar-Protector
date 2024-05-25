using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    public bool _canInteract = false;

    LaserDish _laserDish;

    public void Interact()
    {
        _laserDish.StartMinigame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LaserDish")
        {
            if (!collision.TryGetComponent(out LaserDish laserDish)) return;
            _laserDish = laserDish;
            _canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "LaserDish")
        {
            _laserDish = null;
            _canInteract = false;
        }
    }
}
