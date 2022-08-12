using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerControllerInterface
{
    public void hurtPlayer();

    public void killPlayer();

    public void revivePlayer();

    public void setAnimationTrigger(string trigger);
}
