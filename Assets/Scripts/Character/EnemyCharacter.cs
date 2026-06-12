using UnityEngine;

public class EnemyCharacter : CharacterBase
{
    public override bool IsPlayer()
    {
        return false;
    }
}
