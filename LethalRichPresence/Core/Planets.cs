using System.Collections.Generic;

namespace LethalRichPresence;

public class Planets
{
  public static Dictionary<string, string> planets = new()
    {
        {"", "In orbit"},
        {"SampleSceneRelay", "In orbit"},
        {"InitSceneLaunchOptions", "Modded Moon"},
        {"CompanyBuilding", "71-Gordion"},
        {"Level1Experimentation","41-Experimentation"},
        {"Level2Assurance","220-Assurance"},
        {"Level3Vow","56-Vow"},
        {"Level4March","61-March"},
        {"Level5Rend","85-Rend"},
        {"Level6Dine","7-Dine"},
        {"Level7Offense","21-Offense"},
        {"Level8Titan","8-Titan"},
    };

  public static Dictionary<string, string> weathers = new(){
      {"None", "Clear weather"},
      {"DustClouds", ""},
      {"Rainy", "Rainy"},
      {"Stormy", "Stormy"},
      {"Foggy", "Foggy"},
      {"Flooded", "Flooded"},
      {"Eclipsed", "Eclipsed"}
    };

}