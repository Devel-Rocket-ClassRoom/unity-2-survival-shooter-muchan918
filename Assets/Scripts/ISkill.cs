using UnityEngine;

public interface ISkill
{
    float SkillInterval { get; set; }
    float LastSkillTime { get; set; }
    void Use();
}