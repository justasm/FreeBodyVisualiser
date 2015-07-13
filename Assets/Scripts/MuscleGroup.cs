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

        // Kelly https://eleanormaclure.files.wordpress.com/2011/03/colour-coding.pdf
        string color01 = "FFB300"; //Vivid Yellow
        string color02 = "803E75"; //Strong Purple
        string color03 = "FF6800"; //Vivid Orange
        string color04 = "A6BDD7"; //Very Light Blue
        string color05 = "C10020"; //Vivid Red
        string color06 = "CEA262"; //Grayish Yellow
        string color07 = "817066"; //Medium Gray

        //The following will not be good for people with defective color vision
        string color08 = "007D34"; //Vivid Green
        string color09 = "F6768E"; //Strong Purplish Pink
        string color10 = "00538A"; //Strong Blue
        string color11 = "FF7A5C"; //Strong Yellowish Pink
        string color12 = "53377A"; //Strong Violet
        string color13 = "FF8E00"; //Vivid Orange Yellow
        string color14 = "B32851"; //Strong Purplish Red
        string color15 = "F4C800"; //Vivid Greenish Yellow
        string color16 = "7F180D"; //Strong Reddish Brown
        string color17 = "93AA00"; //Vivid Yellowish Green
        string color18 = "593315"; //Deep Yellowish Brown
        string color19 = "F13A13"; //Vivid Reddish Orange
        string color20 = "232C16"; //Dark Olive Green

        setMuscleGroup(color01, 0, 6, "Adductor brev."); // TODO split?
        setMuscleGroup(color01, 6, 12, "Adductor long.");
        setMuscleGroup(color01, 12, 15, "Adductor magn. dist.");
        setMuscleGroup(color01, 15, 21, "Adductor magn. mid.");
        setMuscleGroup(color01, 21, 25, "Adductor magn. prox.");
        setMuscleGroup(color02, 25, 26, "Biceps femoris CL");
        setMuscleGroup(color02, 26, 29, "Biceps femoris CB");
        setMuscleGroup(color03, 29, 32, "Ext. dig. long.");
        setMuscleGroup(color03, 32, 35, "Ext. hal. long.");
        setMuscleGroup(color03, 35, 38, "Flex. dig. long.");
        setMuscleGroup(color03, 38, 41, "Flex. hal. long.");
        setMuscleGroup(color07, 41, 42, "Gastrocnemius lat.");
        setMuscleGroup(color07, 42, 43, "Gastrocnemius med.");
        setMuscleGroup(color08, 43, 44, "Gemellus inf.");
        setMuscleGroup(color08, 44, 45, "Gemellus sup.");
        setMuscleGroup(color09, 45, 51, "Glut. max. sup.");
        setMuscleGroup(color09, 51, 57, "Glut. max. inf.");
        setMuscleGroup(color09, 57, 63, "Glut. med. ant.");
        setMuscleGroup(color09, 63, 69, "Glut. med. post.");
        setMuscleGroup(color09, 69, 72, "Glut. min."); // TODO split?
        setMuscleGroup(color10, 72, 74, "Gracilis");
        setMuscleGroup(color11, 74, 77, "Iliacus lat.");
        setMuscleGroup(color11, 77, 80, "Iliacus mid.");
        setMuscleGroup(color11, 80, 83, "Iliacus med.");
        setMuscleGroup(color12, 83, 85, "Obt. ext. inf.");
        setMuscleGroup(color12, 85, 88, "Obt. ext. sup.");
        setMuscleGroup(color12, 88, 91, "Obtutator int.");
        setMuscleGroup(color13, 91, 95, "Pectineus");
        setMuscleGroup(color14, 95, 98, "Peroneus brev.");
        setMuscleGroup(color14, 98, 101, "Peroneus long.");
        setMuscleGroup(color14, 101, 104, "Peroneus tert.");
        setMuscleGroup(color15, 104, 105, "Piriformis");
        setMuscleGroup(color15, 105, 106, "Plantaris");
        setMuscleGroup(color15, 106, 108, "Popliteus");
        setMuscleGroup(color16, 108, 109, "Psoas minor");
        setMuscleGroup(color16, 109, 112, "Psoas major");
        setMuscleGroup(color04, 112, 116, "Quadratus fem.");
        setMuscleGroup(color05, 116, 118, "Rectus femoris");
        setMuscleGroup(color06, 118, 119, "Sartorius prox.");
        setMuscleGroup(color06, 119, 120, "Sartorius dist.");
        setMuscleGroup(color07, 120, 121, "Semimembranosus");
        setMuscleGroup(color07, 121, 122, "Semitendinosus");
        setMuscleGroup(color17, 122, 125, "Soleus med.");
        setMuscleGroup(color17, 125, 128, "Soleus lat.");
        setMuscleGroup(color18, 128, 130, "Tensor fasc. l.");
        setMuscleGroup(color19, 130, 133, "Tibialis ant.");
        setMuscleGroup(color19, 133, 136, "Tibialis post. med.");
        setMuscleGroup(color19, 136, 139, "Tibialis post. lat.");
        setMuscleGroup(color20, 139, 145, "Vastus interm.");
        setMuscleGroup(color20, 145, 151, "Vastus lat. inf.");
        setMuscleGroup(color20, 151, 153, "Vastus lat. sup.");
        setMuscleGroup(color20, 153, 163, "Vastus med."); // TODO split?
    }

    private static void setMuscleGroup(string colorHex, int rangeStart, int rangeEnd, string groupName)
    {
        int groupIndex = groups.Count;
        for (int i = rangeStart; i < rangeEnd; i++)
        {
            muscleToGroupId[i] = groupIndex;
        }
        Color color;
        Color.TryParseHexString(colorHex, out color);
        groups.Add(new MuscleGroup(groupIndex, groupName, color));
    }

    public int index { get; private set; }
    public string name { get; private set; }
    public Color color { get; private set; }

    public MuscleGroup(int index, string name, Color color)
    {
        this.index = index;
        this.name = name;
        this.color = color;
    }
}
