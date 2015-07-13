using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class ModelParameterLoader {
    public static void LoadModel(string parameterFilePath, out FreeBodyModel model)
    {
        model = new FreeBodyModel();

        using (XmlTextReader reader = new XmlTextReader(File.OpenRead(parameterFilePath)))
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.

                        switch (reader.Name)
                        {
                            case "study_level_parameters":
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "study_name":
                                            model.studyName = reader.Value;
                                            break;
                                        case "responsible_person":
                                            model.responsiblePerson = reader.Value;
                                            break;
                                        case "output_directory_path_for_visualisation":
                                            model.geometryOutputPath = reader.Value;
                                            break;
                                        case "output_directory_path_for_optimisation":
                                            model.optimisationOutputPath = reader.Value;
                                            break;
                                    }
                                }
                                break;
                            case "universal_physical_parameters":
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "frames_per_second":
                                            model.framesPerSecond = int.Parse(reader.Value);
                                            break;
                                        case "radius_per_marker_metres":
                                            model.markerRadiusMetres = float.Parse(reader.Value);
                                            break;
                                    }
                                }
                                break;
                            case "subject":
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "subject_sex":
                                            model.sex = reader.Value;
                                            break;
                                        case "subject_height_metres":
                                            model.height = reader.Value;
                                            break;
                                        case "subject_mass_kg":
                                            model.mass = reader.Value;
                                            break;
                                        case "subject_anatomy_dataset_path":
                                            model.anatomyDatasetPath = reader.Value;
                                            break;
                                        case "subject_anatomy_dataset_file_name":
                                            model.anatomyDatasetFileName = reader.Value;
                                            break;
                                    }
                                }
                                break;
                            case "dynamic_trial_parameters":
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "start_frame_number":
                                            model.startFrame = int.Parse(reader.Value);
                                            break;
                                        case "end_frame_number":
                                            model.endFrame = int.Parse(reader.Value);
                                            break;
                                    }
                                }
                                break;
                        }
                        //Debug.Log("<" + reader.Name + ">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        //Debug.Log(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        //Debug.Log("</" + reader.Name + ">");
                        break;
                }
            }
        }

        model.rootPath = Path.GetDirectoryName(parameterFilePath);
    }
}