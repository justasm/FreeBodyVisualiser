using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MuscleGroup {

    public static int[] muscleToGroup { get; private set; }
    public static List<string> groupNames { get; private set; }

    static MuscleGroup()
    {
        muscleToGroup = new int[MuscleDataLoader.numMuscleElements];
        groupNames = new List<string>();

        setMuscleGroup(0, 25, "Adductors");
        setMuscleGroup(25, 29, "Biceps Femoris");
        setMuscleGroup(29, 32, "Ext. dig. long.");
        setMuscleGroup(32, 35, "Ext. hal. long.");
        setMuscleGroup(35, 38, "Flex. dig. long.");
        setMuscleGroup(38, 41, "Flex. hal. long.");
        setMuscleGroup(41, 43, "Gastrocnemius");
        setMuscleGroup(43, 45, "Gemellus");
        setMuscleGroup(45, 57, "Glut. max.");
        setMuscleGroup(57, 69, "Glut. med.");
        setMuscleGroup(69, 72, "Glut. min.");
        setMuscleGroup(72, 83, "Iliopsoas");
        setMuscleGroup(83, 88, "Obt. ext.");
        setMuscleGroup(88, 91, "Obtutator int.");
        setMuscleGroup(91, 95, "Pectineus");
        setMuscleGroup(95, 104, "Peroneus");
        setMuscleGroup(104, 105, "Piriformis");
        setMuscleGroup(105, 106, "Plantaris");
        setMuscleGroup(106, 108, "Popliteus");
        setMuscleGroup(108, 112, "Iliopsoas");
        setMuscleGroup(112, 116, "Quadratus fem.");
        setMuscleGroup(116, 118, "Rectus femoris");
        setMuscleGroup(118, 120, "Sartorius");
        setMuscleGroup(120, 122, "Semitendinosus and semimembranosus");
        setMuscleGroup(122, 128, "Soleus");
        setMuscleGroup(128, 130, "TFL");
        setMuscleGroup(130, 139, "Tibialis");
        setMuscleGroup(139, 145, "Vastus interm.");
        setMuscleGroup(145, 153, "Vastus lat.");
        setMuscleGroup(153, 163, "Vastus med.");
    }

    private static void setMuscleGroup(int rangeStart, int rangeEnd, string groupName)
    {
        int group = groupNames.Count;
        for (int i = rangeStart; i < rangeEnd; i++)
        {
            muscleToGroup[i] = group;
        }
        groupNames.Add(groupName);
    }

    public int id;
    public string name;
}
