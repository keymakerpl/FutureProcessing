using System;

namespace Infrastructure.Helpers
{
    public interface IPeselValidator
    {
        string Pesel { get; }

        void ChangePesel(string pesel);
        PeselStatuses CheckChars();
        PeselStatuses CheckCheckSum();
        PeselStatuses CheckDate();
        PeselStatuses CheckLength();
        PeselStatuses CheckSeries();
        PeselStatuses Verify();
        DateTime GetBirthDate();
    }
}