namespace Light.AspNetCore.Hosting.Extensions
{
    internal static class SeparateLetterExtensions
    {
        internal static string ConvertName(this string name, string separate = "_")
        {
            string newName = "";

            for (int i = 0; i < name.Length; i++)
                if (char.IsUpper(name[i]))
                {
                    if (i == 0) // first char
                    {
                        newName += char.ToLower(name[i]);
                    }
                    else
                    {
                        newName += separate + char.ToLower(name[i]); // add prefix to upper chars
                    }
                }
                else
                    newName += name[i];

            return newName;
        }
    }
}
