namespace CommonLib1.Models;

public sealed class Constants
{
    public enum ClientStatus
    {
        Pending,
        Paid,
    }

    public enum ClientServiceStatus
    {
        Pending,
        Serving,
        Completed
    }

    public enum ServiceBranch
    {
        Anabu,
        Manila
    }

    public enum ServiceDesigns
    {
        Simple,
        Complex,
        Intricate
    }

    public enum TimeSlotStatus
    {
        Available,
        BookedByYou,
        Full
    }

    public static readonly Dictionary<ServiceBranch, string> BranchNames = new()
    {
        { ServiceBranch.Anabu,  "Anabu Doyets Imus Cavite" },
        { ServiceBranch.Manila, "The Manila Residence Tower II TAFT Manila" }
    };

    public static readonly Dictionary<ServiceDesigns, List<string>> ImageUrls = new()
    {
        { ServiceDesigns.Simple,
        ["https://drive.google.com/file/d/1GopHQMcMOl9Nf-4ykt6yhLcu6TphUIW6/view?usp=drive_link",
         "https://drive.google.com/file/d/16PQiiau_CQUjvkwOfkN_4oX_zgZ92r4p/view?usp=drive_link",
         "https://drive.google.com/file/d/11lufMmvc0Ghp0tKZUOPsQuBhgXHcWsXi/view?usp=drive_link"] },

        { ServiceDesigns.Complex,
        ["https://drive.google.com/file/d/1D597q_kqRnQmveQbTtcLEvHG_T1h5-s_/view?usp=drive_link",
         "https://drive.google.com/file/d/1GaIOs86FUYeRjc-qjlaERZMATdM-Lmqb/view?usp=drive_link",
         "https://drive.google.com/file/d/1j5G5tzhvQtqxZnCUBKlO-QvRemDJI-V7/view?usp=drive_link"]},

        { ServiceDesigns.Intricate,
        ["https://drive.google.com/file/d/1PszevGwdn5juY66OutNrCTMm-Ez3rMVD/view?usp=drive_link",
         "https://drive.google.com/file/d/1yaIx28JE3cb62ATsEDyYzUlk-U3NJnYW/view?usp=drive_link",
         "https://drive.google.com/file/d/18cYcYJzypwVoVEmmAZsuAyzhg3UxyCSC/view?usp=drive_link"]}
    };
}
