using System.IO;
using System.Text;

public static class BinaryReaderExtension {

    /**
     * Original snippet at StackOverflow http://stackoverflow.com/a/27571627/1200090
     * by user TermWay http://stackoverflow.com/users/985714/termway
     * CC BY-SA 3.0 http://creativecommons.org/licenses/by-sa/3.0/
     */
    public static string ReadLine(this BinaryReader reader)
    {
        if (reader.IsEndOfStream())
            return null;

        StringBuilder result = new StringBuilder();
        char character;
        while (!reader.IsEndOfStream() && (character = reader.ReadChar()) != '\n')
        {
            if (character != '\r' && character != '\n')
            {
                result.Append(character);
            }
        }

        return result.ToString();
    }

    public static bool IsEndOfStream(this BinaryReader reader)
    {
        return reader.BaseStream.Position == reader.BaseStream.Length;
    }
}