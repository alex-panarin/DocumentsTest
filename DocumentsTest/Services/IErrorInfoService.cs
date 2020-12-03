using System;

namespace DocumentsTest.Services
{
    public interface IErrorInfoService
    {
        void ShowError(string error, string reason = default);
    }
}