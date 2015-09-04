using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MusclePart {

    public static int[] muscleToPartId { get; private set; }
    public static List<MusclePart> parts { get; private set; }

    static MusclePart()
    {
        muscleToPartId = new int[MuscleDataLoader.numMuscleElements];
        parts = new List<MusclePart>();

        // Kelly https://eleanormaclure.files.wordpress.com/2011/03/colour-coding.pdf
        string color01 = "FFB300"; //Vivid Yellow
        string color02 = "803E75"; //Strong Purple
        string color03 = "FF6800"; //Vivid Orange
        string color04 = "A6BDD7"; //Very Light Blue
        string color05 = "C10020"; //Vivid Red
        string color06 = "CEA262"; //Grayish Yellow
        string color07 = "817066"; //Medium Gray

        // Below not great for people with defective color vision
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

        setMusclePart(color01, 0, 6, "Adductor brev."); // TODO split?
        setMusclePart(color01, 6, 12, "Adductor long.");
        setMusclePart(color01, 12, 15, "Adductor magn. dist.");
        setMusclePart(color01, 15, 21, "Adductor magn. mid.");
        setMusclePart(color01, 21, 25, "Adductor magn. prox.");
        setMusclePart(color02, 25, 26, "Biceps femoris CL");
        setMusclePart(color02, 26, 29, "Biceps femoris CB");
        setMusclePart(color03, 29, 32, "Ext. dig. long.");
        setMusclePart(color03, 32, 35, "Ext. hal. long.");
        setMusclePart(color03, 35, 38, "Flex. dig. long.");
        setMusclePart(color03, 38, 41, "Flex. hal. long.");
        setMusclePart(color07, 41, 42, "Gastrocnemius lat.");
        setMusclePart(color07, 42, 43, "Gastrocnemius med.");
        setMusclePart(color08, 43, 44, "Gemellus inf.");
        setMusclePart(color08, 44, 45, "Gemellus sup.");
        setMusclePart(color09, 45, 51, "Glut. max. sup.");
        setMusclePart(color09, 51, 57, "Glut. max. inf.");
        setMusclePart(color09, 57, 63, "Glut. med. ant.");
        setMusclePart(color09, 63, 69, "Glut. med. post.");
        setMusclePart(color09, 69, 72, "Glut. min."); // TODO split?
        setMusclePart(color10, 72, 74, "Gracilis");
        setMusclePart(color11, 74, 77, "Iliacus lat.");
        setMusclePart(color11, 77, 80, "Iliacus mid.");
        setMusclePart(color11, 80, 83, "Iliacus med.");
        setMusclePart(color12, 83, 85, "Obt. ext. inf.");
        setMusclePart(color12, 85, 88, "Obt. ext. sup.");
        setMusclePart(color12, 88, 91, "Obtutator int.");
        setMusclePart(color13, 91, 95, "Pectineus");
        setMusclePart(color14, 95, 98, "Peroneus brev.");
        setMusclePart(color14, 98, 101, "Peroneus long.");
        setMusclePart(color14, 101, 104, "Peroneus tert.");
        setMusclePart(color15, 104, 105, "Piriformis");
        setMusclePart(color15, 105, 106, "Plantaris");
        setMusclePart(color15, 106, 108, "Popliteus");
        setMusclePart(color16, 108, 109, "Psoas minor");
        setMusclePart(color16, 109, 112, "Psoas major");
        setMusclePart(color04, 112, 116, "Quadratus fem.");
        setMusclePart(color05, 116, 118, "Rectus femoris");
        setMusclePart(color06, 118, 119, "Sartorius prox.");
        setMusclePart(color06, 119, 120, "Sartorius dist.");
        setMusclePart(color07, 120, 121, "Semimembranosus");
        setMusclePart(color07, 121, 122, "Semitendinosus");
        setMusclePart(color17, 122, 125, "Soleus med.");
        setMusclePart(color17, 125, 128, "Soleus lat.");
        setMusclePart(color18, 128, 130, "Tensor fasc. l.");
        setMusclePart(color19, 130, 133, "Tibialis ant.");
        setMusclePart(color19, 133, 136, "Tibialis post. med.");
        setMusclePart(color19, 136, 139, "Tibialis post. lat.");
        setMusclePart(color20, 139, 145, "Vastus interm.");
        setMusclePart(color20, 145, 151, "Vastus lat. inf.");
        setMusclePart(color20, 151, 153, "Vastus lat. sup.");
        setMusclePart(color20, 153, 163, "Vastus med."); // TODO split?
    }

    private static void setMusclePart(string colorHex, int rangeStart, int rangeEnd, string partName)
    {
        int groupIndex = parts.Count;
        for (int i = rangeStart; i < rangeEnd; i++)
        {
            muscleToPartId[i] = groupIndex;
        }
        Color color;
        Color.TryParseHexString(colorHex, out color);
        parts.Add(new MusclePart(groupIndex, partName, color));
    }

    public int index { get; private set; }
    public string name { get; private set; }
    public Color color { get; private set; }

    public MusclePart(int index, string name, Color color)
    {
        this.index = index;
        this.name = name;
        this.color = color;
    }
}
