using UnityEngine;

public class BuddyNameGenerator
{
    private static string name;

    private static readonly string[] CONSONANTS = new string[]
    {
        "L",
        "K",
        "D",
        "Z",
        "P",
        "J",
        "M",
        "N",
        "B",
        "R",
        "T",
    };

    private static readonly string[] PART1 = new string[]
    {
        "oo",
        "um",
        "owy",
        "ee",
        "al",
        "a",
        "op",
        "ant",
        "ash",
        "uck",
    };

    private static readonly string[] PART2 = new string[]
    {
        "ber",
        "beth",
        "bin",
        "dy",
        "der",
        "ders",
        "ly",
        "lo",
        "wen",
        "worth",
    };

    public static string GetName()
    {
        if (name == null)
        {
            name = GenerateName();
        }
        return name;
    }

    private static string GenerateName()
    {
        string name = "";
        name += CONSONANTS[Random.Range(0, CONSONANTS.Length)];
        name += PART1[Random.Range(0, PART1.Length)];
        name += PART2[Random.Range(0, PART2.Length)];
        return name;
    }
}
