// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using E5R.Sdk.Bit.Services.Abstractions;

namespace System
{
    using Security.Cryptography;
    using Text;
    using System.Linq;

    public static class StringExtensions
    {
        #region Base64
        private static char[] _base64Chars = new[] {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '+', '/'
        };

        /// <summary>
        /// Encode string to Base64.
        /// </summary>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns>Base64 encoded string or <see cref="null" /></returns>
        public static string ToBase64(this string _this, IBitConfiguration config = null)
        {
            if (string.IsNullOrWhiteSpace(_this))
            {
                return null;
            }

            Encoding encoding = config?.DefaultEncoding ?? Encoding.Unicode;

            return Convert.ToBase64String(encoding.GetBytes(_this));
        }

        /// <summary>
        /// Dencode string from Base64.
        /// </summary>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns>String value or <see cref="null" /></returns>
        public static string FromBase64(this string _this, IBitConfiguration config = null)
        {
            if (string.IsNullOrWhiteSpace(_this))
            {
                return null;
            }

            if (!IsBase64String(_this))
            {
                throw new FormatException("Invalid Base64 string");
            }

            Encoding encoding = config?.DefaultEncoding ?? Encoding.Unicode;

            return encoding.GetString(Convert.FromBase64String(_this));
        }
        #endregion Base64

        #region Base64 Utils
        public static bool IsBase64String(this string _this)
        {
            if (_this.Length == 0
                || _this.Length % 4 != 0
                || _this.Contains(' ')
                || _this.Contains('\t')
                || _this.Contains('\r')
                || _this.Contains('\n'))
                return false;

            int index = _this.Length - 1;

            if (_this[index] == '=')
            {
                index--;
            }

            if (_this[index] == '=')
            {
                index--;
            }

            for (int i = 0; i <= index; i++)
            {
                if (!_base64Chars.Contains(_this[i]))
                    return false;
            }

            return true;
        }
        #endregion Base64 Utils

        #region Hash
        private const string HASH_NAME_SHA1 = "SHA1";
        private const string HASH_NAME_SHA256 = "SHA256";
        private const string HASH_NAME_SHA384 = "SHA384";
        private const string HASH_NAME_SHA512 = "SHA512";
        private const string HASH_NAME_MD5 = "MD5";

        /// <summary>
        /// Encode string to hash string.
        /// </summary>
        /// <param name="hashName">Hash name</param>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns>Hash string</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        internal static string ToHash(this string _this, string hashName, IBitConfiguration config = null)
        {
            if (string.IsNullOrWhiteSpace(_this))
            {
                return null;
            }

            if (string.IsNullOrEmpty(hashName))
            {
                throw new ArgumentNullException(nameof(hashName));
            }

            var validNames = new string[] {
                HASH_NAME_SHA1,
                HASH_NAME_SHA256,
                HASH_NAME_SHA384,
                HASH_NAME_SHA512,
                HASH_NAME_MD5
            };

            if (Array.IndexOf(validNames, hashName.ToUpperInvariant()) < 0)
            {
                throw new NotSupportedException(string.Format("Not supported hash lgoritm: {0}.", hashName));
            }

            Encoding encoding = config?.DefaultEncoding ?? Encoding.Unicode;

            return ((HashAlgorithm)CryptoConfig.CreateFromName(hashName))
                .ComputeHash(encoding.GetBytes(_this))
                .ToHexadecimalString();
        }

        /// <summary>
        /// Encode string to SHA1 hash string.
        /// </summary>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns>Hash string</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static string Sha1(this string _this, IBitConfiguration config = null) => ToHash(_this, HASH_NAME_SHA1, config);

        /// <summary>
        /// Encode string to SHA256 hash string.
        /// </summary>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns>Hash string</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static string Sha256(this string _this, IBitConfiguration config = null) => ToHash(_this, HASH_NAME_SHA256, config);

        /// <summary>
        /// Encode string to SHA384 hash string.
        /// </summary>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns>Hash string</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static string Sha384(this string _this, IBitConfiguration config = null) => ToHash(_this, HASH_NAME_SHA384, config);

        /// <summary>
        /// Encode string to SHA512 hash string.
        /// </summary>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns>Hash string</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static string Sha512(this string _this, IBitConfiguration config = null) => ToHash(_this, HASH_NAME_SHA512, config);

        /// <summary>
        /// Encode string to MD5 hash string.
        /// </summary>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns>Hash string</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static string Md5(this string _this, IBitConfiguration config = null) => ToHash(_this, HASH_NAME_MD5, config);

        /// <summary>
        /// Checks raw string combination with hash.
        /// </summary>
        /// <param name="hashValue">Hash value o compare</param>
        /// <param name="hashName">Hash name</param>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns><see cref="true" /> if combine, and <see cref="false" /> if not combine or raw string is null or empty.</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static bool CheckHash(this string _this, string hashValue, string hashName, IBitConfiguration config = null)
        {
            if (string.IsNullOrWhiteSpace(_this))
            {
                return false;
            }

            if (string.IsNullOrEmpty(hashName))
            {
                throw new ArgumentNullException(nameof(hashName));
            }

            string computedHash = _this.ToHash(hashName, config);

            return string.Compare(computedHash, hashValue, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Checks raw string combination with SHA1 hash.
        /// </summary>
        /// <param name="hashValue">Hash value o compare</param>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns><see cref="true" /> if combine, and <see cref="false" /> if not combine or raw string is null or empty.</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static bool Sha1Check(this string _this, string hashValue, IBitConfiguration config = null) => CheckHash(_this, hashValue, HASH_NAME_SHA1, config);

        /// <summary>
        /// Checks raw string combination with SHA256 hash.
        /// </summary>
        /// <param name="hashValue">Hash value o compare</param>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns><see cref="true" /> if combine, and <see cref="false" /> if not combine or raw string is null or empty.</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static bool Sha256Check(this string _this, string hashValue, IBitConfiguration config = null) => CheckHash(_this, hashValue, HASH_NAME_SHA256, config);

        /// <summary>
        /// Checks raw string combination with SHA384 hash.
        /// </summary>
        /// <param name="hashValue">Hash value o compare</param>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns><see cref="true" /> if combine, and <see cref="false" /> if not combine or raw string is null or empty.</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static bool Sha384Check(this string _this, string hashValue, IBitConfiguration config = null) => CheckHash(_this, hashValue, HASH_NAME_SHA384, config);

        /// <summary>
        /// Checks raw string combination with SHA512 hash.
        /// </summary>
        /// <param name="hashValue">Hash value o compare</param>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns><see cref="true" /> if combine, and <see cref="false" /> if not combine or raw string is null or empty.</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static bool Sha512Check(this string _this, string hashValue, IBitConfiguration config = null) => CheckHash(_this, hashValue, HASH_NAME_SHA512, config);

        /// <summary>
        /// Checks raw string combination with MD5 hash.
        /// </summary>
        /// <param name="hashValue">Hash value o compare</param>
        /// <param name="config">Optional <see cref="IBitConfiguration" /> instance</param>
        /// <returns><see cref="true" /> if combine, and <see cref="false" /> if not combine or raw string is null or empty.</returns>
        /// <exception cref="ArgumentNullException" />If <param name="hashName" /> is null or empty</exception>
        /// <exception cref="NotSupportedException" />If <param name="hashName" /> is invalid.</exception>
        public static bool Md5Check(this string _this, string hashValue, IBitConfiguration config = null) => CheckHash(_this, hashValue, HASH_NAME_MD5, config);
        #endregion Hash
    }
}
