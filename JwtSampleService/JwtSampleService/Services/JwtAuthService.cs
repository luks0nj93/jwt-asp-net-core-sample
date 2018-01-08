using System;
using System.IO;
using System.Security.Cryptography;
using JwtSampleService.Extensions;
using JwtSampleService.Settings;
using Microsoft.IdentityModel.Tokens;

namespace JwtSampleService.Services
{
    public class JwtAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private SecurityKey _issuerSigningKey;

        public JwtAuthService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
            InitializeRsa();
            InitializeJwtParameters();
        }

        public TokenValidationParameters Parameters { get; private set; }

        private void InitializeJwtParameters()
        {
            Parameters = new TokenValidationParameters()
            {
                IssuerSigningKey = _issuerSigningKey,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        private void InitializeRsa()
        {
            using (var publicRsa = RSA.Create())
            {
                var publicKeyXml = File.ReadAllText(_jwtSettings.RsaPublicKeyXml);
                RsaExtensions.FromXmlString(publicRsa, publicKeyXml);
                _issuerSigningKey = new RsaSecurityKey(publicRsa);
            }
        }
    }
}