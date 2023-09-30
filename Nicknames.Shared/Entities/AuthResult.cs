using System;
using System.Collections.Generic;
using System.Text;

namespace Nicknames.Shared.Entities
{
    public class AuthResult
    {
        private bool _success;

        private Platform _platform;

        public bool HasPassed() => _success;

        public Platform GetPlatform() => _platform;

        public AuthResult(Platform platform, bool success)
        {
            _platform = platform;
            _success = success;
        }

        public AuthResult(bool success)
        {
            _success = success;
        }
    }

    public enum AuthResponse
    {
        TokenExpired = 0,
        AllAttemptsFailed = 1,
    }

    public class AuthResponseWrapper
    {
        public AuthResponse Response { get; set; }
    }
}