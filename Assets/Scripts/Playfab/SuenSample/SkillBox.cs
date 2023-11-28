using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Skill
{
    public string name;
    public int level;
    public Skill(string _name, int _level)
    {
        name = _name;
        level = _level;
    }
}
public class SkillBox : MonoBehaviour
{
    [SerializeField] TMP_InputField skillName;
    [SerializeField] Slider skillLevelSlider;
    [SerializeField] TMP_Text skillLevelText;

    public Skill ReturnClass()
    {
        return new Skill(skillName.text, (int)skillLevelSlider.value);
    }
    public void SetUI(Skill sk)
    {
        skillName.text = sk.name;
        skillLevelSlider.value = sk.level;
    }
    public void SliderChangeUpdate(float num)
    {
        skillLevelText.text = skillLevelSlider.value.ToString();
    }
    
}
