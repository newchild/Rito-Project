using System;
using System.Text;

namespace RitoConnector
{
    internal static class Keyloader
    {
        private const string Key =
            "Vm0wd2QyUXlWa1pOVldoVFlteEtXRmxVU205V01WbDNXa2M1V0ZKc2JETlhhMUpUVmpGS2RHVkliRmhoTVhCUVdWZDRZV014WkhGUmJGWlhZbFV3ZUZadGNFdFRNVTVJVm10c2FsSnVRbGhXYlhoM1ZWWmFkRTFVVWxSTmJFcFlWVzAxVDJGV1NuTlhiR2hhWVRGYU0xVnNXbUZqYkhCRlZXeFNUbUpGY0VsV2JUQXhWREZrU0ZOclpHcFNWR3hoV1d4b1QwNUdVbkpYYlhSWVVqRktTVnBGV2s5VWJFcEhWMWhrVjFaRmIzZFhWbHBhWlZaT2NscEdhR2xTTW1oWlYxZDRiMVV3TUhoV2JrNVlZbFZhY1ZscmFFTlNiRnBZWlVaT2FGWnNjSHBaTUZaelZqSkdjbUV6YUZaaGExcG9Xa1ZhVDJOc2NFZGhSMnhUWVROQ2IxWXhaREJaVjFGNFZXdGtWMWRIYUZsWmJHaFRWMVpXY1ZKdFJsUldiRm93VkZaU1ExWlhTa2RqUmxwWFlsaFNlbFpxUVhoa1ZsWjBZVVp3YkdFeGNHOVhhMVpoVkRKT2RGTnJaRlJpVjNoVVZGY3hiMWRXV1hoWGJYUnNZWHBHV0Zac2FHOVdiVXBJVld4c1dtRXlhRVJaZWtaWFpFVTFWbFJzVW1sU01VbzFWakowYjJFeVJrZFRXR2hZWW0xNFdGUlhOVzlsYkZsM1YyeHdiR0pHV2pGV01uaGhZVWRGZUdOSE9WaGhNVnBvVmtSS1RtVldUbkphUmxKcFZqTm9WVlp0TURGUk1XUlhWMWhvV0dKRk5WUlVWbHB6VFRGU1ZtRkhPV2hpUlhBd1ZsZDRVMVl5UlhsVlZFSlhWak5vYUZacVJsZFhWbkJIVVd4YVYxSkZSVEU9";

        public static string GetRealKey()
        {
            var tempkey = Key;
            for (var i = 0; i < 10; i++)
            {
                tempkey = Base64Decode(tempkey);
            }
            return Base64Decode(tempkey);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}