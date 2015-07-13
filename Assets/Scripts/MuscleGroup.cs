using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MuscleGroup {

    public static int[] muscleToGroupId { get; private set; }
    public static List<MuscleGroup> groups { get; private set; }

    static MuscleGroup()
    {
        muscleToGroupId = new int[MuscleDataLoader.numMuscleElements];
        groups = new List<MuscleGroup>();

        setMuscleGroup(0, 6, "Adductor brev."); // TODO split?
        setMuscleGroup(6, 12, "Adductor long.");
        setMuscleGroup(12, 15, "Adductor magn. dist.");
        setMuscleGroup(15, 21, "Adductor magn. mid.");
        setMuscleGroup(21, 25, "Adductor magn. prox.");
        setMuscleGroup(25, 26, "Biceps femoris CL");
        setMuscleGroup(26, 29, "Biceps femoris CB");
        setMuscleGroup(29, 32, "Ext. dig. long.");
        setMuscleGroup(32, 35, "Ext. hal. long.");
        setMuscleGroup(35, 38, "Flex. dig. long.");
        setMuscleGroup(38, 41, "Flex. hal. long.");
        setMuscleGroup(41, 42, "Gastrocnemius lat.");
        setMuscleGroup(42, 43, "Gastrocnemius med.");
        setMuscleGroup(43, 44, "Gemellus inf.");
        setMuscleGroup(44, 45, "Gemellus sup.");
        setMuscleGroup(45, 51, "Glut. max. sup.");
        setMuscleGroup(51, 57, "Glut. max. inf.");
        setMuscleGroup(57, 63, "Glut. med. ant.");
        setMuscleGroup(63, 69, "Glut. med. post.");
        setMuscleGroup(69, 72, "Glut. min."); // TODO split?
        setMuscleGroup(72, 74, "Gracilis");
        setMuscleGroup(74, 77, "Iliacus lat.");
        setMuscleGroup(77, 80, "Iliacus mid.");
        setMuscleGroup(80, 83, "Iliacus med.");
        setMuscleGroup(83, 85, "Obt. ext. inf.");
        setMuscleGroup(85, 88, "Obt. ext. sup.");
        setMuscleGroup(88, 91, "Obtutator int.");
        setMuscleGroup(91, 95, "Pectineus");
        setMuscleGroup(95, 98, "Peroneus brev.");
        setMuscleGroup(98, 101, "Peroneus long.");
        setMuscleGroup(101, 104, "Peroneus tert.");
        setMuscleGroup(104, 105, "Piriformis");
        setMuscleGroup(105, 106, "Plantaris");
        setMuscleGroup(106, 108, "Popliteus");
        setMuscleGroup(108, 109, "Psoas minor");
        setMuscleGroup(109, 112, "Psoas major");
        setMuscleGroup(112, 116, "Quadratus fem.");
        setMuscleGroup(116, 118, "Rectus femoris");
        setMuscleGroup(118, 119, "Sartorius prox.");
        setMuscleGroup(119, 120, "Sartorius dist.");
        setMuscleGroup(120, 121, "Semimembranosus");
        setMuscleGroup(121, 122, "Semitendinosus");
        setMuscleGroup(122, 125, "Soleus med.");
        setMuscleGroup(125, 128, "Soleus lat.");
        setMuscleGroup(128, 130, "Tensor fasc. l.");
        setMuscleGroup(130, 133, "Tibialis ant.");
        setMuscleGroup(133, 136, "Tibialis post. med.");
        setMuscleGroup(136, 139, "Tibialis post. lat.");
        setMuscleGroup(139, 145, "Vastus interm.");
        setMuscleGroup(145, 151, "Vastus lat. inf.");
        setMuscleGroup(151, 153, "Vastus lat. sup.");
        setMuscleGroup(153, 163, "Vastus med."); // TODO split?
    }

    private static void setMuscleGroup(int rangeStart, int rangeEnd, string groupName)
    {
        int groupIndex = groups.Count;
        for (int i = rangeStart; i < rangeEnd; i++)
        {
            muscleToGroupId[i] = groupIndex;
        }
        groups.Add(new MuscleGroup(groupIndex, groupName));
    }

    public int index { get; private set; }
    public string name { get; private set; }

    public MuscleGroup(int index, string name)
    {
        this.index = index;
        this.name = name;
    }
}
